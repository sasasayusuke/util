// 科目検索ダイアログ作成
{
    const dialogId = 'subjectSearch';
    create_searchDialog(dialogId,'TM科目',[
        {type:'number',id:'subjectCD',label:'科目CD',forColumnName:'科目CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'teamName',label:'科目名',forColumnName:'科目名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"科目CD",type:'',ColumnName:'科目CD'},
                {label:"科目名",type:'',ColumnName:'科目名'},
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
