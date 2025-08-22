// 部署検索ダイアログ作成
{
    const dialogId = 'departmentSearch';
    create_searchDialog(dialogId,'TM部署',[
        {type:'text',id:'departmentCD',label:'部署CD',forColumnName:'部署CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'departmentName',label:'部署名',forColumnName:'部署名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"部署CD",type:'',ColumnName:'部署CD'},
                {label:"部署名",type:'',ColumnName:'部署名'},
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
