// しまむら直納伝票請求書発行
{

    let category = "請求・売掛";
    let dialogId = "SIMAMURADirectDepositSlipDialog";
    let dialogName = "しまむら直納伝票請求書";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName,[
        { type: 'datepicker', id: 'requestDate', label: '請求日付', options: {
            width: 'normal',
            required:true,
            varidate:{type:'str'},
            format:"month"
        }},
        { type: 'number', id: 'deadLine', label: '締日', options: {
            width: 'normal',
            required:true,
            varidate:{type:'int',maxlength:2},

        }},
        { type: 'range-date', id: 'requestPeriod', label: '請求期間', options: {
            width: 'wide',
            disabled:true,
        }},
        { type: 'text', id: 'custmerCode', label: '得意先', options: {
            width: 'normal',
            digitsNum:4,
            varidate:{type:'zeroPadding',maxlength:4},
            lookupOrigin:{tableId:'TM得意先',keyColumnName:'得意先CD'},
            searchDialog:{id:'custmerNoSearch',title:'得意先検索',multiple:false},
            required:true
        }},
        { type: 'text', id: 'custmerName1', label: '', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名1',id:'custmerCode'}]
        }},
        { type: 'text', id: 'custmerName2', label: '', options: {
            width: 'wide',
            disabled:true,
            lookupFor:[{columnName:'得意先名2',id:'custmerCode'}]
        }},
        { type: 'text-set', id: 'voucherType', label: '伝票種類', options: {
            type:"number",
            width: 'wide',
            required:true,
            varidate:{type:'str',maxlength:1},
            values:[
                {value:'2',text:'直納'},
                {value:'3',text:'mail消耗品'},
                {value:'4',text:'mail直納'},
            ]
        }},
        { type: 'text-set', id: 'contentDistinction', label: '内容区分', options: {
            type:"number",
            width: 'wide',
            valueFrom:'0',
            required:true,
            values:[
                {value:'0',text:'なし'},
                {value:'1',text:'ﾃﾞｨﾊﾞﾛ'},
                {value:'2',text:'台湾'},
                {value:'3',text:'システム開発'},
            ]
        }},
        { type: 'text-set', id: 'postageOnly', label: '送料のみ', options: {
            type:"number",
            width: 'wide',
            valueFrom:'0',
            required:true,
            values:[
                {value:'0',text:'送料以外'},
                {value:'1',text:'送料のみ(Z012)'},
            ]
        }},
        { type: 'text-set', id: 'sortOrder', label: '並び順', options: {
            type:"number",
            width: 'wide',
            valueFrom:'0',
            required:true,
            values:[
                {value:'0',text:'伝票番号'},
                {value:'1',text:'納品日付'},
            ]
        }},
        { type: 'datepicker', id: 'publishingDate', label: '発行日付', options: {
            width: 'normal',
            varidate:{type:'str'},
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: '出力', options: {
            icon:'disk',
            onclick:`create_SIMAMURADirectDepositSlip_report('${category}','${dialogId}','${dialogName}','${btnLabel}')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)});

    // 請求期間入力
    $(document).on('blur',`#${dialogId}_deadLine`,function(){
        let date = $(`#${dialogId}_requestDate`);
        let deadline = $(`#${dialogId}_deadLine`);

        if(date.val() == ""){
            return;
        }
        if(deadline.val() == ""){
            return;
        }

        $(`#${dialogId}_requestPeriodFrom`).val(getFirstDateOfScope(date.val(),deadline.val(),formatDate(new Date())));
        $(`#${dialogId}_requestPeriodTo`).val(getLastDateOfScope(date.val(),deadline.val(),formatDate(new Date())));
        $(`#${dialogId}_publishingDate`).val(getLastDateOfScope(date.val(),deadline.val(),formatDate(new Date())));
    })
}
async function create_SIMAMURADirectDepositSlip_report(category,dialogId,dialogName,btnLabel){
    console.log(`start : ${dialogName}`);

    try{
        // 請求日付
        const billingDate =  $(`#${dialogId}_requestDate`);
        // 締日
        const deadline = $(`#${dialogId}_deadLine`);
        // 請求期間
        const billingPeriodFrom = $(`#${dialogId}_requestPeriodFrom`);
        const billingPeriodTo = $(`#${dialogId}_requestPeriodTo`);
        // 得意先
        const custmerCD = $(`#${dialogId}_custmerCode`);
        // 伝票種類
        const voucherType = $(`#${dialogId}_voucherTypeFrom`);
        // 内容区分
        const contentCategory = $(`#${dialogId}_contentDistinctionFrom`);
        // 送料のみ
        const postage = $(`#${dialogId}_postageOnlyFrom`);
        // 並び順
        const sort = $(`#${dialogId}_sortOrderFrom`);
        // 発行日付
        const issueDate = $(`#${dialogId}_publishingDate`);


        // バリデーション
        if(billingDate.val() == ""){
            billingDate.focus();
            alert('請求日付が未入力です');
            return;
        }

        if(deadline.val() == ""){
            deadline.focus();
            alert('締日が未入力です');
            return;
        }

        if(custmerCD.val() == ""){
            custmerCD.focus();
            alert('得意先CDが未入力です');
            return;
        }

        if(!["2","3","4"].includes(voucherType.val())){
            voucherType.focus();
            alert('伝票種類が未入力です');
            return;
        }

        if(contentCategory.val() == ""){
            alert('内容区分が未入力です');
            contentCategory.focus();
            return;
        }
        else if(!["0","1","2","3"].includes(contentCategory.val())){
            contentCategory.focus();
            alert('内容区分が不正です');
            return;
        }

        if(postage.val() == ""){
            alert('送料のみが未入力です');
            postage.focus();
            return;
        }
        else if(!["0","1"].includes(postage.val())){
            postage.focus();
            alert('送料のみが不正です');
            return;
        }

        if(sort.val() == ""){
            alert('並び順が未入力です。');
            sort.focus();
            return;
        }
        else if(!["0","1"].includes(sort.val())){
            sort.focus();
            alert('並び順が不正です');
            return;
        }

        if(issueDate.val() == ""){
            issueDate.focus();
            alert('発行日付が不正です');
            return;
        }

        // 請求期間を再計算
        billingPeriodFrom.val(getFirstDateOfScope(billingDate.val(),deadline.val(),formatDate(new Date())));
        billingPeriodTo.val(getLastDateOfScope(billingDate.val(),deadline.val(),formatDate(new Date())));

        const item_params = {
            "@iS請求日付":billingPeriodFrom.val(),
            "@iE請求日付":billingPeriodTo.val(),
            "@i得意先CD":custmerCD.val(),
            "@i伝票種類":voucherType.val(),
            "@iSM内容区分":contentCategory.val(),
            "@i請求書発行日付":issueDate.val(),
            "@i送料区分":postage.val(),
            "@i並び順区分":sort.val()
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        // ファイルネーム作成(請求書_SM直納-請求期間ToのYYMMDD-得意先CD.xlsx)
        let filename = `請求書_SM直納-${custmerCD.val()}-${billingPeriodTo.val().replaceAll('-','').slice(-6)}.xlsx`;

        try{
            await download_report(param,filename);
        }catch{
            return;
        }
        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }

}