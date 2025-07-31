// チーム検索ダイアログ作成
{
    const dialogId = 'teamSearch';
    create_searchDialog(dialogId,'TMチーム',[
        {type:'number',id:'teamCD',label:'チームCD',forColumnName:'チームCD',options:{
            width:'normal',
        }},
        {type:'text',id:'teamName',label:'チーム名',forColumnName:'チーム名',options:{
            width:'normal',
        }},
        
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"チームCD",type:'',ColumnName:'チームCD'},
                {label:"チーム名",type:'',ColumnName:'チーム名'},
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
