// 入金検索
{
    const dialogId = 'paymentSearch';
    create_searchDialog(dialogId,'TD入金',[
        {type:'range-date',id:'paymentRangeDate',label:'入金日付',forColumnName:'入金日付',options:{
            width:'normal',
        }},
        {type:'range-number',id:'paymentRangeNo',label:'入金No',forColumnName:'入金番号',options:{
            width:'normal',
            varidate:{type:'int',maxlength:15},
        }},
        {type:'range-number',id:'custmerCD',label:'得意先CD',forColumnName:'得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        {type:'text',id:'edaban',label:'枝番',forColumnName:'枝番',options:{
            value:'1',
            hidden:true
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"入金No",type:'',ColumnName:'入金番号'},
                {label:"入金日付",type:'',ColumnName:'入金日付'},
                {label:"得意先CD",type:'',ColumnName:'得意先CD'},
                {label:"得意先名1",type:'',ColumnName:'得意先名1'},
                {label:"得意先名2",type:'',ColumnName:'得意先名2'},
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
    ' ORDER BY 入金日付 desc, 入金番号, 得意先CD'
)
    
}
