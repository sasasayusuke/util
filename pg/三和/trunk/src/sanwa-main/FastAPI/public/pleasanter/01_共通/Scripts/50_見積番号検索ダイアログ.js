// 見積番号検索ダイアログ
{
    const dialogId = 'estimateNoSearch1';
    create_searchDialog(dialogId,'TD見積',[
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'text',id:"recipientCD",label:'納入先CD',forColumnName:'納入先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'recipientNoSearch',title:'納入先番号検索'}
            }},
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'見積件名',options:{
                width:"wide",
            }},
            {type:'range-text',id:"AllMoney",label:'見積金額(円)',forColumnName:'合計金額',options:{
                alignment:'end',
                width:"normal",
                varidate:{type:'int',maxlength:30},
            }},
            {type:'range-date',id:"estimateDate",label:'見積日',forColumnName:'見積日付',options:{
                width:"normal",
            }},
            { type: 'search-table', id: 'searchTable', label: 'アイウエオ', options: {
                sql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"請求日付",type:'date',ColumnName:'見積日付'},
                    {label:"種別",type:'',ColumnName:'物件種別',specialTerms:`CASE 物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END as 物件種別`},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"見積金額",type:'',ColumnName:'合計金額',specialTerms:'合計金額 + 出精値引 as 合計金額',alignment:'end'},//specialTermsはsqlのselectで計算処理が必要な時などに使う
                    {label:"出力日",type:'',ColumnName:'見積書出力日'},
                    {label:"発注日",type:'date',ColumnName:'注文発行日'},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px',
        'ORDER BY 見積日付 DESC, 見積番号 DESC'
    )
}
{
    const dialogId = 'estimateNoSearch2';
    create_searchDialog(dialogId,'TD見積',[
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'text',id:"recipientCD",label:'納入先CD',forColumnName:'納入先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'recipientNoSearch',title:'納入先番号検索'}
            }},
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'見積件名',options:{
                width:"wide",
            }},
            {type:'range-text',id:"AllMoney",label:'見積金額(円)',forColumnName:'合計金額',options:{
                alignment:'end',
                width:"normal",
                varidate:{type:'int',maxlength:30},
            }},
            {type:'range-date',id:"estimateDate",label:'見積日付',forColumnName:'見積日付',options:{
                width:"normal",
            }},
            { type: 'search-table', id: 'searchTable', label: '', options: {
                sql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"見積日付",type:'date',ColumnName:'見積日付'},
                    {label:"種別",type:'',ColumnName:'物件種別',specialTerms:`CASE 物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END as 物件種別`},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"見積金額",type:'',ColumnName:'合計金額',specialTerms:'合計金額 + 出精値引 as 合計金額',alignment:'end'},
                    {label:"出力日",type:'',ColumnName:'見積書出力日'},
                    {label:"発注日",type:'date',ColumnName:'注文発行日'},
                    {label:"売上状況",type:'',ColumnName:'売上状況',specialTerms:"売上状況=(CASE WHEN 売上日付 IS NULL THEN '' ELSE '売上済' END)"},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px',
        'ORDER BY 見積日付 DESC, 見積番号 ASC'

    )
}
{
    const dialogId = 'estimateNoSearch3';
    create_searchDialog(dialogId,'TD見積',[
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'text',id:"recipientCD",label:'納入先CD',forColumnName:'納入先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'recipientNoSearch',title:'納入先番号検索'}
            }},
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'MT.見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'見積件名',options:{
                width:"wide",
            }},
            {type:'range-text',id:"AllMoney",label:'見積金額(円)',forColumnName:'MT.合計金額 + MT.出精値引',options:{
                alignment:'end',
                varidate:{type:'int',maxlength:30},
                width:"normal",
            }},
            {type:'range-date',id:"appointedDay",label:'納期',forColumnName:'MT.納期S',options:{
                width:"normal",
            }},
            {type:'range-text',id:"supplierCD",label:'仕入先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'supplierSearch',title:'仕入先番号検索'}
            }},
            { type: 'search-table', id: 'searchTable', label: '', options: {
                individualSql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"納期",type:'date',ColumnName:'納期S'},
                    {label:"種別",type:'',ColumnName:'物件種別'},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"見積金額",type:'',ColumnName:'合計金額',alignment:'end'},
                    {label:"出力日",type:'',ColumnName:'見積書出力日'},
                    {label:"発注日",type:'date',ColumnName:'注文発行日'},
                    {label:"仕入状況",type:'',ColumnName:'仕入状況'},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px'
    )
}
{
    const dialogId = 'estimateNoSearch4';
    create_searchDialog(dialogId,'TD見積',[
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'text',id:"recipientCD",label:'納入先CD',forColumnName:'納入先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'recipientNoSearch',title:'納入先番号検索'}
            }},
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'見積件名',options:{
                width:"wide",
            }},
            {type:'range-text',id:"AllMoney",label:'見積金額(円)',forColumnName:'合計金額',options:{
                alignment:'end',
                width:"normal",
                varidate:{type:'int',maxlength:30},
            }},
            {type:'range-date',id:"appointedDay",label:'納期',forColumnName:'納期S',options:{
                width:"normal",
            }},
            { type: 'search-table', id: 'searchTable', label: '', options: {
                individualSql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"納期",type:'date',ColumnName:'納期S'},
                    {label:"種別",type:'',ColumnName:'物件種別',specialTerms:`CASE 物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END as 物件種別`},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"見積金額",type:'',ColumnName:'合計金額',specialTerms:"MT.合計金額 + MT.出精値引 AS 合計金額",alignment:'end'},
                    {label:"出力日",type:'',ColumnName:'見積書出力日'},
                    {label:"発注日",type:'date',ColumnName:'注文発行日'},
                    {label:"売上状況",type:'',ColumnName:'売上状況',specialTerms:"売上状況=(CASE WHEN MT.売上日付 IS NULL THEN '' ELSE '売上済' END)"},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px'
    )
}
{
    const dialogId = 'estimateNoSearch5';
    create_searchDialog(dialogId,'TD見積',[
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'USH.見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'range-date',id:"billingDate",label:'請求日付',forColumnName:'USH.請求日付',options:{
                width:"normal",
            }},
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'MT.得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'range-text',id:"AllMoney",label:'合計金額(円)',forColumnName:'USH.合計金額',options:{
                alignment:'end',
                width:"normal",
                varidate:{type:'int',maxlength:30},
            }},
            {type:'range-date',id:"estimateDate",label:'見積日付',forColumnName:'MT.見積日付',options:{
                width:"normal",
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'MT.見積件名',options:{
                width:"wide",
            }},
            { type: 'search-table', id: 'searchTable', label: 'アイウエオ', options: {
                individualSql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"請求日付",type:'date',ColumnName:'請求日付'},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"合計金額",type:'',ColumnName:'合計金額',alignment:'end'},
                    {label:"見積日付",type:'',ColumnName:'見積日付'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"発行日付",type:'date',ColumnName:'請求書発行日付'},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px'
    )
}
{
    const dialogId = 'estimateNoSearch6';
    create_searchDialog(dialogId,'TD見積',[
            {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'USH.見積番号',options:{
                width:'normal',
                digitsNum:6,
                varidate:{type:'int',maxlength:6},
            }},
            {type:'range-date',id:"billingDate",label:'請求日付',forColumnName:'USH.請求日付',options:{
                width:"normal",
            }},
            {type:'text',id:"custmerCD",label:'得意先CD',forColumnName:'MT.得意先CD',options:{
                width:"normal",
                varidate:{type:'int',maxlength:4},
                searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
            }},
            {type:'range-text',id:"AllMoney",label:'合計金額(円)',forColumnName:'USH.合計金額',options:{
                alignment:'end',
                width:"normal",
                varidate:{type:'int',maxlength:30},
            }},
            {type:'range-date',id:"estimateDate",label:'見積日付',forColumnName:'MT.見積日付',options:{
                width:"normal",
            }},
            {type:'text',id:"estimateName",label:'見積件名',forColumnName:'MT.見積件名',options:{
                width:"wide",
            }},
            { type: 'search-table', id: 'searchTable', label: 'アイウエオ', options: {
                individualSql:true,
                t_heads:[
                    {label:"見積番号",type:'text-set',ColumnName:'見積番号'},
                    {label:"請求日付",type:'date',ColumnName:'請求日付'},
                    {label:"得意先",type:'',ColumnName:'得意先CD'},
                    {label:"合計金額",type:'',ColumnName:'合計金額',alignment:'end'},
                    {label:"見積日付",type:'',ColumnName:'見積日付'},
                    {label:"見積件名",type:'',ColumnName:'見積件名'},
                    {label:"発行日付_mae",type:'date',ColumnName:'請求書発行日付'},
                ]
            }},
        ],
        [
            { type: 'button_inline', id: 'create', label: 'OK', options: {
                onclick:`resultInput('${dialogId}')`,
                icon:'disk'
            }},
        ],
        '1000px'
    )
}


const estimateNoSearch3_baseSQL = (e1,e2) =>{
    return `
        SELECT TOP 1000
            MT.見積番号, MT.納期S,
            CASE MT.物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'メンテ' WHEN 3 THEN '委託' END AS 物件種別,
            MT.得意先CD, MT.見積件名,
            MT.合計金額 + MT.出精値引 AS 合計金額,
            MT.見積書出力日,MT.注文発行日,
            明細件数 = ISNULL(MEICNT.明細件数,0),
            仕入済明細件数 = ISNULL(ZUMICNT.仕入済明細件数,0),
            仕入状況=(
                CASE
                    WHEN ISNULL(ZUMICNT.仕入済明細件数,0) = 0
                        THEN ''
                    WHEN IsNull(MEICNT.明細件数, 0) <= IsNull(ZUMICNT.仕入済明細件数, 0)
                        THEN '全数仕入'
                    WHEN IsNull(MEICNT.明細件数, 0) > IsNull(ZUMICNT.仕入済明細件数, 0)
                        THEN '一部仕入'
                END
            )
        FROM (
            SELECT
                MT.見積番号, MT.納期S, MT.物件種別, MT.得意先CD, MT.納入先CD, MT.見積件名,
                MT.合計金額,MT.出精値引, MT.見積書出力日,MT.注文発行日,MT.受注区分
            FROM
                TD見積 AS MT
            INNER JOIN
                TD見積シートM AS MS
            ON
                MT.見積番号 = MS.見積番号
            ${e2}
            GROUP BY
                MT.見積番号, MT.納期S, MT.物件種別, MT.得意先CD, MT.納入先CD, MT.見積件名,
                MT.合計金額,MT.出精値引, MT.見積書出力日,MT.注文発行日,MT.受注区分
        ) AS MT
        LEFT JOIN (
            SELECT
                MT.見積番号,
                仕入済明細件数 = SUM(CASE WHEN MS.発注数 <= ISNULL(SRU.仕入数量,0) THEN 1 ELSE 0 END)
            FROM
                TD見積 AS MT
            INNER JOIN
                TD見積シートM AS MS
            ON
                MT.見積番号 = MS.見積番号
            LEFT JOIN (
                SELECT
                    見積明細連番,
                    仕入数量 = Sum(仕入数量)
                From
                    TD仕入明細内訳
                GROUP BY
                    見積明細連番
            ) AS SRU
            ON
                MS.見積明細連番 = SRU.見積明細連番
            WHERE
                MS.発注数 <> 0
            GROUP BY
                MT.見積番号
        ) AS ZUMICNT
        ON
            MT.見積番号 = ZUMICNT.見積番号
        LEFT JOIN (
            SELECT
                MT.見積番号,明細件数 = COUNT(MS.見積番号)
            FROM
                TD見積 AS MT
            INNER JOIN
                TD見積シートM AS MS
            ON
                MT.見積番号 = MS.見積番号
            WHERE
                MS.発注数 <> 0
            GROUP BY
                MT.見積番号
        ) AS MEICNT
        ON
            MT.見積番号 = MEICNT.見積番号
        ${e1}
        AND MT.受注区分 = 1
        ORDER BY 
            MT.納期S DESC, MT.見積番号 DESC
    `;
}

const estimateNoSearch4_baseSQL = e =>{
    return `
        SELECT TOP 1000
            MT.見積番号, MT.納期S, MT.物件種別, MT.得意先CD, MT.見積件名,
            MT.合計金額 + MT.出精値引 AS 合計金額, MT.見積書出力日,MT.注文発行日,
            売上状況=(CASE WHEN MT.売上日付 IS NULL THEN '' ELSE '売上済' END),
            社内伝票扱い
        FROM
            TD見積 AS MT
        ${e}
            AND MT.受注区分 = 1
            AND MT.得意先CD <> '9999'
            AND MT.社内伝票扱い = 0
        ORDER BY
            MT.納期S DESC,
            MT.見積番号 DESC
    `;
}

const estimateNoSearch5_baseSQL = e =>{
    return `
        SELECT TOP 1000
            USH.見積番号, USH.請求日付,
            MT.得意先CD, USH.合計金額,MT.見積日付, MT.見積件名,
            USH.請求書発行日付
        FROM
            TD売上請求H AS USH
        INNER JOIN
            TD見積 AS MT
        ON
            USH.見積番号 = MT.見積番号
        ${e}
        ORDER BY 
            USH.請求日付 DESC, 
            USH.見積番号 DESC
    `;
}

const estimateNoSearch6_baseSQL = e =>{
    return `
        SELECT TOP 1000
            USH.見積番号, USH.請求日付,
            MT.得意先CD, USH.合計金額,MT.見積日付, MT.見積件名,
            USH.請求書発行日付
        FROM
            TD売上前請求H AS USH
        INNER JOIN
            TD見積 AS MT
        ON
            USH.見積番号 = MT.見積番号
        ${e}
        ORDER BY
            MT.見積日付 DESC,
            USH.見積番号 DESC
    `;
}

// イベント登録：納入先CD検索ダイアログボタンを押したときに得意先情報を入れる
// グローバル変数を増やしたくないので即時関数
(function(){
        ["1","2","3","4"].forEach(e => {
            $(document).on('click',`#estimateNoSearch${e}_recipientCDField .ui-icon-search`,function(){
                recipient_lookup($(this));
            })
        })
    }
)();

async function recipient_lookup(e){
    let custmerCD_id = e.prev().attr('id').replace('recipient','custmer');
    let custmerCD = $(`#${custmerCD_id}`).val();

    const cls_custmer = new ClsCustmer(custmerCD);
    const res = await cls_custmer.GetByData();

    if(!res){
        alert('得意先CDを入力して下さい');
        return;
    }

    const searchData = searchArr.filter(e => e.dialogId == 'recipientNoSearch')[0];
    openDialog_for_searchDialog(searchData.dialogId,e.prev().attr('id'),searchData.width,'納入先検索');
    $('#recipientNoSearch_custmerFrom').val(cls_custmer["custmerCD"]);
    $('#recipientNoSearch_custmerTo').val(cls_custmer["得意先名1"]);
}

