// 科目検索ダイアログ作成
{
    const dialogId = 'auxiliarySubjectSearch';
    create_searchDialog(dialogId,'TM補助科目',[
        {type:'text',id:'subjectCD',label:'科目CD',forColumnName:'科目CD',options:{
            width:'normal',
            hidden:true,
        }},
        // {type:'text',id:'subjectName',label:'科目名',forColumnName:'ClassB',options:{
        //     width:'normal',
        //     hidden:true,
        // }},
        {type:'text',id:'auxiliarySubjectCD',label:'補助科目CD',forColumnName:'補助CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
        }},
        {type:'text',id:'teamName',label:'補助科目名',forColumnName:'補助科目名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            t_heads:[
                {label:"補助科目CD",type:'',ColumnName:'補助CD'},
                {label:"補助科目名",type:'',ColumnName:'補助科目名'},
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


const auxiliarySubjectSearch_baseSQL = (e) => {
    return `
        select 補助CD,補助科目名 
        from TM補助科目 
        ${e} 
    `
}