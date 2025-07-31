// 担当者検索ダイアログ作成
{
    const dialogId = 'personSearch';
    create_searchDialog(dialogId,'TM担当者',[
        {type:'text',id:'personCD',label:'担当者CD',forColumnName:'担当者CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'personName',label:'担当者名',forColumnName:'担当者名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"担当者CD",type:'',ColumnName:'担当者CD'},
                {label:"担当者名",type:'',ColumnName:'担当者名'},
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
