// 買掛金集計
{
    const dialogId = 'kaikakeKakuninSearch';
    create_searchDialog(dialogId,'TD入金',[
        {type:'number',id:'deadLine',label:'締日',forColumnName:'SM.締日',options:{
            width:'normal',
            varidate:{type:'int',maxlength:2},
        }},
        {type:'range-text',id:'custmerCD',label:'仕入先CD',forColumnName:'SM.仕入先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"仕入先CD",ColumnName:'仕入先CD'},
                {label:"仕入先名",ColumnName:'仕入先名1'},
                {label:"締日",ColumnName:'締日'},
                {label:"開始日付",ColumnName:'支払開始日付'},
                {label:"終了日付",ColumnName:'支払終了日付'},
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

const kaikakeKakunin_baseSQL = e =>{
    return `
        SELECT SM.仕入先CD,TM.仕入先名1, SM.締日, SM.支払開始日付, SM.支払終了日付, SM.処理日
        FROM TM支払金額 AS SM
        INNER JOIN (SELECT SK.仕入先CD, Max(SK.支払日付) AS 支払日付
            FROM TM支払金額 AS SK
            GROUP BY SK.仕入先CD) AS QR
            ON (SM.支払日付 = QR.支払日付)
            AND (SM.仕入先CD = QR.仕入先CD)
        LEFT JOIN TM仕入先 AS TM ON SM.仕入先CD = TM.仕入先CD
        ${e}
        ORDER BY SM.仕入先CD
    `;
}

