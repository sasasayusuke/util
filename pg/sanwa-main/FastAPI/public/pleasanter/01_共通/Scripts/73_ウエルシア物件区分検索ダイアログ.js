// ウエルシア物件区分検索ダイアログ作成
{
    const dialogId = 'WPropertyClassificationSearch';
    create_searchDialog(dialogId,'TMウエルシア物件区分',[
        {type:'text',id:'WPropertyClassificationCD',label:'ウエルシア物件区分',forColumnName:'ウエルシア物件区分CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:15},
        }},
        {type:'text',id:'WPropertyClassificationName',label:'ウエルシア物件区分名',forColumnName:'ウエルシア物件区分名',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"ウエルシア物件区分",type:'',ColumnName:'ウエルシア物件区分CD'},
                {label:"ウエルシア物件区分名",type:'',ColumnName:'ウエルシア物件区分名'},
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
