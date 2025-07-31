// 売掛金集計
{
    const dialogId = 'siharaiSearch';
    create_searchDialog(dialogId,'TD支払',[
        {type:'range-date',id:'paymentDate',label:'支払日付',forColumnName:'支払日付',options:{
            width:'normal',
        }},
        {type:'range-text',id:'paymentNo',label:'支払番号',forColumnName:'支払番号',options:{
            width:'normal',
            varidate:{type:'int',maxlength:15},
        }},
        {type:'range-text',id:'suplierCD',label:'仕入先CD',forColumnName:'仕入先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"支払No",ColumnName:'支払番号'},
                {label:"支払日付",ColumnName:'支払日付'},
                {label:"仕入先CD",ColumnName:'仕入先CD'},
                {label:"仕入先名1",ColumnName:'仕入先名1'},
                {label:"仕入先名2",ColumnName:'仕入先名2'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: 'OK', options: {
            onclick:`resultInput('${dialogId}')`,
            icon:'disk'
        }},
    ],
    '750px',
    ' and 枝番 = 1 ORDER BY 支払日付 desc, 支払番号, 仕入先CD')
}
