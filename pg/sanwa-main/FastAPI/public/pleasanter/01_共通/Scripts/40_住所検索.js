function createAndAddAddressButton(itemLabel) {
    commonAddButtonToNextField(itemLabel, "FindAddressButton", "住所検索", findAddress, "ui-icon-circle-triangle-s")
}

async function findAddress() {
    showLoading()
    let postalCode = commonGetValue("郵便番号");
    const postalCodeRegex = /^\d{3}-\d{4}$/;
    if (!postalCodeRegex.test(postalCode)) {
        hideLoading();
        let message = "郵便番号の形式が正しくありません。例: 123-3456"
        alert(message)
        throw new Error(message);
    }
    // '-' を削除
    postalCode = postalCode.replace(/-/g, '');
    let data = await commonGetData(
        params["郵便番号マスタ"]["ID"],
        [params["郵便番号マスタ"]["都道府県名"], params["郵便番号マスタ"]["市区町村名"], params["郵便番号マスタ"]["町域名"]],
        {[params["郵便番号マスタ"]["郵便番号"]]: postalCode}
    )

    // 取得データの件数チェック
    if (data.length == 0) {
        let message = "取得データが0件です。"
        alert(message)
        throw new Error(message);
    } else if (data.length > 1) {
        postCodeSelect(data);
        hideLoading();
        return;
    }
    hideLoading();

    commonSetValue("住所1", data[0]["都道府県名"] + data[0]["市区町村名"] + data[0]["町域名"]);

}



async function postCodeSelect(records){
    console.log(records)
    choices = '';
    for(let record of records){
        let t = record["都道府県名"] + record["市区町村名"] + record["町域名"];
        choices += `<li class="ui-widget-content ui-selectee">${t}</li>`;
    }

    let dialogHTML = `
        <div id="postSelectDialog" style="display: none;">
            <!-- メイン箇所 -->
            <div id="postSelect" class="dialog ui-dialog-content ui-widget-content" style="display: block; width: auto; min-height: 87.5625px; max-height: none; height: auto;">
                <div class="field-vertical w600">
                    <p class="field-label">
                        <label></label>
                    </p>
                    <div class="field-control">
                        <div class="container-selectable">
                            <div class="wrapper h300">
                                <ol data-action="SearchDropDown" class="control-selectable ui-selectable applied" id="postOptions">
                                ${choices}
                                </ol>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="postSelectDialogBackground" class="ui-widget-overlay ui-front" style="z-index: 100;"></div>
    `;

    $('#Application').append(dialogHTML);

    $(`#postSelectDialog`).dialog({
        modal: false,
        width: "750px",
        resizable: false,
        title:'住所選択'
    });

    $('#postSelectDialog').prev().children('button').attr('id','postSelectDialogClose');

    $('#postSelectDialogClose').on('click',function(){
        $('#postSelectDialogBackground').remove();
        $('#postSelect').parent().remove();
    })

}


$(document).on('click','#postSelect li',function(){
    $('#postSelect li').removeClass('ui-selected');
    $(this).addClass('ui-selected');
    $p.set($p.getControl('ClassG'),$(this).text());
    $('#postSelect').parent().remove();
    $('#postSelectDialogBackground').remove();
})

$(document).on('mouseover','#postSelect li',function(){
    $('#postSelect li').removeClass('ui-selected');
    $(this).addClass('ui-selected');
})
$(document).on('mouseout','#postSelect li',function(){
    $('#postSelect li').removeClass('ui-selected');
})