// 物件ダイアログ作成
{
    const dialogId = 'propertyDataSearch';
    create_searchDialog(dialogId,"TD物件情報",[
        {type:'text',id:'custmerCD',label:'得意先CD',forColumnName:'得意先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
            searchDialog:{id:'custmerNoSearch',title:'得意先番号検索'}
        }},
        {type:'text',id:'recipientCD',label:'納入先CD',forColumnName:'納入先CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:4},
            searchDialog:{id:'recipientNoSearch',title:'納入先検索'}
        }},
        {type:'text',id:'personCD',label:'担当者CD',forColumnName:'担当者CD',options:{
            width:'normal',
            varidate:{type:'int',maxlength:5},
            searchDialog:{id:'personSearch',title:'担当者検索'}
        }},
        {type:'range-text',id:'propertyCD',label:'物件No',forColumnName:'物件番号',options:{
            width:'normal',
            varidate:{type:'int',maxlength:15},
        }},
        {type:'text',id:'propertyName',label:'物件名',forColumnName:'物件名',options:{
            width:'normal',
        }},
        {type:'range-date',id:'propertyRegistrationDate',label:'物件登録日',forColumnName:'物件登録日付',options:{
            width:'normal',
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            sql:true,
            t_heads:[
                {label:"物件番号",type:'',ColumnName:'物件番号'},
                {label:"登録日付",type:'',ColumnName:'物件登録日付'},
                {label:"得意先",type:'',ColumnName:'得意先CD'},
                {label:"納入先",type:'',ColumnName:'納入先CD'},
                {label:"物件名",type:'',ColumnName:'物件名'},
                {label:"更新日付",type:'',ColumnName:'登録変更日'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: 'OK', options: {
            onclick:`resultInput('${dialogId}')`,
            icon:'disk'
        }},
    ],
    "1000px",
    " ORDER BY 物件登録日付 DESC, 物件番号 DESC"
)
}

// 納入先CD検索ダイアログを開く際に得意先情報を検索ダイアログに登録
$('#propertyDataSearch_recipientCDField .ui-icon-search').on('click',async function(e){
    let custmerCD = $('#propertyDataSearch_custmerCD').val();

    const cls_custmer = new ClsCustmer(custmerCD);
    const res = await cls_custmer.GetByData();

    if(!res){
        alert('得意先CDを入力して下さい');
        return;
    }

    const searchData = searchArr.filter(e => e.dialogId == 'recipientNoSearch')[0];
    openDialog_for_searchDialog(searchData.dialogId,$(this).prev().attr('id'),searchData.width,'納入先検索');
    $('#recipientNoSearch_custmerFrom').val(cls_custmer["custmerCD"]);
    $('#recipientNoSearch_custmerTo').val(cls_custmer["得意先名1"]);
})