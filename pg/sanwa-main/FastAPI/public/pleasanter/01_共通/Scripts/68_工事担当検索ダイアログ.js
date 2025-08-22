// 工事担当ダイアログ作成
{
    const dialogId = 'constructionPersonSearch';
    create_searchDialog(dialogId,'',[
        {type:'text',id:'constructionPersonCD',label:'工事担当CD',forColumnName:'',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'constructionPersonName',label:'工事担当名',forColumnName:'',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            t_heads:[
                {label:"工事担当CD",type:'',ColumnName:''},
                {label:"工事担当名",type:'',ColumnName:''},
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
