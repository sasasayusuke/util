// 売掛消費税調整検索ダイアログ作成
{
    const dialogId = 'ReceivableSalesTaxAdjustmentEntrySearch';
    create_searchDialog(dialogId,'TD消費税調整',[
        {type:'range-date',id:'tyosei_date',label:'調整日付',forColumnName:'ND.調整日付',options:{
            width:'normal',
        }},
        {type:'range-number',id:'tyoseiCD',label:'調整番号',forColumnName:'ND.消費税調整番号',options:{
            width:'normal',
        }},
        {type:'range-number',id:'custmerCD',label:'得意先CD',forColumnName:'ND.得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"調整NO",ColumnName:'消費税調整番号'},
                {label:"調整日付",ColumnName:'調整日付'},
                {label:"得意先CD",ColumnName:'得意先CD'},
                {label:"得意先名1",ColumnName:'得意先名1'},
                {label:"得意先名2",ColumnName:'得意先名2'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: 'OK', options: {
            onclick:`resultInput('${dialogId}')`,
            icon:'disk'
        }},
    ])
    
}

const ReceivableSalesTaxAdjustmentEntrySearch_baseSQL = e =>{
    return `
        SELECT TOP 1000 ND.消費税調整番号, ND.調整日付, ND.得意先CD, TM.得意先名1, TM.得意先名2
        FROM TD消費税調整 AS ND
        INNER JOIN TM得意先 AS TM
        ON ND.得意先CD = TM.得意先CD
        ${e}
        ORDER BY ND.調整日付 desc, ND.消費税調整番号, ND.得意先CD
    `;
}

