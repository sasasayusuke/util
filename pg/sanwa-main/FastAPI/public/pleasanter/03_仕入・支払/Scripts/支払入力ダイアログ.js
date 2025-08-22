// 支払情報用のグローバル変数
let HNowdate_for_paymentEntry =null;
let HLastDate_for_paymentEntry=null;
let HFirstDate_for_paymentEntry=null;
let HAddDate_for_paymentEntry  = null;
let mode_for_paymentEntry = 1;
{
    // 支払入力
    const today = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DD')
    const category = "仕入・支払";
    const dialogId = "paymentEntry";
    const dialogName = "支払入力";
    createAndAddDialog(dialogId, dialogName, [
        { type: 'free', id: '', label: '', options: {
            width:'wide',
            newLine:true,
            str:`
                <div style="height:45px;"></div>
            `
		}},
        { type: 'datepicker', id: 'PaymentDate', label: '支払日付', options: {
            width:'wide',
            value:today,
            varidate:{type:'str'},
		}},
		{ type: 'number', id: 'PaymentNo', label: '支払番号', options: {
            width:'wide',
            disabled:true,
            lock:'支払番号',
		}},
		{ type: 'number', id: 'SupplierCD', label: '仕入先コード', options: {
            width:'wide',
            search:true,
            digitsNum:4,
            lookupOrigin:{tableId:'TM仕入先',keyColumnName:'仕入先CD'},
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false}
		}},
		{ type: 'text', id: 'custmerName1', label: '', options: {
            width:'wide',
			disabled:true,
            lookupFor:[{columnName:'仕入先名1',id:'SupplierCD'}]
		}},
		{ type: 'text', id: 'Sum', label: '合計金額', options: {
            alignment:'end',
            width:'wide',
            disabled:true
        }},
        { type: 'free', id: 'label', label: '支払情報', options: {
            width:'wide',
            newLine:true,
            str:`
                <div style="width: 110px; font-weight: bold; text-align: right; font-size: small; margin-bottom: 10px; height:30px; margin-top: 5px;">支払情報</div>

            `
		}},
        { type: 'text', id: 'paymentPeriod', label: '支払期間', options: {
            width:'wide',
            disabled:true
		}},
        { type: 'text', id: 'leftOver', label: '前残', options: {
            alignment:'end',
            width:'wide',
            disabled:true
		}},
        { type: 'text', id: 'stocking', label: '仕入', options: {
            alignment:'end',
            width:'wide',
            disabled:true
		}},
        { type: 'text', id: 'payment', label: '支払', options: {
            alignment:'end',
            width:'wide',
            disabled:true
		}},
        { type: 'text', id: 'remaining', label: '当残', options: {
            alignment:'end',
            width:'wide',
            disabled:true
		}},
        { type: 'free', id: 'buttons', label: '', options: {
            width:'wide',
            str:`
                <div class="both" style="margin-left:120px;">
                    <button id="paymentEntry_nowButton" style="margin-right:20px;" disabled>今回</button>
                    <button id="paymentEntry_beforeButton" disabled>前情報</button>
                    <button id="paymentEntry_afterButton" disabled>次情報</button>
                </div>
            `
		}},
        { type: 'free', id: '', label: '', options: {
            newLine:true,
            str:`
                <p></p>
            `
		}},
        { type: 'input-table', id: 'searchTable', label: '', options: {
            newLine:false,
            lineNumber:true,
            row:15,
            t_heads:[
                {label:'支払区分',ColumnName:'',type:'text-set',search:true,varidate:{type:'str',maxLength:1}},
                {label:'支払種別',ColumnName:'',disabled:true},
                {label:'支払金額',ColumnName:'',alignment:'end',varidate:{type:'int',maxLength:15}},
                {label:'手形期日/番号',ColumnName:'',type:'date',varidateFrom:{type:'str'},varidateTo:{type:'str',maxLength:10},disabled:true,to_disabled:true},
                {label:'備考',ColumnName:'',varidate:{type:'str',maxLength:30}},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'select', label: '選択', options: {
            icon:'disk',
            onclick:`openDialog_for_searchDialog('siharaiSearch','paymentEntry_PaymentNo',1000,'支払選択');`
        }},
        { type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'disk',
            hidden:true,
            onclick:`click_delete_for_paymentEntry('${category}','${dialogId}','${dialogName}')`
        }},
        { type: 'button_inline', id: 'stop', label: '中止', options: {
            icon:'disk',
            onclick:`click_stop_for_paymentEntry()`
        }},
        { type: 'button_inline', id: 'save', label: '保存', options: {
            icon:'disk',
            onclick:`saveClick_for_paymentEntry('${category}','${dialogId}','${dialogName}')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'1100','px',function(){
        $('.paymentEntry_searchTable_0').parent().css('width','150px');
        $('.paymentEntry_searchTable_1').parent().css('width','150px'); 
        $('.paymentEntry_searchTable_2').parent().css('width','150px'); 
        $('.paymentEntry_searchTable_3').parent().css('width','150px');
        $('#paymentEntry').prev().children('.ui-dialog-title').text('支払入力 -- 登録');
    })})
}

// 支払区分検索ダイアログ表示
$(document).on('click','#paymentEntry tbody .ui-icon-search',function(){
    openDialog_for_searchDialog('paymentCategorySearch',$(this).prev().attr('id'),'750px','支払区分検索');
})

// 中止ボタンの動作
function click_stop_for_paymentEntry(){
    if(confirm('現在の編集内容を破棄します。\nよろしいですか？')){
        initialize_for_paymentEntry();
    }
}

// 手形番号は半角文字のみ許可する
$(document).on('input','.paymentEntry_searchTable_3',function(){
    if(now_IME)return;
    let element = $(this);
    element.val(element.val().replace(/[^!-~\s]/g, ""));
})

// ルックアップ機能
$(document).on('blur',`.paymentEntry_searchTable_0`,async function(){
    try{
        // IME入力中だったら終了
        if(now_IME)return;

        const clear_items = ()=>{
            $(this).val('');
            $(this).nextAll().eq(2).val('');
            $(this).parent().nextAll().eq(0).children('input').val('');
            $(this).parent().nextAll().eq(1).children('input').val('');
            $(this).parent().nextAll().eq(2).children('input').val('');
            $(this).parent().nextAll().eq(3).children('input').val('');
            $(this).parent().nextAll().eq(2).children('input').attr('disabled',true);
        }

        if($(this).val() == '' || $(this).val().length != 1 || /[^0-9０-９]/g.test($(this).val())){
            clear_items();
            return;
        }

        // 初期化
        $(this).nextAll().eq(2).val('');
        $(this).parent().nextAll().eq(0).children('input').val('');
        $(this).parent().nextAll().eq(1).children('input').val('');
        $(this).parent().nextAll().eq(2).children('input').val('');
        $(this).parent().nextAll().eq(3).children('input').val('');

        const paymentCategory = convertFullWidthToHalfWidth($(this).val());
        $(this).val(paymentCategory);
        const sql = `SELECT 支払区分名, CASE 支払種別 WHEN '1' THEN '支払' WHEN '2' THEN '手形支払' WHEN '3' THEN '調整・値引' END AS 支払種別 FROM TM支払区分 WHERE 支払区分CD = '${paymentCategory}'`;
        try{
            var res = await fetchSql(sql);
        }
        catch(err){
            clear_items();
            $(this).focus();
            return;
        }

        res = res.results;

        if(res.length != 1){
            $(this).focus();
            clear_items();
            return;
        }

        $(this).nextAll().eq(2).val(res[0].支払区分名);
        $(this).parent().next().children('input').val(res[0].支払種別);

        // 支払い区分が２の場合は手形期日と番号を入力可能に変更
        if(res[0].支払種別 == '手形支払'){
            $(this).parent().nextAll().eq(2).children('input').attr('disabled',false);
        }
        else{
            $(this).parent().nextAll().eq(2).children('input').attr('disabled',true);
        }
    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error("支払入力",err);
        clear_items();
        return;
    }

})

$(document).on('click',`#paymentEntry_searchTable_inputTable tbody tr .textClear`,async function(){
    $(this).prevAll().eq(1).val('').trigger('input');
})

// 合計金額計算
$(document).on('blur','.paymentEntry_searchTable_2',async function(){
    await sleep(100);
    let items = Array.from($('.paymentEntry_searchTable_2'));
    let sum = items.reduce((a,b) => a + Number(convertFullWidthToHalfWidth($(b).val()).replaceAll(',','')),0);
    $('#paymentEntry_Sum').val(sum.toLocaleString());
})

// 初期化
function initialize_for_paymentEntry(){
    UnLockData("支払番号", SpcToNull($('#paymentEntry_PaymentNo').val(), 0));
    $('#paymentEntry input').val('');
    $('#paymentEntry_PaymentDate').val(new Date().toLocaleDateString('sv-SE'));
    $('.paymentEntry_searchTable_3').attr('disabled',true);
    $('#paymentEntry_nowButton').attr('disabled',true);
    $('#paymentEntry_beforeButton').attr('disabled',true);
    $('#paymentEntry_afterButton').attr('disabled',true);
    HNowdate_for_paymentEntry =null;
    HLastDate_for_paymentEntry=null;
    HFirstDate_for_paymentEntry=null;
    HAddDate_for_paymentEntry  = null;
    mode_for_paymentEntry = 1;
    $('#paymentEntry').prev().children('.ui-dialog-title').text('支払入力');
    $('#paymentEntry_delete').css('display','none');
    $('#paymentEntry').prev().children('.ui-dialog-title').text('支払入力 -- 登録');
}

// -------------------------------------------------------------------------------------------------------
// ↓ 支払い情報

// 支払情報を入力する関数
function setup_payment_for_paymentEntry(res){
    let tmp = formatDate_for_japanese(res.支払開始日付) + " ～ " + formatDate_for_japanese(res.支払終了日付)
    $('#paymentEntry_paymentPeriod').val(tmp);
    $('#paymentEntry_leftOver').val(res.前月残高.toLocaleString());
    $('#paymentEntry_stocking').val(res.仕入金額.toLocaleString());
    $('#paymentEntry_payment').val(res.支払金額.toLocaleString());
    $('#paymentEntry_remaining').val(res.当月残高.toLocaleString());

    HNowdate_for_paymentEntry = new Date(res.支払日付);

    // ボタンのアクティブ制御
    if(HNowdate_for_paymentEntry.getTime() == HLastDate_for_paymentEntry.getTime()){
        $('#paymentEntry_afterButton').attr('disabled',true);
    }else{
        $('#paymentEntry_afterButton').attr('disabled',false);
    }

    if(HNowdate_for_paymentEntry.getTime() == HFirstDate_for_paymentEntry.getTime()){
        $('#paymentEntry_beforeButton').attr('disabled',true);
    }else{
        $('#paymentEntry_beforeButton').attr('disabled',false);
    }
}

$(document).on('blur','#paymentEntry_custmerName1',function(){
    getFristShir_for_paymentEntry();//最初の支払を検索
    nowPaymentData_for_paymentEntry();//現在の支払を検索
})
$(document).on('click','#paymentEntry_nowButton',function(){
    nowPaymentData_for_paymentEntry();
})


// 今回の支払情報
async function nowPaymentData_for_paymentEntry(){
    try{
        let suplierCD = $('#paymentEntry_SupplierCD');

        if(suplierCD.val() =="")return;

        const sql = `
            SELECT MOTO.仕入先CD,MOTO.支払日付,支払開始日付,支払終了日付,前月残高,(支払金額+調整金額) AS 支払金額,
                (仕入金額+返品金額+訂正金額+消費税額) AS 仕入金額,当月残高
            FROM TM支払金額 AS MOTO
                Inner Join
                    (SELECT 仕入先CD, MAX(DISTINCT 支払日付) AS 支払日付
                        From TM支払金額
                        GROUP BY 仕入先CD
                    ) AS MAXDATE
                ON MOTO.仕入先CD = MAXDATE.仕入先CD
                AND MOTO.支払日付 = MAXDATE.支払日付
            WHERE MOTO.仕入先CD = '${suplierCD.val()}'
        `;
        let res = await fetchSql(sql);
        res = res.results;

        if(res.length != 1){
            $('#paymentEntry_nowButton').attr('disabled',true);
            $('#paymentEntry_beforeButton').attr('disabled',true);
            $('#paymentEntry_afterButton').attr('disabled',true);
            return;
        }
        else{
            $('#paymentEntry_nowButton').attr('disabled',false);
        };

        HLastDate_for_paymentEntry = new Date(res[0].支払日付);
        setup_payment_for_paymentEntry(res[0]);
    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
}

// 前情報
$(document).on('click','#paymentEntry_beforeButton',async function(){

    try{

        let suplierCD = $('#paymentEntry_SupplierCD');

        if(suplierCD.val() =="")return;
        const fromat_nowdate = HNowdate_for_paymentEntry.toLocaleString('sv-SE').replaceAll('-','/').replaceAll('T',' ');
        const sql = `
            SELECT MOTO.仕入先CD,MOTO.支払日付,支払開始日付,支払終了日付,前月残高,(支払金額+調整金額) AS 支払金額,
                (仕入金額+返品金額+訂正金額+消費税額) AS 仕入金額,当月残高
            FROM TM支払金額 AS MOTO
            WHERE 仕入先CD= '${suplierCD.val()}'
            AND 支払日付 =  (SELECT MAX(支払日付) AS 支払日付
                    FROM TM支払金額
                    WHERE 支払日付 < '${fromat_nowdate}'
                    AND 仕入先CD = '${suplierCD.val()}'
                )
        `;

        let res = await fetchSql(sql);
        res = res.results;

        if(res.length != 1){
            return;
        }

        setup_payment_for_paymentEntry(res[0]);
    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
})

// 次情報
$(document).on('click','#paymentEntry_afterButton',async function(){

    try{
        let suplierCD = $('#paymentEntry_SupplierCD');

        if(suplierCD.val() =="")return;

        const sql = `
            SELECT
                MOTO.仕入先CD,MOTO.支払日付,支払開始日付,支払終了日付,前月残高,(支払金額+調整金額) AS 支払金額,
                (仕入金額+返品金額+訂正金額+消費税額) AS 仕入金額,当月残高
            FROM
                TM支払金額 AS MOTO
            WHERE
                仕入先CD= '${suplierCD.val()}'
            AND
                支払日付 = (SELECT MIN(支払日付) AS 支払日付
                    FROM TM支払金額
                    WHERE 支払日付 > '${HNowdate_for_paymentEntry.toLocaleString('sv-SE').replaceAll('-','/').replaceAll('T',' ')}'
                    AND 仕入先CD = '${suplierCD.val()}'
                )
        `;

        let res = await fetchSql(sql);
        res = res.results;

        if(res.length != 1){
            return;
        }

        setup_payment_for_paymentEntry(res[0]);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
})

// 最初の支払
async function getFristShir_for_paymentEntry(){
    try{
        let suplierCD = $('#paymentEntry_SupplierCD');

        if(suplierCD.val() =="")return;

        const sql = `
            SELECT 仕入先CD, MIN(支払日付) AS 支払日付
            FROM TM支払金額
            WHERE 仕入先CD = '${suplierCD.val()}'
            GROUP BY 仕入先CD
        `;

        let res = await fetchSql(sql);
        res = res.results;

        if(res.length != 1)return;
        HFirstDate_for_paymentEntry = new Date(res[0].支払日付);

    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
}

// ↑ 支払情報
// -------------------------------------------------------------------------------------------------------
// 修正モード
let gGetuDate_for_paymentEntry = null;
$(document).on('blur',"#paymentEntry_PaymentNo",async function(){
    try{
        const paymentNo = $('#paymentEntry_PaymentNo');
        const paymentDate = $('#paymentEntry_PaymentDate');
        const suplierCD = $('#paymentEntry_SupplierCD');
        const suplierName = $('#paymentEntry_custmerName1');

        const query = `select * from TD支払 where 支払番号 = ${paymentNo.val()}`;
        try{
            var res = await fetchSql(query);
            const cls_dates = new clsDates('買掛月次更新日');
            await cls_dates.GetbyID();
            gGetuDate_for_paymentEntry = cls_dates.updateDate;
        }catch(e){
            return;
        }

        if(res["results"].length == 0){
            return;
        }

        res = res['results'];

        // データロック
        var Lockres = await LockData("支払番号", paymentNo.val());
        if (!Lockres){
            paymentNo.val("");
            return;
        }

        if(new Date(res[0].支払日付) <= gGetuDate_for_paymentEntry){
            alert('更新済みの為、修正できません。');
            initialize_for_paymentEntry();
            return;
        }

        paymentDate.val(new Date(res[0].支払日付).toLocaleDateString('sv-SE'));
        paymentNo.val(res[0].支払番号);
        suplierCD.val(res[0].仕入先CD);
        suplierName.val(res[0].仕入先名1);
        HAddDate_for_paymentEntry = new Date(res[0].初期登録日).toLocaleDateString('sv-SE');

        const get_paymentCategory = (e) => {
            switch(e){
                case 1:
                    return "支払";
                case 2:
                    return "手形支払";
                case 3:
                    return "調整・値引";
            }
        }

        // 明細セット
        for(i = 0;i < res.length; i++){
            $(`#paymentEntry_searchTable_${i}_0_From`).val(res[i].支払区分CD);
            $(`#paymentEntry_searchTable_${i}_0_To`).val(res[i].支払区分名);
            $(`#paymentEntry_searchTable_${i}_1 input`).val(get_paymentCategory(res[i].支払種別));
            $(`#paymentEntry_searchTable_${i}_2 input`).val(null_to_zero(res[i].支払金額));
            $(`#paymentEntry_searchTable_${i}_4 input`).val(null_to_zero(res[i].摘要名,""));
            if(res[i].手形期日 != null){
                $(`#paymentEntry_searchTable_${i}_3 input[type='date']`).val(res[i].手形期日);
                $(`#paymentEntry_searchTable_${i}_3 input[type='text']`).val(res[i].手形番号);
            }
            if(res[i].支払種別 == 2){
                // $('#receptEntry_before').attr('disabled',false);
                $(`#paymentEntry_searchTable_${i}_3 input[type='date']`).attr('disabled',false);
                $(`#paymentEntry_searchTable_${i}_3 input[type='text']`).attr('disabled',false);
            }

        }

        $('.paymentEntry_searchTable_2').trigger('blur');
        getFristShir_for_paymentEntry();//最初の支払を検索
        nowPaymentData_for_paymentEntry();//現在の支払を検索
        mode_for_paymentEntry = 2;

        if(res[0].支払更新FLG == 1){
            alert('支払処理済みデータです。');
        }

        $('#paymentEntry').prev().children('.ui-dialog-title').text('支払入力 -- 修正');
        $('#paymentEntry_delete').css('display','inline-block');


    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        initialize_for_paymentEntry();
        return;
    }
})

// ↑ 修正モード
// -------------------------------------------------------------------------------------------------------
// ↓ 登録処理

async function saveClick_for_paymentEntry(category,dialogId,dialogName){
    try{
        console.log('【start】支払入力')
        const res = await item_check_for_paymentEntry();
        if(!res[0]){
            return;
        }
        if(!confirm('保存します。')){
            return;
        }

        const item_params = {
            mode:mode_for_paymentEntry,
            paymentDate:$('#paymentEntry_PaymentDate').val(),
            paymentNo:$('#paymentEntry_PaymentNo').val(),
            SupplierCD:$('#paymentEntry_SupplierCD').val(),
            sumAll:Number(null_to_zero($('#paymentEntry_Sum').val().replaceAll(',',''),0)),
            input_tableData:res[1],
            HADDDate:HAddDate_for_paymentEntry
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": '保存',
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        
        initialize_for_paymentEntry();



    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
}


async function item_check_for_paymentEntry(){
    try{
        const paymentEntry = $('#paymentEntry_PaymentDate');
        const suplierCD = $('#paymentEntry_SupplierCD');

        if(paymentEntry.val() == ""){
            alert('明細が未入力です。');
            paymentEntry.focus();
            return false;
        }

        const cls_dates = new clsDates('買掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate_for_paymentEntry = cls_dates.updateDate;
        if(new Date(paymentEntry.val()) <= gGetuDate_for_paymentEntry){
            alert('更新済みの為、修正できません。');
            return [false];
        }

        if(suplierCD.val() == ""){
            alert('仕入先CDが未入力です。');
            return [false];
        }

        const change_paymentTypeNo = e =>{
            switch(e){
                case '支払':
                    return 1;
                case '手形支払':
                    return 2;
                case '調整・値引':
                    return 3;
            }
        }

        // 入力チェック
        const rows_count = $('#paymentEntry_searchTable_inputTable tbody tr').length;
        let firstFlg = true;
        const tableData = [];
        for(i = 0; i < rows_count; i++){
            const paymentCategory = $(`#paymentEntry_searchTable_${i}_0_From`);
            const paymentCategoryName = $(`#paymentEntry_searchTable_${i}_0_To`);
            const paymentType = $(`#paymentEntry_searchTable_${i}_1 input`);
            const paymentMoney    = null_to_zero($(`#paymentEntry_searchTable_${i}_2 input`),0);
            const tegataDate      = $(`#paymentEntry_searchTable_${i}_3 input[type="date"]`);
            const tegataNo        = $(`#paymentEntry_searchTable_${i}_3 input[type="text"]`);
            const remarks         = $(`#paymentEntry_searchTable_${i}_4 input`);

            if(firstFlg && paymentCategory.val() == ""){
                alert('支払区分CDが未入力です。');
                paymentCategory.focus();
                return [false];
            }
            if(paymentCategory.val()==""){
                break;
            }
            if(paymentMoney.val() == ""){
                alert('支払金額が未入力です。');
                paymentMoney.focus();
                return [false];
            }
            if(paymentType.val() == "手形支払" && tegataDate.val() == ""){
                alert(`手形期日 (${i+1}行目)を入力して下さい。`);
                tegataDate.focus();
                return [false];
            }
            if(!byte_check(`備考${i+1}行目`,remarks.val(),30)){
                remarks.focus();
                return [false];
            }
            tableData.push({
                paymentCategory:paymentCategory.val(),
                paymentCategoryName:paymentCategoryName.val(),
                paymentType:change_paymentTypeNo(paymentType.val()),
                paymentMoney:Number(SpcToNull(paymentMoney.val().replaceAll(',',''),0)),
                tegataDate:SpcToNull(tegataDate.val()),
                tegataNo:SpcToNull(tegataNo.val()),
                remarks:SpcToNull(remarks.val())
            });

            firstFlg = false;
        }
        return [true,tableData];

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return [false];
    }
}


// ↑ 登録処理
// -------------------------------------------------------------------------------------------------------
// ↓ 削除処理

async function click_delete_for_paymentEntry(category,dialogId,dialogName){
    try{
        if(!confirm('削除します。\nよろしいですか？')){
            return;
        }

        const paymentNo = $('#paymentEntry_PaymentNo').val();
        if(paymentNo == ""){
            return;
        }

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
        }catch{
            return;
        }
        initialize_for_paymentEntry();

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
}

$(document).on('click',`#paymentEntry_select`, function(){
    initialize_for_paymentEntry();
})