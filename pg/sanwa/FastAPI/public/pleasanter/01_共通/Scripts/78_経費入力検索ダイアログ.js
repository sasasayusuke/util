// 売掛消費税調整検索ダイアログ作成
{
    const dialogId = 'ExpenseEntrySearch';
    create_searchDialog(dialogId,'TD経費',[
        {type:'range-date',id:'expenseDate',label:'経費日付',forColumnName:'KH.経費日付',options:{
            width:'normal',
        }},
        {type:'range-text',id:'expenseCD',label:'経費番号',forColumnName:'KH.経費番号',options:{
            width:'normal',
            varidate:{type:'int',maxlength:15},
        }},
        {type:'range-text',id:'personCD',label:'担当者CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"経費番号",ColumnName:'経費番号'},
                {label:"経費日付",ColumnName:'経費日付'},
                {label:"科目",ColumnName:'科目名'},
                {label:"金額",ColumnName:'金額'},
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

const ExpenseEntrySearch_baseSQL = e =>{
    return `    
        SELECT TOP 1000 KH.経費番号, KH.経費日付, KM.科目名, KM.金額
        FROM TD経費 AS KH
        INNER JOIN
            (SELECT * FROM TD経費明細 WHERE 枝番 = 1) AS KM
        ON KH.経費番号 = KM.経費番号
        ${e}
        ORDER BY KH.経費日付 desc, KH.経費番号
    `;
}

