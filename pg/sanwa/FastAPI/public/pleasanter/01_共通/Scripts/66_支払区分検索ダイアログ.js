// 支払区分検索ダイアログ作成
{
    const dialogId = 'paymentCategorySearch';
    create_searchDialog(dialogId,'TM支払区分',[
        {type:'text',id:'paymentCategoryCD',label:'支払区分CD',forColumnName:'支払区分CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'paymentCategoryName',label:'支払区分名',forColumnName:'支払区分名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"支払区分CD",type:'',ColumnName:'支払区分CD'},
                {label:"支払区分名",type:'',ColumnName:'支払区分名'},
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
