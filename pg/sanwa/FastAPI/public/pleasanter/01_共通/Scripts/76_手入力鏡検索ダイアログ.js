// 手入力鏡検索
{
    const dialogId = 'mirrorSearch';
    create_searchDialog(dialogId,'TD入金',[
        {type:'range-date',id:'seikyu_date',label:'請求日付',forColumnName:'TD.請求日付',options:{
            width:'normal',
        }},
        {type:'range-text',id:'custmerCD',label:'得意先CD',forColumnName:'TD.得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        {type:'range-text',id:'estimateNo',label:'見積番号',forColumnName:'TM.見積番号',options:{
            width:'normal',
            varidate:{type:'int',maxlength:6},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"鏡番号",ColumnName:'鏡番号',hidden:true},
                {label:"請求日付",ColumnName:'請求日付'},
                {label:"得意先CD",ColumnName:'得意先CD'},
                {label:"得意先名1",ColumnName:'得意先名1'},
                {label:"得意先名2",ColumnName:'得意先名2'},
                {label:"見積番号",ColumnName:'見積番号'},
                {label:"金額",ColumnName:'合計',alignment:'end'},
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

const mirrorSearch_baseSQL = e =>{
    return `
        SELECT TOP 1000 GP.鏡番号,GP.請求日付,GP.得意先CD,GP.得意先名1,GP.得意先名2,UC.見積番号,GP.合計
        FROM (SELECT TD.鏡番号,TD.請求日付,TD.得意先CD,TD.得意先名1,TD.得意先名2,(TD.金額+TD.消費税) AS 合計
            FROM TD手入力鏡 AS TD
            LEFT JOIN TD手入力鏡内訳 TM ON TD.鏡番号 = TM.鏡番号
            ${e}
            GROUP BY TD.鏡番号,TD.請求日付,TD.得意先CD,TD.得意先名1,TD.得意先名2,(TD.金額+TD.消費税) ) AS GP
        LEFT JOIN (SELECT 鏡番号,見積番号 FROM TD手入力鏡内訳 WHERE 枝番=1) AS UC ON GP.鏡番号=UC.鏡番号
        ORDER BY GP.請求日付 DESC,GP.得意先CD,GP.得意先名1,GP.得意先名2,UC.見積番号
    `;
}

