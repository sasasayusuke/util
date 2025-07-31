// 仕入先検索ダイアログ作成
{
    const dialogId = 'supplierSearch';
    create_searchDialog(dialogId,'TM仕入先',[
        {type:'over-text',id:'supplierCD',label:'仕入先CD',forColumnName:'仕入先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
        }},
        {type:'text',id:'supplierName1',label:'仕入先名1',forColumnName:'仕入先名1',options:{
            width:'normal',
        }},
        {type:'text',id:'FURIGANA',label:'フリガナ',forColumnName:'フリガナ',options:{
            width:'normal',
            varidate:{type:'furigana'},
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"仕入先CD",type:'',ColumnName:'仕入先CD'},
                {label:"仕入先名1",type:'',ColumnName:'仕入先名1'},
                {label:"仕入先名2",type:'',ColumnName:'仕入先名2'},
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
