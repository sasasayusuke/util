// 得意先検索ダイアログ作成
{
    const dialogId = 'custmerNoSearch';
    create_searchDialog(dialogId,'TM得意先',[
        {type:'over-text',id:'custmerCD',label:'得意先CD',forColumnName:'得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        {type:'text',id:'custmerName1',label:'得意先名1',forColumnName:'得意先名1',options:{
            width:'normal',
        }},
        {type:'text',id:'furigana',label:'フリガナ',forColumnName:'フリガナ',options:{
            width:'normal',
            varidate:{type:'furigana'},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
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
    ]),
    '1000px',
    ' ORDER BY 得意先CD'
}
