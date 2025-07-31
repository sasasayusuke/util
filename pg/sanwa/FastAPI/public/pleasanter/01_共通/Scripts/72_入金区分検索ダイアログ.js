// 入金区分検索ダイアログ作成
{
    const dialogId = 'depositCategorySearch';
    create_searchDialog(dialogId,'TM入金区分',[
        {type:'text',id:'depositCategoryCD',label:'入金区分CD',forColumnName:'入金区分CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'depositCategoryName',label:'入金区分名',forColumnName:'入金区分名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"入金区分CD",type:'',ColumnName:'入金区分CD'},
                {label:"入金区分名",type:'',ColumnName:'入金区分名'},
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
