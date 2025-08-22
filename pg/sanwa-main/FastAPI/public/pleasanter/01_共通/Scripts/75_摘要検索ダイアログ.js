// ウエルシア物件区分検索ダイアログ作成
{
    const dialogId = 'summariesSearch';
    create_searchDialog(dialogId,'TM科目摘要',[
        {type:'text',id:'categoryCD',label:'科目CD',forColumnName:'科目CD',options:{
            width:'normal',
            disabled:true
        }},
        {type:'text',id:'summariesCD',label:'摘要CD',forColumnName:'摘要CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'summariesName',label:'摘要名',forColumnName:'摘要名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"摘要CD",type:'',ColumnName:'摘要CD'},
                {label:"摘要名",type:'',ColumnName:'摘要名'},
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
