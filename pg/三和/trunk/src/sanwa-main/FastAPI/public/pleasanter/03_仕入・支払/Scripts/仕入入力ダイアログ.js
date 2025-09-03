//仕入入力
{
    const category = '仕入・支払';
    const dialogId = "stockInput1";
    const dialogName = "仕入入力";
    const btnLabel = "明細内訳";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text-set', id: 'processCd', label: '処理区分', options: {
            type:"number",
            width:'wide',
            values:[
                { value: '0', text: '新規'},
                { value: '1', text: '修正' }
            ],
            valueFrom:'0',
            required:true,
            maxLength:1,
            varidate:{type:'int',maxlength:1},
        }},
        { type: 'text-set', id: 'estimate', label: '見積番号', options: {
            required:true,
            width:'wide',
            varidate:{type:'int',maxlength:6},
            digitsNum:6,
            lock:'見積番号',
            searchDialog:{id:'estimateNoSearch3',title:'見積番号検索',multiple:false},
            lookupOrigin:{tableId:"TD見積",keyColumnName:'見積番号',forColumnName:'見積件名'},
        }},
        { type: 'text-set', id: 'supplier', label: '仕入先', options: {
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false,focusTarget:false},
            lookupOrigin:{tableId:'TM仕入先',keyColumnName:'仕入先CD',forColumnName:'仕入先名1'},
            digitsNum:4,
            width:'wide',
            varidate:{type:'zeroPadding',maxlength:4},
        }},
        { type: 'text', id: 'totalAmount', label: '合計金額', options: {
            disabled:true,
            alignment:'end'
        }},
        { type: 'input-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            row:200,
            lineNumber:true,
            disabled:true,
            t_heads:[
                {label:"仕入先",type:'text',ColumnName:'仕入先',disabled:true},
                {label:"仕入先名",type:'text',ColumnName:'仕入先名',disabled:true},
                {label:"配送先",type:'text',ColumnName:'配送先',disabled:true},
                {label:"配送先名",type:'text',ColumnName:'配送先名',disabled:true},
                {label:"仕入金額",type:'text',ColumnName:'仕入金額',alignment:'end',disabled:true},
                {label:"仕入日付",type:'dateonly',ColumnName:'仕入日付',disabled:true},
                {label:btnLabel,type:'button',btn_label:btnLabel,disabled:true},
            ]
        }},
    ],
    []);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'970','px',function(){
        $('#stockInput1_searchTable_inputTable th').eq(1).css('width','60px');
        $('#stockInput1_searchTable_inputTable th').eq(2).css('width','250px');
        $('#stockInput1_searchTable_inputTable th').eq(3).css('width','60px');
        $('#stockInput1_searchTable_inputTable th').eq(4).css('width','250px');
        $('#stockInput1_searchTable_inputTable th').eq(5).css('width','150px');
        $('#stockInput1_searchTable_inputTable th').eq(6).css('width','100px');
        $('#stockInput1_searchTable_inputTable th').eq(7).css('width','80px');
    })});

    $(document).css('width',`#${dialogId}_totalAmount`,'130px');
    $(`.${dialogId}`).attr('disabled',true);

    // 見積番号が入力されたら
    $(document).on('blur',`#${dialogId}_estimateTo`,function(){
        clearSearchInputValue(dialogId);
        if($(this).val() != ""){
            search_table_for_stockInput(category, dialogId, dialogName, btnLabel, '通常伝');
        }
        
    })

    // $(document).on('blur',`#${dialogId}_estimateTo`,function(){
    //     clearSearchInputValue(dialogId)
    // })

    $(document).on('blur',`#${dialogId}_supplierTo`,function(){
        if($(this).val() != ""){
            search_supplier_for_stockInput(category, dialogId, dialogName, btnLabel);
        }else{
            clearSearchInputValue(dialogId);
            search_table_for_stockInput(category, dialogId, dialogName, btnLabel, '通常伝',true);
        }
        
    })
}

//仕入入力（社内伝）
{

    const category = '仕入・支払';
    const dialogId = "stockInput2";
    const dialogName = "仕入入力(社内伝用)";
    const btnLabel = "明細内訳";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'text-set', id: 'processCd', label: '処理区分', options: {
            type:"number",
            width:'wide',
            values:[
                { value: '0', text: '新規'},
                { value: '1', text: '修正' }
            ],
            valueFrom:'0',
            required:true,
            maxLength:1,
            varidate:{type:'int',maxlength:1},
        }},
        { type: 'text-set', id: 'estimate', label: '見積番号', options: {
            required:true,
            varidate:{type:'int',maxlength:6},
            width:'wide',
            digitsNum:6,
            lock:'見積番号',
            searchDialog:{id:'estimateNoSearch1',title:'見積番号検索',multiple:false},
            lookupOrigin:{tableId:"TD見積",keyColumnName:'見積番号',forColumnName:'見積件名'},
        }},
        { type: 'text-set', id: 'supplier', label: '仕入先', options: {
            searchDialog:{id:'supplierSearch',title:'仕入先検索',multiple:false},
            lookupOrigin:{tableId:'TM仕入先',keyColumnName:'仕入先CD',forColumnName:'仕入先名1'},
            digitsNum:4,
            width:'wide',
            varidate:{type:'zeroPadding',maxlength:4},
        }},
        { type: 'text', id: 'totalAmount', label: '合計金額', options: {
            disabled:true,
            alignment:'end'
        }},
        { type: 'input-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            row:200,
            lineNumber:true,
            disabled:true,
            t_heads:[
                {label:"仕入先",type:'text',ColumnName:'仕入先',disabled:true},
                {label:"仕入先名",type:'text',ColumnName:'仕入先名',disabled:true},
                {label:"配送先",type:'text',ColumnName:'配送先',disabled:true},
                {label:"配送先名",type:'text',ColumnName:'配送先名',disabled:true},
                {label:"仕入金額",type:'text',ColumnName:'仕入金額',alignment:'end',disabled:true},
                {label:"仕入日付",type:'dateonly',ColumnName:'仕入日付',disabled:true},
                {label:btnLabel,type:'button',btn_label:btnLabel,disabled:true},
            ]
        }},
    ],
    []);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'970','px',function(){
        $('#stockInput2_searchTable_inputTable th').eq(1).css('width','60px');
        $('#stockInput2_searchTable_inputTable th').eq(2).css('width','250px');
        $('#stockInput2_searchTable_inputTable th').eq(3).css('width','60px');
        $('#stockInput2_searchTable_inputTable th').eq(4).css('width','250px');
        $('#stockInput2_searchTable_inputTable th').eq(5).css('width','150px');
        $('#stockInput2_searchTable_inputTable th').eq(6).css('width','100px');
        $('#stockInput2_searchTable_inputTable th').eq(7).css('width','80px');
    })});

    $(`#${dialogId}_totalAmount`).css('width','130px');
    $(`.${dialogId}`).attr('disabled',true);

    $(document).on('blur',`#${dialogId}_estimateTo`,function(){
        clearSearchInputValue(dialogId)
        if($(this).val() != ""){
            search_table_for_stockInput(category, dialogId, dialogName, btnLabel, '社内伝');
        }
        
    })

    $(document).on('blur',`#${dialogId}_supplierTo`,function(){
        if($(this).val() != ""){
            search_supplier_for_stockInput(category, dialogId, dialogName, btnLabel) ;
        }else{
            clearSearchInputValue(dialogId);
            search_table_for_stockInput(category, dialogId, dialogName, btnLabel, '社内伝',true);
        }
        
    })

}

/**
 * 見積番号が入力されたら最初に動く関数
 * バリデーションを行う
 * 社内伝、通常伝共用
 * @param {string} category 処理区分
 * @param {string} dialogId ダイアログID
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタン名
 * @param {string} target 通常伝 or 社内伝　どちらから呼び出されたか
 * @param {boolean} executeLock_flg ロック（確認）処理を実行するか
 * @param {boolean} noAlert_flg 明細がない場合にalertを出すか判別するフラグ
 * @returns 
 */
let now_flg = false;
async function search_table_for_stockInput(category, dialogId, dialogName, btnLabel, target,executeLock_flg = false,noAlert_flg = false){
    try{
        if(now_flg)return;
        now_flg = true;
        const element_processCd = $(`#${dialogId}_processCdFrom`);
        const processCd = element_processCd.val();

        const element_estimateNo = $(`#${dialogId}_estimateFrom`);
        const estimateNo = element_estimateNo.val();

        // 仕入先を初期化
        $(`#${dialogId}_supplierFrom`).val('');
        $(`#${dialogId}_supplierTo`).val('');

        // 処理区分バリデーション
        if(processCd == null || processCd == ""){
            alert('処理区分が未入力です。');
            element_processCd.focus();
            return;
        }
        if(!["1","0"].includes(processCd)){
            alert('処理区分が正しくありません。');
            element_processCd.focus();
            return;
        }

        // 見積番号バリデーション
        if(estimateNo == ""){
            // alert('見積番号が未入力です。');
            // element_estimateNo.focus();
            return;
        }

        switch(await get_estimate_DB(estimateNo, target)){
            case -1:
                alert('指定の見積番号は存在しません。');
                break;
            case -2:
                alert('指定の見積番号は確定してません。');
                break;
            case -4:
                alert('指定の見積番号は社内伝票です。');
                break;
            case -5:
                alert('指定の見積番号は社内伝票ではありません。');
                break;
            case 0:
                switch(await download_for_stockInput(category, dialogId, dialogName, btnLabel)){
                    case -1:
                        if(!noAlert_flg){
                            alert('該当データなし');
                        }
                        element_processCd.val('0');
                        element_estimateNo.val('').trigger('blur');
                        element_estimateNo.focus();
                        return;
                    case 0:
                        if(!executeLock_flg){
                            if(!(await LockData('見積番号',estimateNo))){
                                item_delete_for_stockInput(dialogId,true,true);
                            }
                        }
                        return;
                    default:
                        element_estimateNo.val('').focus();
                        return;
                }
        }
    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }finally{
        await sleep(0);
        now_flg = false;
    }

}

/**
 * 仕入先が入力されたときの処理
 * @param {string} category カテゴリー
 * @param {string} dialogId ダイアログID
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタン名
 */
async function search_supplier_for_stockInput(category, dialogId, dialogName, btnLabel){
    try{
        // ルックアップを先に実行させる
        await sleep(0);

        const element_processCd = $(`#${dialogId}_processCdFrom`);
        const processCd = element_processCd.val();

        const element_estimateNo = $(`#${dialogId}_estimateFrom`)
        const estimateNo = element_estimateNo.val();

        // 処理区分バリデーション
        if(processCd == null || processCd == ""){
            alert('処理区分が未入力です。');
            element_processCd.focus();
            return;
        }
        if(!["1","0"].includes(processCd)){
            alert('処理区分が正しくありません。');
            element_processCd.focus();
            return;
        }

        // 見積番号バリデーション
        if(estimateNo == ""){
            alert('見積番号が未入力です。');
            element_estimateNo.focus();
            return;
        }

        switch(download_supplier_for_stockInput(category, dialogId, dialogName, btnLabel)) {
            case -1:
                alert('指定の仕入先は存在しません。');
                return;
        }

    }
    catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        throw err;
    }
}


/**
 * 見積番号を検索し、条件に適合しているかチェック
 * @param {number} estimateNo 見積番号
 * @param {string} target 通常伝 or 社内伝
 * @returns {number} 0以外はエラー
 */
async function get_estimate_DB(estimateNo, target = ""){

    let res_number;

    const sql = `
        SELECT
            見積番号, 見積件名, 得意先名1, 得意先名2, 現場名,
            納期S, 納期E,
            受注区分, 売上日付,
            社内伝票扱い,得意先CD
        FROM TD見積
        WHERE 見積番号 = ${estimateNo}
    `;

    let res = await fetchSql(sql);
    let records = res.results[0]
    if(res.count == 0 || records.length == 0) {
        return -1;
    }

    if(records["受注区分"] == 0){
        res_number = -2;
    }
    else{
        res_number = 0;
    }

    if(records["社内伝票扱い"] != 0 || records["得意先CD"] == "9999"){
        if(target == "通常伝"){
            res_number = -4;
        }
    }
    else{
        if(target == "社内伝"){
            res_number = -5;
        }
    }
    return res_number;
}

/**
 * 仕入明細を検索
 * @param {string} category カテゴリー
 * @param {string} dialogId ダイアログID
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタン名
 * @returns {number} 0以外はエラー
 */
async function download_for_stockInput(category, dialogId, dialogName, btnLabel){
    let res_number = -1;    // 返り値

    const processCd = $(`#${dialogId}_processCdFrom`).val();
    const estimateNo = $(`#${dialogId}_estimateFrom`).val();

    let param = {
        "storedname": "usp_HS0100仕入抽出",
        "params":{
            "@i処理区分":Number(processCd),
            "@i見積番号":Number(estimateNo),
        },
        output_params:{
            "@RetST":'INT',
            "@RetMsg":'VARCHAR(255)',
            "@RetMDataCnt":"INT",
            "@Ret得意先CD":"VARCHAR(4)",
            "@Ret大小口区分":"SMALLINT",
        }
    };

    let response = await fetchStored(param);
    if(response.count == 0) {
        return res_number;
    }
    else {
        res_number = 0;
    }

    // テーブルに値を登録
    SetupItems_for_stockInput(category, dialogId, dialogName, btnLabel, response)

    return res_number;
}


/**
 * 
 * @param {string} category カテゴリー
 * @param {string} dialogId ダイアログID
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタン名
 * @returns {number} 0以外はエラー
 */
async function download_supplier_for_stockInput(category, dialogId, dialogName, btnLabel) {

    const processCd = $(`#${dialogId}_processCdFrom`).val();
    const estimateNo = $(`#${dialogId}_estimateFrom`).val();
    const supplierNo = $(`#${dialogId}_supplierFrom`).val();

    let param = {
        "storedname": "usp_HS0100仕入抽出",
        "params":{
            "@i処理区分":Number(processCd),
            "@i見積番号":Number(estimateNo),
            "@i仕入先CD":supplierNo,
        },
        "output_params":{
            "@RetST":'INT',
            "@RetMsg":'VARCHAR(255)',
            "@RetMDataCnt":"INT",
            "@Ret得意先CD":"VARCHAR(4)",
            "@Ret大小口区分":"SMALLINT",
        }
    };

    let response = await fetchStored(param);
    if(response.count == 0) {
        return -2;
    }
    else{
        res_number= 0;
    }

    clearSearchInputValue(dialogId)
    SetupItems_for_stockInput(category, dialogId, dialogName, btnLabel, response)

    return res_number;
}


/**
 * 仕入明細情報をテーブルに入力する関数
 * @param {string} category カテゴリー
 * @param {string} dialogId ダイアログID
 * @param {string} dialogName ダイアログ名
 * @param {string} btnLabel ボタン名
 * @param {*} response 仕入明細情報
 */
function SetupItems_for_stockInput(category, dialogId, dialogName, btnLabel, response) {

    let records = response.results[0]
    let output = response.output_values

    const processCd = $(`#${dialogId}_processCdFrom`).val();
    const processName = $(`#${dialogId}_processCdTo`).val();

    const estimateNo = $(`#${dialogId}_estimateFrom`).val();
    const estimateTitle = $(`#${dialogId}_estimateTo`).val();

    

    // 合計金額
    const totalAmount = records.reduce((a,b) => a + Number(b.合計金額),0).toLocaleString()
    $(`#${dialogId}_totalAmount`).val(totalAmount);

    for(i = 0; i < records.length;i++){
        let record = records[i];

        let suppCd = record["仕入先CD"]
        $(`#${dialogId}_searchTable_${i}_0 input`).val(suppCd).attr('disabled',true);

        let suppName = record["仕入先名1"] + record["仕入先名2"]
        $(`#${dialogId}_searchTable_${i}_1 input`).val(suppName).attr('disabled',true);

        let delivCd = record["配送先CD"]
        $(`#${dialogId}_searchTable_${i}_2 input`).val(delivCd).attr('disabled',true);

        let delivName = record["配送先名1"] + record["配送先名2"]
        $(`#${dialogId}_searchTable_${i}_3 input`).val(delivName).attr('disabled',true);

        let total = Number(record["合計金額"]).toLocaleString()
        $(`#${dialogId}_searchTable_${i}_4 input`).val(total).attr('disabled',true);

        let saleDate = record["仕入日付"]
        if(saleDate != null){
            saleDate = saleDate.slice(0,10);
        }
        if(processCd == '0'){
            $(`#${dialogId}_searchTable_${i}_5 input`).val('').attr('disabled',true);
        }else{
            $(`#${dialogId}_searchTable_${i}_5 input`).val(saleDate).attr('disabled',true);
        }

        $(`#${dialogId}_searchTable_${i}_6 button`).prop('disabled', false).click(async function() {

            const params = new URLSearchParams();

            // 月次更新日取得
            const cls_date = new clsDates('買掛月次更新日');
            await cls_date.GetbyID();
            const gGetuDate = cls_date.updateDate;

            const button_element = $(this);

            // クエリパラメータを設定
            params.append("category", category);
            params.append("title", '仕入入力');
            // params.append("title", dialogName);
            params.append("button", btnLabel);
            params.append("user", $p.userName());
            params.append("opentime", dialog_openTime);
            params.append("permittion", $p.userName() == "Administrator" ? "100" : window.permittion["仕入入力"]);

            params.append("処理区分名", commonGetDivisionName(processName, processCd));
            params.append("見積件名", estimateTitle);
            params.append("仕入先名", suppName);
            params.append("配送先名", delivName);

            params.append("税集計区分", record["税集計区分"]);
            params.append("仕入端数", record["仕入端数"]);
            params.append("消費税端数", record["消費税端数"]);
            params.append("支払更新FLG", record["支払更新FLG"]);

            params.append("@i仕入日付",  commonGetDate(saleDate != null ? new Date(saleDate) : new Date()));
            params.append("@i支払日付",  commonGetDate(record["支払日付"] != null ? new Date(record["支払日付"]) : new Date()));

            params.append("@i処理区分", processCd);
            params.append("@i見積番号", estimateNo);
            params.append("@i仕入番号", record["仕入番号"]);
            params.append("@i仕入先CD", suppCd);
            params.append("@i配送先CD", delivCd);
            params.append("@i得意先CD", output["@Ret得意先CD"]);
            params.append("@i大小口区分", output["@Ret大小口区分"]);

            params.append("@i仕入先名1",  record["仕入先名1"]);
            params.append("@i仕入先名2",  record["仕入先名2"]);
            params.append("@i配送先名1",  record["配送先名1"]);
            params.append("@i配送先名2",  record["配送先名2"]);

            params.append("@i合計金額", record["合計金額"]);
            params.append("@i税抜金額", record["税抜金額"]);
            params.append("@i外税対象額", record["外税対象額"]);
            params.append("@i外税額", record["外税額"]);
            params.append("@i支払締日", record["締日"]);
            params.append("@i税集計区分", record["税集計区分"]);
            params.append("@i仕入端数", record["仕入端数"]);
            params.append("@i消費税端数", record["消費税端数"]);


            // ggetuDateFlg作成
            if(new Date(params.get('@i仕入日付')) <= gGetuDate || new Date(params.get('@i支払日付')) <= gGetuDate){
                params.append('gGetuDateFLG',true);
            }else{
                params.append('gGetuDateFLG',false);
            }

            // json文字列に変換し、sessionStorageにパラメータに保存
            const json_str = JSON.stringify(Object.fromEntries(params));
            // 遷移先での識別子（これをURLに入れて渡す）
            const session_storage_name = `siire_${new Date().getTime()}`;
            // sessionStorageに登録
            sessionStorage.setItem(session_storage_name,json_str);

            // 仕入番号のロックは入力画面側で行うため、ここではロックしない
            // if(!(await LockData('仕入番号',record['仕入番号']))){
            //     // UnLockData('見積番号',$(`#${dialogId}_estimateFrom`).val());
            //     return;
            // }

            // 子画面（仕入明細入力画面）を別タブで開く
            window.open(`/items/${INPUT_GUIS["仕入明細入力"]}/index?siire_id=${session_storage_name}`, '_blank');

            // 明細が閉じたら画面を初期化
            window.addEventListener('storage', async function(event) {
                if (event.key === `${session_storage_name}_delete` && event.newValue === 'false') {
                    localStorage.clear();

                    clearSearchInputValue(dialogId);
                    $('#stockInput1_supplierField').val('');
                    $('#stockInput1_totalAmount').val("");
                    // いったんロック解除
                    const estimateNo = $(`#stockInput${dialogName.includes('社内伝') ? '2':'1'}_estimateFrom`).val();
                    await UnLockData('見積番号',estimateNo);
                    search_table_for_stockInput(category,dialogId,dialogName,btnLabel,dialogName.includes('社内伝') ? '社内伝':'通常伝',false,true);
                }
            });
        })
    }
}

/**
 * 指定されたダイアログのテーブルをリセット
 * @param {string} dialogId ダイアログのID
 */
function clearSearchInputValue(dialogId) {
    // 全てのinputをクリア
    $(`#${dialogId}_searchTable_inputTable input`).val('');
    // ボタンのクリック関数を非活性
    $(`[id^=${dialogId}_searchTable_] button`).prop('disabled', true).off('click')
}


// 見積番号にfocusが当たったとき、ロックを解除して画面を初期化
$(document).on('focus','#stockInput1_estimateFrom,#stockInput2_estimateFrom',async function(e){
    if($(this).val() == "")return;
    const dialogId = $(this).closest('.dialog').attr('ID');
    item_delete_for_stockInput(dialogId);
})
// 処理区分にfocusが当たったとき、ロックを解除して画面を初期化
$(document).on('focus','#stockInput1_processCdFrom,#stockInput2_processCdFrom',async function(e){
    const dialogId = $(this).closest('.dialog').attr('ID');
    item_delete_for_stockInput(dialogId);
})

// 見積番号 or 処理区分にfocusが当たったときにunlockしていろいろ削除
/**
 * アンロックし、ダイアログをリセットする関数
 * @param {string} dialogId ダイアログのID
 * @param {boolean} lockFlg ロック解除するか　true:解除する false:解除しない
 * @param {boolean} focusFlg 見積番号項目にfocusを当てるか true:あてる false:あてない
 */
async function item_delete_for_stockInput(dialogId,lockFlg = false,focusFlg = false){
    const estimate_no = $(`#${dialogId}_estimateFrom`).val();
    if(estimate_no != "" && !lockFlg){
        UnLockData('見積番号',estimate_no);
    }
    if(focusFlg){
        $(`#${dialogId}_estimateFrom`).val('').focus();
    }else{
        $(`#${dialogId}_estimateFrom`).val('');
    }

    
    $(`#${dialogId}_estimateTo`).val('');
    $(`#${dialogId}_supplierFrom`).val('');
    $(`#${dialogId}_supplierTo`).val('');
    $(`#${dialogId}_totalAmount`).val('');

    clearSearchInputValue(dialogId);
}