// 売掛金集計
{
    const dialogId = 'urikakeKakuninSearch';
    create_searchDialog(dialogId,'TD入金',[
        {type:'number',id:'deadLine',label:'締日',forColumnName:'SM.締日',options:{
            width:'normal',
            varidate:{type:'int',maxlength:2},
        }},
        {type:'range-text',id:'custmerCD',label:'得意先CD',forColumnName:'SM.得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"得意先CD",ColumnName:'得意先CD'},
                {label:"得意先名",ColumnName:'得意先名1'},
                {label:"締日",ColumnName:'締日'},
                {label:"開始日付",ColumnName:'請求開始日付'},
                {label:"終了日付",ColumnName:'請求終了日付'},
                {label:"処理日付",ColumnName:'処理日'},
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

const urikakeKakunin_baseSQL = e =>{
    return `
        SELECT SM.得意先CD,TM.得意先名1, SM.締日, SM.請求開始日付, SM.請求終了日付, SM.処理日
            FROM TM請求金額 AS SM
            INNER JOIN (SELECT SK.得意先CD, Max(SK.請求日付) AS 請求日付
                                FROM TM請求金額 AS SK
                                GROUP BY SK.得意先CD) AS QR
                                ON (SM.請求日付 = QR.請求日付)
                                AND (SM.得意先CD = QR.得意先CD)
            LEFT JOIN TM得意先 AS TM ON SM.得意先CD = TM.得意先CD
            ${e}
            ORDER BY SM.得意先CD
    `;
}

