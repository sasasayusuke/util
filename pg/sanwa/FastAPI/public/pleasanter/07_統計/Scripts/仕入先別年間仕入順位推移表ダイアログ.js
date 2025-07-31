//仕入先別年間仕入順位推移表
{
    let category = "統計";
    let dialogId = "TabulationOfPurchasePayableDialog";
    let dialogName = "仕入先別年間仕入順位推移表";
    let btnLabel = "出力";

    createAndAddDialog(dialogId, dialogName, [
        { type: 'free', id: 'AggregateYear', label: '集計年', options: {
            str:`
                <div id="${dialogId}_AggregateYearField" class="field-wide both">
                    <p class="field-label"><label for="${dialogId}_AggregateYearFrom" class="SdtRequired">集計年</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="range-text-container" style="display: block;">
                                <input id="${dialogId}_AggregateYearFrom" name="${dialogId}_AggregateYearFrom" class="control-textbox range-text-input" type="number" style="width: 35%; margin:0;" onkeydown="return event.keyCode !== 69">
                                    <span class="range-text-separator" style="margin: 0;">年4月　～　</span>
                                <input id="${dialogId}_AggregateYearTo" name="${dialogId}_AggregateYearTo" class="control-textbox range-text-input" type="number" style="width: 35%; margin:0;" onkeydown="return event.keyCode !== 69">
                                    <span class="range-text-separator" style="margin: 0;">年3月</span>
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="${dialogId}_AggregateYear-error"></div>
                </div>
            `
        }},
    ],
    [
        { type: 'button_inline', id: 'output', label: btnLabel, options: {
            icon:'disk',
            onclick:`TabulationOfPurchasePayableDialog_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId)})
}

/**
 *
 * @param {string} dialogId     ダイアログID
 * @param {string} category     カテゴリー
 * @param {string} dialogName   ダイアログ名
 * @param {string} btnLabel     ボタンラベル
 * @returns
 */
async function TabulationOfPurchasePayableDialog_report(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log('start : 仕入先別年間仕入順位推移表');

        let fromDt = $(`#${dialogId}_AggregateYearFrom`).val();
        let toDt   = $(`#${dialogId}_AggregateYearTo`).val();

        if(fromDt == ""){
            alert('開始集計年が未入力です。');
            $(`#${dialogId}_AggregateYearFrom`).focus();
            return;
        }

        if(toDt == ""){
            alert('終了集計年が未入力です。');
            $(`#${dialogId}_AggregateYearTo`).focus();
            return;
        }

        if(fromDt > toDt){
            alert('集計年が不正です。');
            $(`#${dialogId}_AggregateYearFrom`).focus();
            return;
        }

        // 範囲チェック（2年以内）
        if(toDt - fromDt >= 10){
            alert('範囲が大きすぎます。（最大10年）');
        $(`#${dialogId}_AggregateYearFrom`).focus();
            return;
        }

        let param_fromDt = getFirstDateOfScope(fromDt + "/04/01", 99, fromDt + "/04/01");
        let param_toDt = getLastDateOfScope(toDt + "/03/01", 99, fromDt + "/03/01");

        let item_params = {
            "@iS集計日付":SpcToNull(param_fromDt.replaceAll('-','/')),
            "@iE集計日付":SpcToNull(param_toDt.replaceAll('-','/'))
        };

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params": item_params
        };


        let filename = "仕入先別年間仕入順位推移表.xlsx"

        try{
            await download_report(param,filename);
        }catch{
            return;
        }

        console.log('end : 仕入先別年間仕入順位推移表');

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}