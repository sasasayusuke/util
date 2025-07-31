// 納入先検索ダイアログ作成
{
    const dialogId = 'recipientNoSearch';
    create_searchDialog(dialogId,'TM納入先',[
        {type:'text-set',id:'custmer',label:'得意先',forColumnName:'得意先CD',options:{
            width:'normal',
            disabled:true,
        }},
        {type:'over-text',id:'recipientCD',label:'納入先CD',forColumnName:'納入先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        {type:'text',id:'recipientName1',label:'納入先名1',forColumnName:'納入先名1',options:{
            width:'normal',
        }},
        {type:'text',id:'furigana',label:'フリガナ',forColumnName:'フリガナ',options:{
            width:'normal',
            varidate:{type:'furigana'},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"納入先CD",type:'',ColumnName:'納入先CD'},
                {label:"納入先名1",type:'',ColumnName:'納入先名1'},
                {label:"納入先名2",type:'',ColumnName:'納入先名2'},
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
    ' ORDER BY 納入先CD'
)
    $(`#${dialogId}_custmerField`).removeClass('field-wide').addClass('field-normal');
    
}