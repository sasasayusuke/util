// 売上前請求書alert制御用
let sales_before_flg = false;
let dates;
// モーダル表示中フラグ
let uriage_modal_open = false;
//売上入力
{

    const category = '売上・入金';
    const dialogId = "salesInput";
    const dialogName = "売上入力";

    // const btnLabel = '出力';


    createAndAddDialog(dialogId, dialogName, [
        { type: 'text-set', id: 'processCategory', label: '処理区分', options: {
            newLine:true,
            values:[
                { value: '0', text: '新規'},
                { value: '1', text: '修正' }
            ],
            valueFrom:'0',
            required:true,
            maxLength:1,
            width:'wide',
            varidate:{type:'int',maxlength:1},
        }},
        { type: 'text-set', id: 'selectedExtractsCategory', label: '抽出区分', options: {
            values:[    
                { value: '0', text: '通常'},
                { value: '1', text: '仕入済' },
                { value: '2', text: '社内出し' },
            ],
            width:'wide',
            valueFrom:'0',
            required:true,
            maxLength:1,
            varidate:{type:'int',maxlength:1},
        }},
        { type: 'datepicker', id: 'stockMonth', label: '仕入月', options: {
            format:'month',
            varidate:{type:'str'},
        }},
        { type: 'text', id: 'totalAmount', label: '合計金額', options: {
            disabled:true,
            alignment:'end'
        }},
        { type: 'free', id: 'requesteIndication', label: '', options: {
            str:`
                <div id="salesInput_requesteIndication" class="field-normal" style="color: red; font-weight:bold; padding-left:120px"></div>
            `
        }},
        { type: 'text', id: 'estimateNo', label: '見積番号', options: {
            newLine:true,
            width: 'normal',
            required:true,
            digitsNum:6,
            lock:'見積番号',
            varidate:{type:'int',maxlength:6},
            lookupOrigin:{tableId:'TD見積',keyColumnName:'見積番号'},
            searchDialog:{id:'estimateNoSearch4',title:'見積番号検索',multiple:false}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名', options: {
            width: 'wide',
            disabled:true,
            // lookupFor:[{columnName:'見積件名',id:'estimateNo'}]
        }},
        { type: 'text-set', id: 'custmer', label: '得意先', options: {
            width: 'wide',
            disabled:true,
            // lookupFor:[{columnName:'得意先',id:'estimateNo'}]
        }},
        { type: 'free', id: 'delivery', label: '納入先', options: {
            width: 'wide',
            disabled:true,
            str:`
                <div id="salesInput_deliveryField" class="field-wide both">
                    <p class="field-label"><label for="salesInput_deliveryFrom" >納入先</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="text-set">
                                <input id="salesInput_deliveryFrom" name="salesInput_deliveryFrom" class="control-textbox inputText" type="text"disabled>
                                <input id="salesInput_deliveryTo1" name="salesInput_deliveryTo1" class="control-textbox inputText" type="text" disabled>
                                <input id="salesInput_deliveryTo2" name="salesInput_deliveryTo2" class="control-textbox disableText" type="text" disabled>
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="salesInput_delivery-error"></div>
                </div>
            `
        }},
        { type: 'text', id: 'deliveryDay', label: '納期', options: {
            width: 'wide',
            disabled:true,
            // lookupFor:[{columnName:'納期',specialTerms:`ISNULL(FORMAT(納期S, 'yyyy/MM/dd'), '') + (CASE WHEN 納期E IS NULL THEN '' ELSE '~' END) + ISNULL(FORMAT(納期E, 'yyyy/MM/dd'), '') as 納期`,id:'estimateNo'}]
        }},
        { type: 'text', id: 'siteName', label: '現場名', options: {
            width: 'wide',
            disabled:true,
            // lookupFor:[{columnName:'現場名',id:'estimateNo'}]
        }},
        { type: 'text-set', id: 'displayManager', label: '担当者', options: {
            width: 'wide',
            disabled:true,
        }},
        { type: 'free', id: '', label: '', options: {
            newLine:false,
            str:`
                <p></p>
            `
		}},
        { type: 'input-table', id: 'searchTable', label: 'アイウエオ', options: {
            individualSql:true,
            row:200,
            lineNumber:true,
            disabled:true,
            t_heads:[
                {label:"見積番号",type:'text',ColumnName:'見積番号',disabled:true},
                {label:"見積件名",type:'text',ColumnName:'見積件名',disabled:true,width:'400px'},
                {label:"売上金額",type:'text',ColumnName:'売上金額',disabled:true,alignment:'end'},
                {label:"売上日付",type:'text',ColumnName:'得意先CD',disabled:true},
                {label:"明細内訳",type:'button',btn_label:'明細内訳',disabled:true},
                {label:"売上番号",type:'text',ColumnName:'売上番号',disabled:true,hidden:true},
                {label:"税集計区分",type:'text',ColumnName:'税集計区分',disabled:true,hidden:true},
                {label:"売上端数",type:'text',ColumnName:'売上端数',disabled:true,hidden:true},
                {label:"消費税端数",type:'text',ColumnName:'消費税端数',disabled:true,hidden:true},
            ]
        }},
    ],
    [
        // { type: 'button_inline', id: 'output', label: btnLabel, options: {
        //     icon:'disk',
        //     onclick:
        // }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'1000','px',()=>{

        $('#salesInput_searchTable_inputTable th').eq(5).css('width','80px');
        $('#salesInput_searchTable_inputTable th').eq(6).hide();
        $('#salesInput_searchTable_inputTable th').eq(7).hide();
        $('#salesInput_searchTable_inputTable th').eq(8).hide();
        $('#salesInput_searchTable_inputTable th').eq(9).hide();
        $('td:has(".salesInput_searchTable_5")').hide();
        $('td:has(".salesInput_searchTable_6")').hide();
        $('td:has(".salesInput_searchTable_7")').hide();
        $('td:has(".salesInput_searchTable_8")').hide();

    })});

}

(function(){
    $('#salesInput_searchTable_searchButton').parent().hide();

    // lookupsに追加する（特殊パターンのため）
    let lookup = lookups.find(e => e.id == "salesInput_estimateNo");
    lookup.toParam = [
        {columnName:'見積件名',id:'salesInput_estimateName'},
        {columnName:'得意先CD',id:'salesInput_custmerFrom'},
        {columnName:'得意先名',id:'salesInput_custmerTo'},
        {columnName:'納入先CD',id:'salesInput_deliveryFrom'},
        {columnName:'納入先名1',id:'salesInput_deliveryTo1'},
        {columnName:'納入先名2',id:'salesInput_deliveryTo2'},
        {columnName:'納期',id:'salesInput_deliveryDay'},
        {columnName:'現場名',id:'salesInput_siteName'},
        {columnName:'担当者CD',id:'salesInput_displayManagerFrom'},
        {columnName:'担当者名',id:'salesInput_displayManagerTo'},
    ];
    console.log(lookup);
})();

const salesInput_baseSQL = (e)=>{
    return `
        SELECT MH.見積番号, MH.見積件名, MH.得意先CD, MH.得意先名1, MH.得意先名2,
            MH.納入得意先CD, MH.納入先CD, MH.納入先名1, MH.納入先名2,
            MH.現場名, MH.納期S, MH.納期E,
            MH.受注区分, MH.売上日付, MH.社内伝票扱い,
            MH.担当者CD, TN.担当者名
        FROM TD見積 AS MH
            LEFT JOIN TM担当者 AS TN
                ON MH.担当者CD = TN.担当者CD
        ${e}
    `;
};

/**
 * 画面初期化関数
 * @param {boolean} lock_flg trueが指定されたら見積番号をアンロック
 * @param {boolean} focus_Flg 見積番号にフォーカスするか true:する false:しない
 */
const lookup_blank_for_salesInput = (lock_flg = false,focus_Flg = false) =>{
    const estimate_no = $('#salesInput_estimateNo').val();
    if(lock_flg && estimate_no != "")UnLockData('見積番号',estimate_no);

    if(focus_Flg){
        $('#salesInput_estimateNo').val('').focus();
    }else{
        $('#salesInput_estimateNo').val('');
    }
    
    $('#salesInput_estimateName').val('');
    $('#salesInput_custmerFrom').val('');
    $('#salesInput_custmerTo').val('');
    $('#salesInput_deliveryFrom').val('');
    $('#salesInput_deliveryTo1').val('');
    $('#salesInput_deliveryTo2').val('');
    $('#salesInput_deliveryDay').val('');
    $('#salesInput_siteName').val('');
    $('#salesInput_displayManagerFrom').val('');
    $('#salesInput_displayManagerTo').val('');
    $('#salesInput_totalAmount').val('');
    $('#salesInput_searchTable_inputTable tbody input').val('');
    $('#salesInput_requesteIndication').text('');
    $('#salesInput_searchTable_inputTable button').attr('disabled',true).off('click');
    
}


/**
 * 見積番号が入力されたらルックアップする関数（22_ダイアログ共通.jsから呼出し）
 * @param {Array} tableData 見積のルックアップ用データ
 */
async function salesInput_lookup(tableData){

    $('#salesInput_requesteIndication').text('');

    // 渡された文字列が正しい日付か判断する関数
    const check_date =  (date) => Number.isNaN(date.getDate());

    const dateFormat_for_japanese = (date) =>{
        return date.getFullYear() + "年" + (date.getMonth()+1) + "月" + date.getDate() + "日";
    }

    // バリデーション
    let process_category = $('#salesInput_processCategoryFrom').val();              //処理区分
    let selection_category = $('#salesInput_selectedExtractsCategoryFrom').val();   //抽出区分
    if(!["0","1"].includes(process_category)){
        alert('処理区分を入力してください。');
        lookup_blank_for_salesInput();
        $('#salesInput_processCategoryFrom').focus();
        return;
    }
    if(!["0","1","2"].includes(selection_category)){
        alert('抽出区分を入力してください。');
        lookup_blank_for_salesInput();
        $('#salesInput_selectedExtractsCategoryFrom').focus();
        return;
    }
    if($('#salesInput_stockMonth').val() == "" && selection_category == 1){
        alert('仕入月を入力してください。');
        lookup_blank_for_salesInput();
        $('#salesInput_stockMonth').focus();
        return;
    }
    if($('#salesInput_selectedExtractsCategoryFrom').val() == 1 && $('#salesInput_stockMonth').val() == ""){
        alert('抽出仕入日付が未入力です。');
        $('#salesInput_stockMonth').focus();
        lookup_blank_for_salesInput();
        return;
    }

    if(tableData.results.length != 1){
        alert('指定の見積番号は存在しません。');
        lookup_blank_for_salesInput(false,true);
        return;
    }
    if(tableData.results[0].受注区分 == 0){
        alert('指定の見積番号は確定してません。');
        lookup_blank_for_salesInput(false,true);
        return;
    }
    if(tableData.results[0].得意先CD == '9999' || tableData.results[0].社内伝票扱い != 0){
        alert('社内在庫分なので売上処理できません。');
        lookup_blank_for_salesInput(false,true);
        return;
    }
    if(!(await LockData('見積番号',tableData.results[0].見積番号))){
        lookup_blank_for_salesInput(false,true);
        return;
    }

    tableData = tableData.results[0];

    // 日付を計算
    let date_from = !tableData.納期S ? "" :  dateFormat_for_japanese(new Date(tableData.納期S));
    let date_to = !tableData.納期E ? "" :  dateFormat_for_japanese(new Date(tableData.納期E));

    // 値を転記していく
    $('#salesInput_estimateName').val(tableData.見積件名);
    $('#salesInput_custmerFrom').val(tableData.得意先CD);
    $('#salesInput_custmerTo').val(tableData.得意先名1 + ' ' +tableData.得意先名2);
    $('#salesInput_deliveryFrom').val(tableData.納入得意先CD);
    $('#salesInput_deliveryTo1').val(tableData.納入先CD);
    $('#salesInput_deliveryTo2').val(tableData.納入先名1 + ' ' +tableData.納入先名2);
    $('#salesInput_deliveryDay').val(date_from + " ~ " + date_to);
    $('#salesInput_siteName').val(tableData.現場名);
    $('#salesInput_displayManagerFrom').val(tableData.担当者CD);
    $('#salesInput_displayManagerTo').val(tableData.担当者名);

    

    // テーブル検索
    search_salesInput_table(process_category);
}

/**
 * 指定された見積番号で売上明細を検索し、テーブル作成
 * @param {string} process_category 処理区分
 * @param {boolean} noAlert_flg 明細がない場合にalertを出すか判別するフラグ
 */
async function search_salesInput_table(process_category,noAlert_flg = false){
    // ストアド実行
    let fromDt;
    let toDt;
    let dt = $('#salesInput_stockMonth').val();
    const estimateNo = $('#salesInput_estimateNo').val();
    const extraction = $('#salesInput_selectedExtractsCategoryFrom').val();
    if(dt == ""){
        fromDt = "";
        toDt = "";
    }
    else{
        fromDt =    getFirstDateOfScope(dt,99,dt).replaceAll('-','/');
        toDt =      getLastDateOfScope(dt,99,dt).replaceAll('-','/');
    }
    let param = {
        "storedname": "usp_HD0700売上抽出",
        "params":{
            "@i処理区分":process_category,
            "@i抽出区分":extraction,
            "@i見積番号":estimateNo,
            "@is仕入日付":SpcToNull(fromDt,""),
            "@ie仕入日付":SpcToNull(toDt,""),
        },
        "output_params":{
            "@RetST":'INT',
            "@RetMsg":'VARCHAR(255)'
        }
    };

    let res = await fetchStored(param);
    let records = res.results[0]
    if(res.count == 0 || records.length == 0) {
        if(!noAlert_flg){
            alert('該当データなし');
        }
        lookup_blank_for_salesInput(true);
        $('#salesInput_processCategoryFrom').val('0').select();
        return;
    }

    // 月次更新日チェック
    const uriageseikyu_sql = `SELECT * FROM TD売上請求H WHERE 見積番号 = ${estimateNo} `;
    const uriageMaeSeikyu_sql = `SELECT * FROM TD売上前請求H WHERE 見積番号 = ${estimateNo} `;
    let uriageseikyuH;
    const cls_dates = new clsDates('売掛月次更新日');
    try{
        uriageseikyuH = await fetchSql(uriageseikyu_sql);
        uriageseikyuH = uriageseikyuH.results;
        
        var uriageMaeSeikyuH = await fetchSql(uriageMaeSeikyu_sql);
        uriageMaeSeikyuH = uriageMaeSeikyuH.results;

        await cls_dates.GetbyID();
    }
    catch(err){
        return;
    }
    
    dates = cls_dates.updateDate;

    if(uriageseikyuH.length != 0 && new Date(uriageseikyuH[0].請求日付) <= new Date(dates)){
        alert('請求日が更新済みの為、修正できません。');
        let requrested_html = `請求済み<br>　請求日付：${uriageseikyuH[0].請求日付.slice(0,10).replaceAll('-','/')}<br>　発行日付：${uriageseikyuH[0].請求書発行日付.slice(0,10).replaceAll('-','/')}`;
        $('#salesInput_requesteIndication').append(requrested_html);
        lookup_blank_for_salesInput(true,true);
        return;
    }

    if(uriageseikyuH.length != 0){
        let requrested_html = `請求済み<br>　請求日付：${uriageseikyuH[0].請求日付.slice(0,10).replaceAll('-','/')}<br>　発行日付：${uriageseikyuH[0].請求書発行日付.slice(0,10).replaceAll('-','/')}`;
        $('#salesInput_requesteIndication').html(requrested_html);
    }
    else{
        $('#salesInput_requesteIndication').val('');
    }

    if(uriageMaeSeikyuH.length > 0){
        // alertでfocusイベントに着火しクリア処理が走ってしまうためフラグを建てる
        sales_before_flg = true;
        alert('売上前請求書が発行済みです。');
        // 移行の処理をイベントループの最後に。
        await sleep(0);
        sales_before_flg = false;
    }

    // 先にボタンのクリック関数をけしておく
    $(`[id^=salesInput_searchTable_] button`).prop('disabled', true).off('click')
    for(i = 0; i < records.length;i++){
        let record = records[i];

        let col0 = record.見積番号
        $(`#salesInput_searchTable_${i}_0 input`).val(col0);

        let col1 = record.見積件名
        $(`#salesInput_searchTable_${i}_1 input`).val(col1);

        let col2 = Number(record.合計金額).toLocaleString()
        $(`#salesInput_searchTable_${i}_2 input`).val(col2);

        let col3 = process_category == 1  && record.売上日付 != null? record.売上日付.slice(0,10).replaceAll('-','/') : ""
        $(`#salesInput_searchTable_${i}_3 input`).val(col3);

        let col5 = record.売上番号;
        $(`#salesInput_searchTable_${i}_5 input`).val(col5);

        let col6 = record.税集計区分;
        $(`#salesInput_searchTable_${i}_6 input`).val(col6);

        let col7 = record.売上端数;
        $(`#salesInput_searchTable_${i}_7 input`).val(col7);

        let col8 = record.消費税端数;
        $(`#salesInput_searchTable_${i}_8 input`).val(col8);

        $(`#salesInput_searchTable_${i}_4 button`).prop('disabled', false).click(async function () {
            try{
                const params = new URLSearchParams();

                // ヘッダー
                const process_category = $('#salesInput_processCategoryFrom').val();
                const extraction_no = $('#salesInput_selectedExtractsCategoryFrom').val();
                const estimate_no = $('#salesInput_estimateNo').val();
                const estimate_name = $('#salesInput_estimateName').val();
                const custmer_no = $('#salesInput_custmerFrom').val();
                const custmer_name = $('#salesInput_custmerTo').val();
                const delivery_custmer_no = $('#salesInput_deliveryFrom').val();
                const delivery_no = $('#salesInput_deliveryTo1').val();
                const delivery_name = $('#salesInput_deliveryTo2').val();
                const stock_date = $('#salesInput_stockMonth').val();

                const sDate = getFirstDateOfScope(dt,99,dt);
                const eDate = getLastDateOfScope(dt,99,dt);

                // 該当行
                const sales_no = $(this).parent().next().children('input').val();
                let sales_date = $(this).parent().prev().children('input').val();
                if(sales_date == ""){
                    sales_date = new Date().toLocaleDateString('sv-SE');
                }else{
                    sales_date = sales_date.replaceAll('/','-');
                }
                const tax_category = $(this).parent().nextAll().eq(1).children('input').val();
                const sales_fraction = $(this).parent().nextAll().eq(2).children('input').val();
                const tax_fraction = $(this).parent().nextAll().eq(3).children('input').val();

                const button_element = $(this);

                // バリデーション
                if(estimate_no == ""){
                    alert('見積番号を入力して下さい。');
                    return;
                }

                params.append('category',"売上・入金");
                params.append('title','売上入力');
                params.append('button','明細内訳');
                params.append("user", $p.userName());
                params.append("opentime", dialog_openTime);

                params.append("@i処理区分",process_category);
                params.append("@i見積番号",estimate_no);
                params.append("@i売上番号",sales_no);
                params.append("@i抽出区分",extraction_no);
                params.append("@is仕入日付",sDate);
                params.append("@ie仕入日付",eDate);

                params.append("処理区分名", process_category == 0 ? '新規':'修正');
                params.append('抽出区分名',extraction_no == 0 ? '通常':extraction_no == 1? '仕入済':'社内出し');
                params.append("見積件名", estimate_name);
                params.append("得意先CD", custmer_no);
                params.append("得意先名", custmer_name);
                params.append("納入得意先CD", delivery_custmer_no);
                params.append("納入先CD", delivery_no);
                params.append("納入先名", delivery_name);
                params.append("売上日付", sales_date);
                params.append("税集計区分", tax_category);
                params.append("売上端数", sales_fraction);
                params.append("消費税端数", tax_fraction);

                // 更新フラグは常にtrueにして、売上明細入力.js内でリアルタイム判定させる（別タブ版と同じ動き）
                params.append('更新フラグ',true);
                // 判定用の月次更新日も渡す
                params.append('月次更新日', dates ? dates.toISOString() : null);

                if(process_category == '1'){
                    if(!(await LockData('売上番号',sales_no))){
                        // await UnLockData('見積番号',estimate_no);
                        // lookup_blank_for_salesInput();
                        return;
                    }
                }

                // パラメータオブジェクトを直接作成
                const modalParams = Object.fromEntries(params);

                // 売上明細入力画面をモーダルで開く
                openUriageModal(modalParams, process_category);
            }

            catch(err){
                alert(err)
                return;
            }
        });
    }

    // 合計計算
    $('#salesInput_totalAmount').val(
        records.reduce((a,b) =>a + b.合計金額,0).toLocaleString()
    );
}

// 見積番号横の×ボタンが押されたらテーブル明細も削除;
$(document).on('click','#salesInput .textClear',async function(){
    await sleep(0);
    lookup_blank_for_salesInput(true,true);
})

// 処理区分にフォーカスが入ったら画面を初期化
$(document).on('focus','#salesInput_processCategoryFrom',function(){
    if(!sales_before_flg && !uriage_modal_open){
        lookup_blank_for_salesInput(true);
    }
    
})
// 見積番号にフォーカスが入ったら画面を初期化
$(document).on('focus','#salesInput_estimateNo',async function(){
    if(!sales_before_flg && !uriage_modal_open){
        lookup_blank_for_salesInput(true);
    }
})

/**
 * 売上明細入力画面をモーダルで開く関数
 * @param {Object} modalParams パラメータオブジェクト
 * @param {string} process_category 処理区分
 */
async function openUriageModal(modalParams, process_category) {
    // モーダルのHTML構造を作成
    const modalId = 'uriageModal';
    const modalContainerId = 'uriageModalContainer';
    const modalHtml = `
        <div id="${modalId}" class="modal" style="display: none; position: fixed; z-index: 1000; left: 0; top: 0; width: 100%; height: 100%; overflow: hidden; background-color: rgba(0,0,0,0.5); display: flex; align-items: center; justify-content: center;">
                <div id="${modalContainerId}" style="width: 100%; height: 100%;">
                </div>
            </div>
        </div>
    `;
    
    // 既存のモーダルがあれば削除
    const existingModal = document.getElementById(modalId);
    if (existingModal) {
        existingModal.remove();
    }
    
    // モーダルをbodyに追加
    document.body.insertAdjacentHTML('beforeend', modalHtml);
    
    const modal = document.getElementById(modalId);
    // const closeBtn = modal.querySelector('.close');
    
    // モーダルを表示
    modal.style.display = 'block';
    
    // モーダル表示フラグをON
    uriage_modal_open = true;
    
    // 自動カナ変換のタイマーを停止
    if (typeof pauseAutoKanaTimers === 'function') {
        pauseAutoKanaTimers();
    }
    
    // フォーカス管理とタブトラップを設定
    setupModalFocus(modal);
    
    // 完了時のコールバック関数を定義
    const onComplete = async function() {
        closeUriageModal(modalId);
        
        $('#salesInput_searchTable_inputTable input').val('');
        search_salesInput_table(process_category, true);
    };

    // 閉じるボタンのイベント（React側で処理）
    // closeBtn.onclick = function() {
    //     closeUriageModal(modalId);
    // };
    
    // 売上明細入力.jsの機能を実行
    await loadUriageModalContent(modalParams, onComplete);
}

/**
 * 売上明細モーダルを閉じる関数
 * @param {string} modalId モーダルID
 */
function closeUriageModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        // Reactのrootをクリーンアップ
        const modalContainerId = 'uriageModalContainer';
        if (window.FormLib && window.FormLib.cleanupRoot) {
            window.FormLib.cleanupRoot(modalContainerId);
        }
        modal.style.display = 'none';
        modal.remove();
        
        // モーダル表示フラグをOFF
        uriage_modal_open = false;
        
        // 自動カナ変換のタイマーを再開
        if (typeof resumeAutoKanaTimers === 'function') {
            resumeAutoKanaTimers();
        }
    }
}

/**
 * モーダルのフォーカス管理とタブトラップを設定
 * @param {HTMLElement} modal モーダル要素
 */
function setupModalFocus(modal) {
    const modalContent = modal.querySelector('.modal-content');
    if (!modalContent) return;
    
    // フォーカス可能な要素を取得
    const focusableElements = modalContent.querySelectorAll(
        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    const firstFocusableElement = focusableElements[0];
    const lastFocusableElement = focusableElements[focusableElements.length - 1];
    
    // Escapeキーでモーダルを閉じる
    function handleKeyDown(e) {
        if (e.key === 'Escape') {
            const modalId = modal.id;
            if (modalId === 'uriageModal') {
                closeUriageModal(modalId);
            } else if (modalId === 'siireModal') {
                closeSiireModal(modalId);
            }
            return;
        }
        
        // Tabキーでのフォーカストラップ
        if (e.key === 'Tab') {
            if (e.shiftKey) {
                // Shift+Tab
                if (document.activeElement === firstFocusableElement) {
                    lastFocusableElement.focus();
                    e.preventDefault();
                }
            } else {
                // Tab
                if (document.activeElement === lastFocusableElement) {
                    firstFocusableElement.focus();
                    e.preventDefault();
                }
            }
        }
    }
    
    modalContent.addEventListener('keydown', handleKeyDown);
    
    // 初期フォーカスを設定
    if (firstFocusableElement) {
        firstFocusableElement.focus();
    } else {
        modalContent.focus();
    }
}

/**
 * 売上明細入力.jsの内容をモーダル内で実行する関数
 * @param {Object} modalParams パラメータオブジェクト
 * @param {Function} onComplete 完了時のコールバック関数
 */
async function loadUriageModalContent(modalParams, onComplete) {
    try {
        showLoading();

        await get_taxRate(modalParams['売上日付']);

        let itemParams = {
            "処理区分名": modalParams["処理区分名"],
            "抽出区分名": modalParams["抽出区分名"],
            "見積件名": modalParams["見積件名"],
            "得意先CD": modalParams["得意先CD"],
            "得意先名": modalParams["得意先名"],
            "納入得意先CD": modalParams["納入得意先CD"],
            "納入先CD": modalParams["納入先CD"],
            "納入先名": modalParams["納入先名"],
            "売上日付": modalParams['売上日付'],
            "更新フラグ": modalParams['更新フラグ'],

            "@i処理区分": modalParams["@i処理区分"],
            "@i見積番号": modalParams["@i見積番号"],
            "@i売上番号": modalParams["@i売上番号"],
            "@i抽出区分": modalParams["@i抽出区分"],
            "@is仕入日付": modalParams["@is仕入日付"],
            "@ie仕入日付": modalParams["@ie仕入日付"],
        };

        let fetchParams = {
            "category": modalParams["category"],
            "title": modalParams["title"],
            "button": modalParams["button"],
            "user": modalParams["user"],
            "opentime": modalParams["opentime"],
            "params": itemParams
        };

        let res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
            },
            body: JSON.stringify(fetchParams)
        });

        if (!res.ok) {
            const err = await res.json();
            throw new Error(err);
        }

        let context = await res.json();

        // viewsを作成
        const columns = context.content.columns;
        const view_names = columns[0].views;
        let views = {};
        view_names.forEach((e) => {
            views[e] = [];
        });
        
        columns.forEach((e) => {
            if (!('views' in e)) {
                for (const view in views) {
                    views[view].push(e);
                }
            } else {
                e.views.forEach((j) => {
                    views[j].push(e);
                });
            }
        });

        if (context["content"]["gUcnt"] != 0) {
            alert('原価売価未確定が存在します。');
        }

        // 作成したviewsをcontextに追加
        context.view_list = views;

        // Reactをマウントするための要素の取得
        const modalId = 'uriageModal';
        const modalContainerId = 'uriageModalContainer';
        const container = document.querySelector(`#${modalId} #${modalContainerId}`);

        // コンテナの初期化
        container.innerHTML = "";

        // Reactバンドルが読み込まれた後、window.FormLib があるはず
        if (window.FormLib) {
            try {
                // contextに必要な値を追加
                context.update_flg = modalParams['更新フラグ'] === 'true' || modalParams['更新フラグ'] === true;
                context.process_category = modalParams['@i処理区分'];
                
                // デバッグ用ログ
                console.log('売上入力 - modalParams:', {
                    更新フラグ: modalParams['更新フラグ'],
                    処理区分: modalParams['@i処理区分']
                });
                console.log('売上入力 - context値:', {
                    update_flg: context.update_flg,
                    process_category: context.process_category
                });
                
                // モーダル用のコンテキストを設定
                window.uriageModalContext = {
                    modalParams: modalParams,
                    onComplete: onComplete
                };
                window.FormLib.initFormUriage(container.id, context);
            } catch (error) {
                console.error('FormLib の初期化中にエラーが発生しました:', error);
            }
        } else {
            console.error('FormLib が見つかりません');
        }
        hideLoading();

    } catch (error) {
        console.error('予期せぬエラー：', error.message);
        alert('読み込み処理' + error.message);
        throw error;
    } finally {
        hideLoading();
    }
}

// 税率取得関数（売上明細入力.jsから移植）
async function get_taxRate(date) {
    try {
        var res = await GetTax(new Date(date));
        ZEIRITU = res.results[0].税率;
    } catch (e) {
        console.error("Error fetching tax:", e);
    }
}
