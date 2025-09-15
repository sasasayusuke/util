//lookupするための情報と、どの検索ダイアログを開くかの情報を格納
let lookups = [];

// 現在開いているダイアログの情報（古い順に入ってる）
let searchOutputId = [];

// 各検索ダイアログの情報。ダイアログ作成時に作成
let searchArr = [];

let now_lookup_flg = false;

// ロック項目
let lockItems = [];

// ダイアログが開かれた時間を格納
let dialog_openTime;

const styleElement = document.createElement('style');
styleElement.textContent = `
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 9999;
        display: none;
        justify-content: center;
        align-items: center;
    }

    .loading-spinner {
        width: 50px;
        height: 50px;
        border: 5px solid #f3f3f3;
        border-top: 5px solid #3498db;
        border-radius: 50%;
        animation: spin 1s linear infinite;
    }

    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }

    .searchDialog input:disabled,
    .dialog input:disabled,
    .dialog select:disabled,
    .dialog textarea:disabled {
        background-color: #f0f0f0;
        color: #888;
        cursor: not-allowed;
    }
    .dialog .error {
        border: 1px solid red;
    }
    .dialog .error-message {
        color: red;
        font-size: 0.8em;
        margin-top: 5px;
    }
    .field-wide {
        width: 100%;
        max-width:800px;
    }
    .field-normal {
        width: 60%;
        max-width:450px;
    }
    .both {
        clear: both;
        min-height:37px;
        padding-bottom:5px;
        height:37px;
    }
    .range-container {
        display: flex;
        align-items: center;
    }
    .range-input {
        flex-grow: 1;
        margin: 0 10px;
        -webkit-appearance: none;
        width: 100%;
        height: 5px;
        background: #d3d3d3;
        outline: none;
        opacity: 0.7;
        transition: opacity .2s;
    }
    .range-input:hover {
        opacity: 1;
    }
    .range-input::-webkit-slider-thumb {
        -webkit-appearance: none;
        appearance: none;
        width: 15px;
        height: 15px;
        background: #4CAF50;
        cursor: pointer;
        border-radius: 50%;
    }
    .range-input::-moz-range-thumb {
        width: 15px;
        height: 15px;
        background: #4CAF50;
        cursor: pointer;
        border-radius: 50%;
    }
    .range-label {
        min-width: 30px;
        text-align: center;
    }
    .range-text-container {
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
    .oneItem{
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
    .text-set{
        display: flex;
        align-items: center;
        justify-content: space-between;
    }
    .range-text-input {
        width: 45%;
    }
    .range-text-separator {
        margin: 0 10px;
    }
    .SdtRequired::after {
        content: "*";
        color: red;
        margin-left: 3px;
    }
    .radioButton{
        padding-top:7px;
    }
    .radioButton input[type="radio"]{
        vertical-align: sub;
    }
    .inputText{
        width:20%;
    }
    .disableText{
        width:80%;
    }
    .SdtTime{
        /* position: absolute;
        top:9px;
        right:-17px;
        z-index: 10; */
        cursor:pointer;
    }
    .ui-icon-search{
        position: absolute;
        top:9px;
        right:-17px;
        z-index: 10;
        cursor:pointer;
    }
    .ui-icon-close{
        position: absolute;
        top:9px;
        right:-33px;
        z-index: 10;
        cursor: pointer;
    }
    .search{
        position:static;
    }
    .button-small {
        background: linear-gradient(145deg, #f8f8f8, #efefef);
        border-radius: 5px;
        color: #555;
        text-align: center;
        cursor: pointer;
        transition: all 0.15s ease;
    }
    .button-small:hover {
        background: linear-gradient(145deg, #f2f2f2, #e8e8e8);
    }
    .button-small:active {
        background: linear-gradient(145deg, #e8e8e8, #f2f2f2);
        transform: translateY(0.5px);
    }
    td > input:focus{
        border:none;
        background-color:white;
    }
    td > input:focus-visible {
        outline:none;
        box-shadow:none;
    }
    tr:focus{
        border:none;
        background-color: #d3d3d3;
    }
    tr:focus-visible{
        outline: none;
    }
    .table {
        border-collapse: separate;/*collapseから変更*/
        border-spacing: 0;
        border-radius: 5px 5px 0 0;
        /* overflow: hidden; */
    }

    .table thead th,
    .table tbody td {
        border-bottom: 1px solid rgb(192, 192, 192);/*一括指定せず、border-bottomのみ*/
        border-left: 1px solid rgb(192, 192, 192);
    }
    .table tr th:first-child,
    .table tbody tr td:first-child {
        border-left: none;
    }
    td{
        width:auto;
    }
    .input_table input:disabled{
        background-color:white;
    }

`;
// スタイルを追加
document.head.appendChild(styleElement);

// ダイアログをモーダルにするための関数
$(window).on('load',function(){
    $(document).off("click", ".ui-widget-overlay");
})


// ローディングオーバーレイ要素を作成
const loadingOverlay = document.createElement('div');
loadingOverlay.className = 'loading-overlay';
loadingOverlay.innerHTML = '<div class="loading-spinner"></div>';
document.body.appendChild(loadingOverlay);


// フィールド作成の関数
const createField = (dialogId, type, id, label, options = {},multipleFlg = false) => {
    const fullId = `${dialogId}_${id}`;
    if (document.getElementById(fullId)) {
        let message = `[SYS-063] フィールドID "${fullId}" は既に存在します。一意のIDを使用してください。`;
        alert(message)
        throw new Error(message);
    }

    const hiddenAttr = options.hidden ? 'display:none':"";
    const disabledAttr = options.disabled ? 'disabled' : '';
    const requiredAttr = options.required ? 'required' : '';
    const additionalAttrs = Object.entries(options.attributes || {})
        .map(([key, value]) => `${key}="${value}"`)
        .join(' ');
    const widthClass = options.width === 'wide' ? 'field-wide' : 'field-normal';
    const requiredClass = requiredAttr == 'required' ? "SdtRequired" : '';
    const maxLength = options.varidate && 'maxlength' in options.varidate ? options.varidate.maxlength : null;
    const maxlengthForNumberClass = ['number','range-number'].includes(type) && options.varidate && 'maxlength' in options.varidate ? `maxlength_for_number` : "";

    // バリデーション
    let varidateClass = "";
    let numOnlyClass = "";
    if(options.varidate && options.varidate.type == "int"){
        varidateClass = 'varidate_int';
        numOnlyClass = options.searchDialog || ('alignment' in options && options.alignment == 'end') ? "numOnly" : "";
    }
    else if(options.varidate && options.varidate.type == 'zeroPadding'){
        varidateClass = 'varidate_zeroPadding';
        numOnlyClass = options.searchDialog ? "numOnly" : "";
    }
    else if(options.varidate && options.varidate.type == 'str'){
        varidateClass = 'varidate_str';
    }
    else if(options.varidate && options.varidate.type == 'furigana'){
        varidateClass = 'varidate_furigana';
    }
    

    if(['text-set','select','radio'].includes(type) && !('values' in options) && !('searchDialog' in options)  && !(options.disabled)){
        let message = `[SYS-064] ダイアログ名:"${dialogId}"　項目ID:"${id}" の選択肢（values）を指定してください`;
        alert(message)
        throw new Error(message);
    }

    // ロック項目チェック
    if(options.lock){
        let tmp_id = id;
        if(type == 'text-set'){
            tmp_id += "From";
        }
        lockItems.push({dialogId:dialogId,id:tmp_id,name:options.lock});
    }

    let initializeClass = options.value ? 'initialize_target':'';

    let t_head = '';
    let fieldHTML = '';
    switch(type) {
        case 'text':
        case 'number':
        case 'email':

            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}" class="${requiredClass}">${label}</label></p>
                    <div class="field-control" >
                        <div class="container-normal">
                            <div class="oneItem">
                                <input id="${fullId}" name="${fullId}" class="control-textbox ${numOnlyClass} ${initializeClass} ${'alignment' in options && options.alignment == 'end' ? 'input-Number' :''} ${maxlengthForNumberClass} ${varidateClass}" type="${type}"
                                    ${options.placeholder ? `placeholder="${options.placeholder}"` : ''}
                                    ${options.value ? `value="${options.value}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    ${maxLength != null ? `maxlength='${maxLength}'` : ''}
                                    style='${'alignment' in options ? `text-align:${options.alignment}` : ''}'
                                    ${type == 'number' ? `onkeydown="return event.keyCode !== 69"`:""}
                                >
                                ${options.unit ?`<div style="font-size:12px; padding-left: 3px;">${options.unit}</div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-search dialogSearch ${varidateClass}" style="position:static" tabindex="${options.searchDialog.focusTarget === false ? '-1':'0'}"></div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}')"style="position:static"></div>`: ''}
                        </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'over-text':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}" class="${requiredClass}">${label}</label></p>
                    <div class="field-control" >
                        <div class="container-normal">
                            <div class="overText">
                                <input id="${fullId}" name="${fullId}" class="control-textbox ${numOnlyClass} ${initializeClass} ${'alignment' in options && options.alignment == 'end' ? 'input-Number' :''} ${maxlengthForNumberClass} ${varidateClass}" type="${type}"
                                    ${options.placeholder ? `placeholder="${options.placeholder}"` : ''}
                                    ${options.value ? `value="${options.value}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    ${maxLength != null ? `maxlength='${maxLength}'` : ''}
                                    style='${'alignment' in options ? `text-align:${options.alignment}` : ''}'
                                >
                                ${options.unit ?`<div style="font-size:12px; padding-left: 3px;">${options.unit}</div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-search dialogSearch ${varidateClass}" style="position:static" tabindex="${options.searchDialog.focusTarget === false ? '-1':'0'}"></div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}')"style="position:static"></div>`: ''}
                        </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'select':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <select id="${fullId}" name="${fullId}" class="control-dropdown"
                                ${options.onchange ? `onchange="${options.onchange}('${dialogId}', '${id}')"` : ''}
                                ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                                <option value="">&nbsp;</option>
                                ${options.values ? options.values.map(opt => `<option value="${opt.value}" ${opt.selected ? 'selected' : ''}>${opt.text}</option>`).join('') : ''}
                            </select>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'datepicker':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                        <div class="oneItem">
                            <input id="${fullId}" name="${fullId}" class="control-textbox ${varidateClass}" type=${options.format ? options.format : "date"} max="${options.format && options.format == 'month' ? '9999-12':'9999-12-31'}"
                                ${options.placeholder ? `placeholder="${options.placeholder}"` : ''}
                                ${options.value ? `value="${options.value}"` : ''}
                                autocomplete="off"
                                ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                            <div class="ui-icon ui-icon-clock SdtTime" style="position:static"></div>
                        </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'textarea':
            fieldHTML = `
                <div id="${fullId}Field" class="field-wide both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div id="${fullId}_viewer" class="control-markup not-send" ondblclick="editMarkdown('${dialogId}', '${id}');">
                                <pre>${options.value ? options.value : '<br>'}</pre>
                            </div>
                            <div id="${fullId}.editor" class="ui-icon ui-icon-pencil button-edit-markdown" onclick="editMarkdown('${dialogId}', '${id}');">
                            </div>
                            <textarea id="${fullId}" name="${fullId}" class="control-markdown" tabindex="-1"
                                ${options.placeholder ? `placeholder="${options.placeholder}"` : ''}
                                style="height: 100px; display: none;"
                                ${disabledAttr} ${requiredAttr} ${additionalAttrs}>${options.value ? options.value : ''}</textarea>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'checkbox':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <div class="field-control">
                        <div class="container-normal">
                            <input id="${fullId}" name="${fullId}" type="checkbox"
                                ${options.checked ? 'checked' : ''}
                                ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                            <label for="${fullId}" class="${requiredClass}">${label}</label>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'radio':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label ${requiredClass}" >${label}</p>
                    <div class="field-control">
                        <div class="container-normal radioButton">
                            ${options.values ? options.values.map(opt => `
                                <input type="radio" id="${fullId}_${opt.value}" name="${fullId}" value="${opt.value}"
                                    ${opt.checked ? 'checked' : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                                <label for="${fullId}_${opt.value}">${opt.text}</label>
                            `).join('') : ''}
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'range':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}"  class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="range-container">
                                <span class="range-label">${options.min || 0}</span>
                                <input id="${fullId}" name="${fullId}" class="range-input" type="range"
                                    min="${options.min || 0}" max="${options.max || 100}"
                                    value="${options.value || options.min || 0}"
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    oninput="document.getElementById('${fullId}Value').textContent = this.value">
                                <span class="range-label">${options.max || 100}</span>
                            </div>
                            <div id="${fullId}Value" style="text-align: center;">${options.value || options.min || 0}</div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'range-text':
        case 'range-number':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}From" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="range-text-container">
                                <input id="${fullId}From" name="${fullId}From" class="control-textbox range-text-input ${numOnlyClass} ${'alignment' in options && options.alignment == 'end' ? 'input-Number' :''} ${maxlengthForNumberClass} ${varidateClass}" type="${type=='range-text'?'text':'number'}"
                                    ${options.placeholderFrom ? `placeholder="${options.placeholderFrom}"` : ''}
                                    ${options.valueFrom ? `value="${options.valueFrom}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    style='${'alignment' in options ? `text-align:${options.alignment}` : ''}'
                                    ${maxLength != null ? `maxlength='${maxLength}'` : ''}
                                    ${type == 'range-number' ? `onkeydown="return event.keyCode !== 69"`:""}
                                >
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-search dialogSearch ${varidateClass}" style="position:static" tabindex="${options.searchDialog.focusTarget === false ? '-1':'0'}"></div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}From')"  style="position:static"></div>`: ''}
                                <span class="range-text-separator">${options.betweenString ? options.betweenString : '～'}</span>
                                <input id="${fullId}To" name="${fullId}To" class="control-textbox range-text-input ${numOnlyClass} ${'alignment' in options && options.alignment == 'end' ? 'input-Number' :''} ${maxlengthForNumberClass} ${varidateClass}" type="${type=='range-text'?'text':'number'}"
                                    ${options.placeholderTo ? `placeholder="${options.placeholderTo}"` : ''}
                                    ${options.valueTo ? `value="${options.valueTo}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    style='${'alignment' in options ? `text-align:${options.alignment}` : ''}'
                                    ${maxLength != null ? `maxlength='${maxLength}'` : ''}
                                    ${type == 'range-number' ? `onkeydown="return event.keyCode !== 69"`:""}
                                >
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-search dialogSearch ${varidateClass}" style="position:static" tabindex="${options.searchDialog.focusTarget === false ? '-1':'0'}"></div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}To')"  style="position:static"></div>`: ''}
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'range-date':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}From" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="range-text-container">
                                <input id="${fullId}From" name="${fullId}From" class="control-textbox ${varidateClass}" type=${options.format ? options.format : "date"} max="${options.format && options.format == 'month' ? '9999-12':'9999-12-31'}"
                                    ${options.placeholderFrom ? `placeholder="${options.placeholderFrom}"` : ''}
                                    ${options.valueFrom ? `value="${options.valueFrom}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                                <span class="range-text-separator">～</span>
                                <input id="${fullId}To" name="${fullId}To" class="control-textbox ${varidateClass}" type=${options.format ? options.format : "date"} max="${options.format && options.format == 'month' ? '9999-12':'9999-12-31'}"
                                    ${options.placeholderTo ? `placeholder="${options.placeholderTo}"` : ''}
                                    ${options.valueTo ? `value="${options.valueTo}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}>
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'text-set':
            fieldHTML = `
                <div id="${fullId}Field" class="${widthClass} both" style="${hiddenAttr}">
                    <p class="field-label"><label for="${fullId}From" class="${requiredClass}">${label}</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="text-set">
                                <input id="${fullId}From" name="${fullId}From" class="control-textbox inputText ${varidateClass}" type="${'type' in options ? options.type : 'text'}"
                                    ${options.placeholderFrom ? `placeholder="${options.placeholderFrom}"` : ''}
                                    ${options.valueFrom ? `value="${options.valueFrom}"` : ''}
                                    ${disabledAttr} ${requiredAttr} ${additionalAttrs}
                                    ${maxLength != null ? `maxlength='${maxLength}'` : ''}
                                    ${'type' in options &&  options.type == 'number' ? `onkeydown="return event.keyCode !== 69"`:""}
                                >
                                <input id="${fullId}To" name="${fullId}To" value="${options.values ? options.values.map(e => e.value + ":" + e.text).join(' '):''}" class="control-textbox disableText" type="text" disabled>
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-search dialogSearch ${varidateClass}" style="position: static;" tabindex="${options.searchDialog.focusTarget === false ? '-1':'0'}"></div>`: ''}
                                ${options.searchDialog ?`<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}From')"style="position: static;"></div>`: ''}
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="${fullId}-error"></div>
                </div>
            `;
            break;
        case 'button_block':
            fieldHTML = `
                <div style="width:80%; display: flex;justify-content: end; margin-bottom:10px;">
                    <button id=${fullId} class="button button-icon ui-button ui-corner-all ui-widget applied customized" data-icon="ui-icon-disk" type="button" style="z-index: 9999;" onclick="${options.onclick}">
                        <span class="ui-button-icon ui-icon ui-icon-disk " style="position: static;"></span>
                        <span class="ui-button-icon-space"> </span>${label}
                    </button>
                </div>
            `
            break;
        case 'button_inline':
            fieldHTML = `
                <button id=${fullId} class="button button-icon ui-button ui-corner-all ui-widget applied customized" data-icon="ui-icon-disk" type="button" style="z-index: 9999; ${hiddenAttr}" onclick="${'onclick' in options ? options.onclick : ''}">
                    <span class="ui-button-icon ui-icon ui-icon-disk " style="position: static;"></span>
                    <span class="ui-button-icon-space"> </span>${label}
                </button>
            `
            break;
        case 'buttonTextSet':
            fieldHTML = `
                <div class="input-button-container" style="clear: both; padding: 10px; border:1px solid #ccc; border-radius: 5px; width: 95%; margin: 0 auto; margin-bottom: 20px;">
                    <label for="my-input" class="label" style="top:-20px; left:10px; background-color: rgb(254,238,189); padding:0 10px;">${label}</label>
                    <span style="display: none;">
                        <input style="display: inline-block; left: 30px; position: absolute;border: 1px solid rgb(192,192,192); width: 75%; height: 24px; border-radius: 5px;">
                        <button class="button button-icon ui-button ui-corner-all ui-widget applied changePath customized" type="button" style="float: right; top:-4px;" >
                            <span class="ui-button-icon ui-icon ui-icon-disk"></span>
                            <span class="ui-icon-disk"> </span>登録
                        </button>
                    </span>
                    <span>
                        <span style="display: inline-block; left: 30px; position: absolute;"></span>
                        <button class="button button-icon ui-button ui-corner-all ui-widget applied customized" type="button" style="float: right; top:-4px;" onclick="${options.onclick ? options.onclick : ''}">
                            <span class="ui-button-icon ui-icon ui-icon-disk"></span>
                            <span class="ui-icon-disk"> </span>${options.btnLabel}
                        </button>
                    </span>
                </div>
            `
            break;
        case 'ImportFile':
            fieldHTML = `
                <div class="input-button-container" style="clear: both; padding: 10px; border:1px solid #ccc; border-radius: 5px; width: 95%; margin: 0 auto; margin-bottom: 20px;">
                    <label for="my-input" class="label" style="top:-20px; left:10px; background-color: rgb(254,238,189); padding:0 10px;">${label}</label>
                    <input type="file" id="${id}" accept="${options.fileType}">
                </div>
            `
            break;
        case 'borderText':
            fieldHTML = `
            <div class="field-wide both" style="height:55px;padding-top:15px;">
                <div class="input-container" style="clear: both; padding: 10px; border:1px solid #ccc; border-radius: 5px; width: 95%; margin: 0 auto;">
                    <label for="my-input" class="label" style="top:-20px; left:10px; background-color: rgb(254,238,189); padding:0 10px;">${label}</label>
                    <span style="display: inline-block; left: 30px; position: absolute;"></span>
                </div>
            </div>
            `
            break;
        case 'input-table':
            // thead作成
            t_head = '';
            if(options.multiple || multipleFlg)t_head += `<th style="text-align: center;border-collapse: collapse;" class='inversion'><input type="checkbox" class="allCheck"></th>`;
            if(options.lineNumber)t_head += `<th>No</th>`
            t_head += options.t_heads.map(e => `<th style="text-align: center;border-collapse: collapse; ${e.width ?'width:'+ e.width + ";":''}">${e.label}</th>`).join('');
            // tbody作成
            let t_body = '';
            columnCou = options.t_heads.length;
            for(i = 0;i < options.row;i++){
                t_body += `<tr style='background-color:white !important' class="${dialogId}_${i+1}">`;
                t_body += options.multiple || multipleFlg ?  `<td class='inversion'><input type='checkbox'></td>` : ``;
                t_body += options.lineNumber ? `<td style="text-align:center;">${i + 1}</td>` : '';
                options.t_heads.forEach((e,j) =>{
                    let for_numItem = false;
                    if('alignment' in e)for_numItem = true;

                    const maxlength = e.varidate && 'maxLength' in e.varidate ? `maxlength="${e.varidate.maxLength}" `: "";
                    const maxlengthFrom = e.varidateFrom && 'maxLength' in e.varidateFrom ? `maxlength="${e.varidateFrom.maxLength}" `: "";
                    const maxlengthTo = e.varidateTo && 'maxLength' in e.varidateTo ? `maxlength="${e.varidateTo.maxLength}" `: "";

                    let varidateClass = "";
                    if(e.varidate && e.varidate.type == "int"){
                        varidateClass = 'varidate_int';
                    }
                    else if(e.varidate && e.varidate.type == 'zeroPadding'){
                        varidateClass = 'varidate_zeroPadding';
                    }
                    else if(e.varidate && e.varidate.type == 'str'){
                        varidateClass = 'varidate_str';
                    }

                    let varidateFromClass = "";
                    if(e.varidateFrom && e.varidateFrom.type == "int"){
                        varidateFromClass = 'varidate_int';
                    }
                    else if(e.varidateFrom && e.varidateFrom.type == 'zeroPadding'){
                        varidateFromClass = 'varidate_zeroPadding';
                    }
                    else if(e.varidateFrom && e.varidateFrom.type == 'str'){
                        varidateFromClass = 'varidate_str';
                    }

                    let varidateToClass = "";
                    if(e.varidateTo && e.varidateTo.type == "int"){
                        varidateToClass = 'varidate_int';
                    }
                    else if(e.varidateTo && e.varidateTo.type == 'zeroPadding'){
                        varidateToClass = 'varidate_zeroPadding';
                    }
                    else if(e.varidateTo && e.varidateTo.type == 'str'){
                        varidateToClass = 'varidate_str';
                    }


                    const table_requiredAttr_from = e.requiredFrom ? 'required':'';
                    const table_requiredAttr_to = e.requiredTo ? 'required':'';
                    const table_requiredAttr = e.required ? 'required':'';
                    switch (e.type){
                        case 'text-set':
                            const disabled_from   = e.disabled ? 'disabled' : '';
                            const disabled_to     = 'to_disabled' in e && !e.to_disabled ? '':'disabled';

                            t_body += `
                                <td id="${fullId}_${i}_${j}">
                                    <input id="${fullId}_${i}_${j}_From" class="${`${fullId}_${j}`} ${varidateFromClass} ${for_numItem ? 'input-Number':''}" type="text" style='border:none; height:50%;
                                        width:${e.search ? "calc(100% - 39px)":'100%'}; ${for_numItem ? `text-align:${e.alignment}`:''}' ${disabled_from} ${maxlengthFrom} ${table_requiredAttr_from}
                                    >
                                    ${e.search ? `<div class="ui-icon ui-icon-search tableInSearch ${varidateFromClass}" style="position: static;" tabindex="0"></div>`: ''}
                                    ${e.search ? `<div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}_${i}_${j}_From')"style="position: static;"></div>`:''}
                                    <input type="text" id="${fullId}_${i}_${j}_To" class="${for_numItem ? 'input-Number':''} ${varidateToClass} ${`${fullId}_${j}_to`}"
                                        style='border: none; border-top:dashed rgb(192,192,192) 1px; height:50%; width:100%; background-color: white;${for_numItem ? `text-align:${e.alignment}`:''}' ${disabled_to}
                                        ${table_requiredAttr_to}${maxlengthTo}
                                    >
                                </td>
                            `;
                            break;
                        case 'date':
                            t_body += `
                                <td id="${fullId}_${i}_${j}">
                                    <input type="date" style='border:none; height:100%; width:100%' ${e.disabled ? 'disabled':''} max="9999-12-31" class="${varidateFromClass} ${`${fullId}_${j}`} "${table_requiredAttr_from}>
                                    <input type='text' class="${`${fullId}_${j}`} ${`${fullId}_${j}_to`} ${varidateToClass}" style="border:none;border-top:dashed rgb(192,192,192) 1px;width:100%;" ${e.disabled ? 'disabled':''} ${maxlengthTo} ${table_requiredAttr_to}>
                                </td>`;
                            break;
                        case 'dateOnly':
                            t_body += `
                                <td id="${fullId}_${i}_${j}">
                                    <input type="date" style='border:none; height:100%; width:100%'  ${e.disabled ? 'disabled':''} max="9999-12-31" class="${`${fullId}_${j}`} ${varidateClass}" ${table_requiredAttr}>
                                </td>`;
                            break;
                        case 'button':
                            t_body += `
                                <td id="${fullId}_${i}_${j}" class="button-small">
                                    <button
                                        style="width:100%;"
                                        ${e.disabled ? 'disabled':''}
                                    >
                                        ${e.btn_label ? e.btn_label : 'ボタン'}
                                    </button>
                                </td>`;
                            break;
                        default:
                            if(e.search) {
                                t_body += `
                                    <td id="${fullId}_${i}_${j}" style="height:100%">
                                        <input id="${fullId}_${i}_${j}_From" class="${`${fullId}_${j}`} ${varidateClass}" type="text" style='border:none;width:calc(100% - 39px); height:100%; ${for_numItem ? `text-align:${e.alignment}`:''}'  ${e.disabled ? 'disabled':''} ${maxlength} ${table_requiredAttr}>
                                        <div class="ui-icon ui-icon-search tableInSearch ${varidateClass}" style="position: static;" tabindex="0"></div>
                                        <div class="ui-icon ui-icon-close textClear" onclick="textClear('${fullId}_${i}_${j}')"style="position: static;"></div>
                                    </td>`;
                            } else {
                                t_body += `
                                    <td id="${fullId}_${i}_${j}" style="height:100%">
                                        <input type="text"  class="${`${fullId}_${j}`} ${varidateClass} ${for_numItem ? 'input-Number':''}" style='border:none; height:100%; width:100%; background-color:white;${for_numItem ? `text-align:${e.alignment}`:''} ' ${e.disabled ? 'disabled': ''} ${maxlength} ${table_requiredAttr}>
                                    </td>`;
                            }
                            break;
                    }
                })
                t_body += "</tr>";
            }
            fieldHTML = `
                <div id="${fullId}_inputTable" class="input_table ui-dialog-content ui-widget-content" style="display: inline-block; margin-top:20px; width: 98%;margin-left:1%;">
                    <div class="field-vertical " style="width: 100%;">
                        <div class="field-control">
                            <div class="container-selectable">
                                <div class="wrapper h300">
                                    <table class="table inputTable" style="height:100%;">
                                        <thead style="position: sticky; top:0; background-color: gold; z-index:9999">
                                            <tr>
                                                ${t_head}
                                            </tr>
                                        <tbody>
                                            ${t_body}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            break;
        case 'search-table':
            t_head = '';
            if(options.multiple || multipleFlg)t_head += `<th style="text-align: center;border-collapse: collapse;" class='inversion'><input type="checkbox" class="allCheck"></th>`;
            t_head += options.t_heads.map(e => `<th style="text-align: center;border-collapse: collapse; ${e.hidden ? 'display:none;':''}">${e.label}</th>`).join('');
            fieldHTML = `
                <div style="width:80%; display: flex;justify-content: end;">
                    <button id="${fullId}_searchButton" class="button button-icon ui-button ui-corner-all ui-widget applied customized" data-icon="ui-icon-search" type="button" style="z-index: 9999;" onclick='create_searchList($(this));'>
                        <span class="ui-button-icon ui-icon ui-icon-search search" style="position: static;"></span>
                        <span class="ui-button-icon-space"> </span>検索
                    </button>
                </div>
                <div id="${fullId}_readTable" class="ui-dialog-content ui-widget-content" style="display: inline-block; margin-top:20px; width: 98%;margin-left:1%;">
                    <div class="field-vertical " style="width: 100%;">
                        <div class="field-control">
                            <div class="container-selectable">
                                <div class="wrapper h300">
                                    <table class="table searchTable">
                                        <thead style="position: sticky; top:0; background-color: gold; z-index:9999">
                                            <tr>
                                                ${t_head}
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            break;
        case 'free':
            fieldHTML = options.str;
            break;
    }
    return fieldHTML;
};


//時計マークがクリックされたら現在日付を入力
$(document).on('click','.SdtTime',function(){

    //フォーマットを取得
    const format = $(this).prev().prop('type');
    //現在日付を取得
    let dt = new Date();
    //typeがmonthの場合は「yyyy-mm」、dateの場合は「yyyy-mm-dd」形式に文字列成型
    let formatDt = dt.toLocaleDateString('sv-SE');
    if(format == "month"){
        formatDt = formatDt.slice(0,7);
    }
    $(this).prev().val(formatDt);
})

// lookups作成
function createLookups(dialogId, title, fields){

    //キー項目を取得
    let keyObjs = fields.filter(e => e.options && typeof(e.options) == "object" && ( e.options.lookupOrigin || e.options.searchDialog));

    for(let keyObj of keyObjs){

        // id作成
        let key_fullId = dialogId + "_" + keyObj.id;
        // 転記先項目を取得
        let lookpupList = fields.filter(e => e.options.lookupFor  && e.options.lookupFor[0].id == keyObj.id);

        let tmp_lookups = {};

        // キー項目の値を代入
        tmp_lookups.id              = key_fullId;
        tmp_lookups.type            = keyObj.type;
        tmp_lookups.tableId         = keyObj.options.lookupOrigin ? keyObj.options.lookupOrigin.tableId : '';
        tmp_lookups.keyColumnName   = keyObj.options.lookupOrigin ? keyObj.options.lookupOrigin.keyColumnName : '';
        tmp_lookups.digitsNum       = keyObj.options.digitsNum ? keyObj.options.digitsNum:'';
        if('searchDialog' in keyObj.options){
            tmp_lookups.searchDialog    = {id:keyObj.options.searchDialog ? keyObj.options.searchDialog.id:'',
                title:keyObj.options.searchDialog ? keyObj.options.searchDialog.title:'',
                multiple:keyObj.options.searchDialog.multiple ? true : false};
        }
        if(keyObj.options.lookupOrigin && 'base_sql' in keyObj.options.lookupOrigin)tmp_lookups.base_sql = keyObj.options.lookupOrigin.base_sql;
        // 転記先項目のパラメータ設定
        if(keyObj.options.lookupOrigin && keyObj.type == 'text-set'){
            let tmp = [{type:'text',id:key_fullId + 'To',columnName:keyObj.options.lookupOrigin.forColumnName}];
            if(keyObj.options.lookupOrigin.specialTerms)tmp[0].specialTerms = keyObj.options.lookupOrigin.specialTerms;
            tmp_lookups.toParam = tmp;
        }
        else if(keyObj.options.lookupOrigin){
            tmp_lookups.toParam = lookpupList.map(e => {
                let param = {};
                param.type = e.type;
                param.id = e.type == 'text-set' ? dialogId + "_" + e.id + 'From' :dialogId + "_" + e.id;
                param.columnType = e.options.lookupFor[0].columnType;
                param.columnName = e.options.lookupFor[0].columnName;
                if('specialTerms' in e.options.lookupFor[0])param.specialTerms = e.options.lookupFor[0].specialTerms;
                if('func' in e.options.lookupFor[0])param.func = e.options.lookupFor[0].func;
                return param;
            })
        }

        // 前回出力情報
        if("get_lastTime_output" in keyObj.options){
            tmp_lookups.get_lastTime_output = {dialogId:dialogId,name:keyObj.options.get_lastTime_output};
        }

        lookups.push(tmp_lookups);

        // イベント登録
        if(keyObj.options.lookupOrigin){
            if(keyObj.type == 'text-set')key_fullId += 'From';
            $(document).on('blur',`#${key_fullId}`,async function(){
                // if(now_lookup_flg)return;
                // now_lookup_flg = true;
                let parentClass = $(this).parent().attr('class');
                let id = $(this).attr('id');
                if(parentClass == 'text-set')id = id.slice(0,-4);

                if($(this).val() == ""){
                     // クリア処理
                    let lookup = lookups.filter(e => e.id == id)[0];
                    let prm = lookup.toParam ? lookup.toParam : [];

                    prm.forEach(e => {
                        $(`#${e.id}`).val('').trigger('blur');
                    });

                    // 前回出力情報削除
                    if('get_lastTime_output' in lookup){
                        $(`#${lookup.get_lastTime_output.dialogId} .input-container span`).text('');
                    }
                }
                else{
                    dialogLookups($(this),parentClass);
                }
                
                // now_lookup_flg = false;
            })
        }
    }
}

// 全角数字を半角数字に変換する関数
const convertFullWidthToHalfWidth = (str) => {
    return str.replace(/[ー－]/g,'-').replace(/[０-９]/g, function(char) {
        // '０' (全角) の文字コードは 65296、'0' (半角) の文字コードは 48
        // 差分 (65296 - 48) を引いて半角に変換
        return String.fromCharCode(char.charCodeAt(0) - 65248);
    });
}


// ルックアップする
// tableなど特殊なものは各ダイアログファイルに記載
async function dialogLookups(t,parentClass = ''){
    try {
        showLoading();

        t.val(convertFullWidthToHalfWidth(t.val()));

        let item_label = t.closest('.field-control').prev().children('label').text();

        let keyId = t.attr('id');
        if(parentClass == 'text-set') keyId = keyId.slice(0,-4);

        let lookup = lookups.filter(e => e.id == keyId)[0];
        let prm = lookup.toParam;
        let columns = lookup.toParam.map(e => {
            if('specialTerms' in e){
                return e.specialTerms;
            }else{
                return e.columnName;
            }
        });

        // 空欄だったらルックアップのクリア処理
        const item_ids = prm.map(e => e.id);//ルックアップ先の項目ID配列
        //item_idsの中のルックアップ項目を取得
        const lookup_item_arr = lookups.filter(e => {
            const tmp = e.type == 'text-set' ? e.id + "From" : e.id;
            return item_ids.includes(tmp);
        });
        if(t.val() == ""){
            //ルックアップ先項目を削除
            prm.forEach(e =>{
                $(`#${e.id}`).val('').trigger('input');
            })
            // 子ルックアップを削除
            lookup_item_arr.forEach(e =>{
                e.toParam.forEach(j =>{
                    $(`#${j.id}`).val('').trigger('input');
                })
            })
            return;
        }

        // SQLが必要かで処理を分ける
        let tableData;
        if(!/^[0-9]+$/.test(lookup.tableId)){

            // 売上入力の場合は処理を分ける
            if("salesInput_estimateNo" == lookup.id){
                where = `WHERE MH.見積番号 = ${$('#salesInput_estimateNo').val()}`;
                let sql = salesInput_baseSQL(where);
                tableData = await fetchSql(sql);
                salesInput_lookup(tableData);
                return;
            }
            else if('base_sql' in lookup){
                const sql = lookup.base_sql + $('#'+ t.attr('id')).val();
                tableData = await fetchSql(sql);
                tableData = tableData.results;
            }
            else{
                let sql = `SELECT ${columns} FROM ${lookup.tableId} WHERE ${lookup.keyColumnName} = '${$('#'+ t.attr('id')).val()}'`;
                tableData = await fetchSql(sql);
                tableData = tableData.results;
            }
        }
        else{
            await $p.apiGet({
                id: lookup.tableId,
                data: {
                    View: {
                        'ApiDataType': "KeyValues",
                        'ApiColumnKeyDisplayType':'ColumnName',
                        'ApiColumnValueDisplayType':"Value",
                        'GridColumns': columns,
                        "ColumnFilterHash":{
                            [lookup['keyColumnName']]:$(`#${t.attr('id')}`).val()
                        },
                        "ColumnFilterSearchTypes": {
                            [lookup['keyColumnName']]: "ExactMatch"
                        }
                    },
                },
                done: function (data) {
                    tableData = data.Response.Data;
                }
            });
        }
        if(tableData.length != 1) {
            alert(`指定の${item_label}は存在しません`);
            t.val('').trigger('blur');
            t.focus();
            return;
        }

        prm.forEach(e => {
            let tmp = tableData[0][e.columnName];
            let item = "";
            if('func' in e){
                tmp = e.func(tableData[0]);
            }
            if(e.type == "datepicker"){
                $(`#${e.id}`).val(
                    !tmp ? "":
                    new Date(tmp).toLocaleDateString('sv-SE')
                ).trigger('blur');
            }else{
                $(`#${e.id}`).val(tmp).trigger('blur');
            }
        });

        // 前回出力情報
        if('get_lastTime_output' in lookup){
            get_history(lookup.get_lastTime_output.name,$(`#${t.attr('id')}`).val(),lookup.get_lastTime_output.dialogId)
        }

    } catch(error) {
        message = '[GEN-036] ルックアップエラー：' + error
        console.error(message);
        alert(message);
    } finally {
        hideLoading();
    }
}

$(document).on('blur','textarea',function(){
    $(this).hide();
    $(this).prev().prev().text($(this).val());
    $(this).prev().prev().show();
})

$(document).on('mousedown','button:has(.ui-icon-closethick)',function(){
    const element = $(this).closest('div').next();
    const id = element.attr('id');
    if(element.hasClass('searchDialog')){
        closeDialog(id,false);
    }else{
        closeDialog(id,true);
    }
})
$(document).on('keydown','button:has(.ui-icon-closethick)',function(event){
    if (event.key === 'Enter') {
        $(this).trigger('mousedown');
    }
})

// ダイアログを閉じる関数
function closeDialog(dialogId,deleteFlg) {

    const lock_filter = lockItems.filter(e => e.dialogId == dialogId);
    if(lock_filter.length == 1){
        const tmp = $(`#${dialogId}_${lock_filter[0]["id"]}`).val();
        if(tmp != ""){
            UnLockData(lock_filter[0]["name"],tmp);
        }
    }

    $(`#${dialogId}`).dialog('close');
    searchOutputId.pop();

    $(`#${dialogId}`).dialog("destroy").remove();

    if(!deleteFlg)dialogId = dialogId.slice(0,-6);
    $('#Application').append(htmls[dialogId]);
    if(!deleteFlg)dialogId += 'Dialog'
    $(`#${dialogId}`).dialog({
        autoOpen: false
    })

}



// 全件チェック
$(document).on('change','.allCheck',function(){
    const allCheckFlg = $(this).prop('checked');

    // let targetElements = $(this).closest('table').find('tbody tr').find('input[type="checkbox"]');
    let targetElements = $(this).closest('table').find('tbody tr').find('td:first-child input[type="checkbox"]');

    targetElements.prop('checked',allCheckFlg);

})

// inputtableの複数選択画面で、tdデフラグを反転
$(document).on('click','.inversion',function(){
    let targetElement = $(this).find('input[type="checkbox"]');
    targetElement.prop('checked',!targetElement.prop('checked')).trigger('change');
})
$(document).on('click','.inversion > input',function(event){
    event.stopPropagation();
})

// 検索ダイアログの横にある×ボタンが押されたときに実行
// keyになるinput要素を削除
// blurイベントを着火させてlookup対象項目も削除
function textClear(id){
    const dialogId = id.split('_')[0];
    const lock_filter = lockItems.filter(e => e.dialogId == dialogId);
    if(lock_filter.length == 1){
        const tmp = $(`#${dialogId}_${lock_filter[0]["id"]}`).val();
        if(tmp != ""){
            UnLockData(lock_filter[0]["name"],tmp);
        }
    }
    $(`#${id}`).val('').trigger('blur');
}


$(document).on('keydown','.ui-icon-search',function(e){
    if(e.which == 13){
        $(this).trigger('click');
    }
})
$(document).on('keydown','.ui-icon-close',function(e){
    if(e.which == 13){
        $(this).trigger('click');
    }
})


$(document).on('keydown', 'tr', function(e) {
    let tbodyElement = $(this);
    let idx = $(this).index();

    // フォーカス移動
    if (e.which == 38 && idx != 0) {
        let prevElement = tbodyElement.prev();
        // ensureElementVisible(prevElement);
        prevElement.prev().focus();
        prevElement.focus();
        e.preventDefault();
    } else if (e.which == 40 && idx != (tbodyElement.siblings().length)) {
        let nextElement = tbodyElement.next();
        // ensureElementVisible(nextElement);
        nextElement.next().focus();
        nextElement.focus();
        e.preventDefault();
    }
});

// 数値入力用項目がフォーカスから外れたら3桁区切りにする
$(document).on('blur','.input-Number',async function(){
    await sleep(1);
    let val = Number(convertFullWidthToHalfWidth($(this).val().replaceAll(',','').replace(/[ー－]/g,'-').replace(/[^-\d０-９,]/g, '')));
    if(val == 0){
        return;
    }
    if(/^-?[0-9]+$/.test(val)){
        $(this).val(val.toLocaleString());
    }
})

// 3桁区切り解除
$(document).on('focus','.input-Number',async function(){
    let val = Number($(this).val().replaceAll(',',''));
    if(val == 0){
        $(this).val('');
    }
    else if(/^-?[0-9]+$/.test(val)){
        $(this).val(val);
    }
})

$(document).on('input','.varidate_int',function(){
    input_numberOnly($(this))
})
$(document).on('input','.varidate_zeroPadding',function(){
    input_numberOnly($(this))
})

// マイナス符号、数字（半角・全角）のみ許可する
function input_numberOnly(e){
    if(now_IME)return;
    const target_element = varidate_input_select(e);
    const target_value = target_element.val();

    after_value = target_value.replace(/[^-ー－\d０-９]/g, "");
    target_element.val(after_value);
}

// falseを返すとバリデーション処理終了
function location_relation(e,t){
    const current_element = t
    const nextFocus_element = e.relatedTarget;
    const dialogId = current_element.closest('.dialog,.searchDialog').attr('ID');
    const inputs = $(`#${dialogId} input, #${dialogId} .ui-icon-search,#${dialogId} button`);
    const currentIndex = inputs.index(current_element); // 現在の input の位置
    const nextIndex = inputs.index(nextFocus_element); // 次にフォーカスされる要素の位置
    const siblings =  t.siblings();

    // 選択要素が終了ボタン
    if($(nextFocus_element).text() == '\n                \n                 終了\n            ' || nextFocus_element == null){
        return false;
    }
    else if(t.parent().hasClass('range-text-container')){
        if(t.prop('tagName') == 'INPUT'){
            return false;
        }else if(nextIndex > currentIndex){
            return true;
        }else{
            return false;
        }

    }
    else if(siblings.is(nextFocus_element)){
        return false;
    }
    else if (nextIndex > currentIndex) {
        return true;
    } else{
        return false;
    }
}

// バリデーション
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
now_varidate_flg = false;

// 固定長数字で0埋め（得意先CDなど）
$(document).on('blur','.varidate_zeroPadding',function(event){
    varidate_zeroPadding(event,$(this));
})
async function varidate_zeroPadding(event,e){
    if(now_varidate_flg)return;
    now_varidate_flg = true;
    const element = varidate_input_select(e);
    const dialog_element = element.closest('.dialog');

    await sleep(0);
    if(!location_relation(event,e)){
        now_varidate_flg = false;
        return;
    }

    const maxLength = element.prop('maxlength');
    const required_flg = element.prop('required');

    if(!required_flg && element.val() == ''){
        now_varidate_flg = false;
        return;
    }
    // 0-9 or ０-９の値以外が入力されていたらクリア処理とアラート
    if((required_flg && element.val() == '') || /[^０-９\d]/.test(element.val())){
        element.focus();
        element.val('');
        alert(`${maxLength}桁の数字を入力してください`);

        // alertで新しくイベント着火しちゃうから、新しく着火したイベントを先に進めて終了させる（now_varidate_flg）
        await sleep(0);
        now_varidate_flg = false;
        return;
    }

    // 全角数字を半角数字に変換
    let val = convertFullWidthToHalfWidth(element.val());
    val = ('0000000' + val).slice(-(maxLength));
    element.val(val);

    now_varidate_flg = false;
}

$(document).on('blur','.varidate_int',function(event){
    varidate_number(event,$(this));
})
// 半角数字のバリデーション（合計金額など）
async function varidate_number(event,e){

    if(now_varidate_flg)return;
    now_varidate_flg = true;
    const element = varidate_input_select(e);
    const dialog_element = element.closest('.dialog');

    await sleep(0);
    if(!location_relation(event,e)){
        now_varidate_flg = false;
        return;
    }

    const required_flg = element.prop('required');

    if(!/(^-?ー?－?[０-９\d]+$|^$)/.test(element.val()) || (required_flg && element.val() == "")){
    // if(!/^[\d\uFF10-\uFF19]+(,\d{3})*$/.test(element.val())){
        element.focus();
        element.val('');
        alert("未入力項目があります。");

        // alertで新しくイベント着火しちゃうから、新しく着火したイベントを先に進めて終了させる（now_varidate_flg）
        await sleep(0);
        now_varidate_flg = false;
        return;
    }

    // 全角数字を半角数字に変換
    let val = convertFullWidthToHalfWidth(element.val());
    element.val(val);
    now_varidate_flg = false;

}

// 必須項目チェック
$(document).on('blur','.varidate_str',function(event){
    varidate_fixedLength_str(event,$(this));
})
async function varidate_fixedLength_str(event,e){

    if(now_varidate_flg)return;
    now_varidate_flg = true;

    const element = varidate_input_select(e);

    await sleep(0);
    if(!location_relation(event,e)){
        now_varidate_flg = false;
        return;
    }
    const required_flg = element.prop('required');

    // 必須チェック
    if(element.val() == '' && required_flg){
        alert('必須項目です。');
        element.val('');
        element.focus();
        await sleep(0);
        now_varidate_flg = false;
        return;
    }
    now_varidate_flg = false;
}

// text-setのバリデーション
$(document).on('input','.text-set input',async function(){
    try{
        if(now_IME)return;

        // lookup項目は除外
        if($(this).attr('id').slice(-2) == 'To')return;

        let id = $(this).attr('id').slice(0,-4);
        let tmp = lookups.filter(e => e.id == id);

        if(tmp.length != 0)return;

        let choices = $(this).next().val().split(' ').map(e => e.split(':')[0]);
        choices.push("");

        if($(this).val().length > 1){
            $(this).val($(this).val().slice(0,1));
            return;
        }

        if(!choices.includes($(this).val())){
            $(this).val('').focus();
        }
    }
    catch(err){
        return;
    }
})

// フリガナのバリデーション
$(document).on('input','.varidate_furigana',function(){
    try{
        if(now_IME)return;
        
        const target_element = varidate_input_select($(this));
        const after_value = target_element.val().replace(/[^\u30A0-\u30FF\uFF65-\uFF9F]/g, '');
        $(this).val(after_value);

    }catch(e){
        console.error(e);
        return;
    }
})

// input type=number はmaxlengthが働かないので指定桁以上を入力できないようにする
$(document).on('input','.maxlength_for_number',function(){
    let maxlength = $(this).attr('maxlength');
    if(maxlength == ""){
        return;
    }
    else{
        maxlength = Number(maxlength);
    }
    let val = $(this).val();
    if(val.length > maxlength){
        $(this).val(val.slice(0,maxlength));
    }

})

// divかinputかわからないため、input要素を返す
function varidate_input_select(e){
    if(e.parent().hasClass('text-set') && e.prop('tagName') == 'DIV'){
        return e.prevAll().eq(1);
    }
    else if(e.prop('tagName') == 'DIV'){
        return e.prev();
    }else{
        return e;
    }
}


// ブラウザのアクティブ状態を監視
let isWindowActive = true;
window.addEventListener('blur', () => {
    isWindowActive = false;
});

window.addEventListener('focus', () => {
    isWindowActive = true;
});

// 指定したミリ秒待機する
async function sleep(s){
    await new Promise((resolve,reject) => {
        setTimeout(() => {
            resolve();
        }, s);
    })
}

// ローディング制御関数
function showLoading() {
    //     document.querySelector('.loading-overlay').style.display = 'flex';
    const element = document.querySelector('.loading-overlay');
    if(element == null)return;
    element.style.display = 'flex';
}

function hideLoading() {
    //     document.querySelector('.loading-overlay').style.display = 'none';
    const element = document.querySelector('.loading-overlay');
    if(element == null)return;
    element.style.display = 'none';
}

// 帳票をダウンロードする処理　→　（TODO 帳票取得以外にも　使ってるので　名称変更し 共通.jsへ移動予定　（download_report → fetch_service））
async function download_report(param, fileName = "", accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"){
    try {
        showLoading();
        let res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
                "Accept": accept,
            },
            body: JSON.stringify(param)
        });

        if (!res.ok) {
            const errorData = await res.json();
            console.error('[API-065]', errorData);
            throw new Error('[API-065] ' + errorData.message);
        }

        // 成功メッセージをヘッダーから取得して表示
        const messageCode = res.headers.get('Success-Message-Code');
        if (messageCode) {
            if (SUCCESS_MESSAGE_CODE[messageCode]) {
                alert(`**${SUCCESS_MESSAGE_CODE[messageCode]}**`);
            } else {
                alert(`登録されていないメッセージコードです: ${messageCode}`);
            }
        }


        // ファイルのダウンロードをするかどうか
        const skip = res.headers.get('Download-Skip');
        if (skip) {
            console.log("ファイルのダウンロードをスキップします")
        } else {
            // ダウンロードファイル名をクライアント側で上書きする場合
            let downloadFileName = ""
            if (fileName) {
                downloadFileName = fileName;
            // ダウンロードファイル名をサーバー側で指定する場合
            } else {
                let tmpText = res.headers.get('Content-Disposition').split('filename=')[1].replace(/"/g, '')
                downloadFileName = decodeString(tmpText)
            }

            const blob = await res.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = downloadFileName;
            document.body.appendChild(a);
            a.click();

            // リンクを削除
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
            console.log("ファイルをダウンロードしました")

        }

    } catch(error) {
        message = '[GEN-066] ファイルダウンロード中にエラーが発生しました'
        console.error(message, error.message);
        alert(message + error.message);
        throw error;
    } finally {
        hideLoading();
    }
}


/**
 * 指定された日付を特定のフォーマットで返す関数
 * @param {number} day          日付（1-31の数値、または99で月末日）
 * @param {string} type         出力形式（'month'で年月のみ、その他で年月日）
 * @returns {string}            YYYY-MM-DD または YYYY-MM 形式の日付文字列
 * @example
 * // 月の15日の完全な日付を取得
 * monthDateGein(15)    // => '2024-11-15'
 * // 月末日を取得
 * monthDateGein(99)    // => '2024-11-30'
 * // 年月のみを取得
 * monthDateGein(15, 'month')   // => '2024-11'
 */
function monthDateGein(day,type){
    let dt = new Date();
    let resDate;
    if(day == 99){
        resDate = new Date(dt.getFullYear(),dt.getMonth()+1,0).toLocaleDateString('sv-SE');
    }
    else{
        resDate = new Date(dt.getFullYear(),dt.getMonth(),day).toLocaleDateString('sv-SE');
    }
    resDate = type == 'month' ? resDate.slice(0,7) : resDate;
    console.log(resDate);
    return resDate;
}

/**
 * 文字列を変換する関数
 * @param {string} str          チェックしたい文字列
 * @param {string} replaceStr   変換したい文字列　規定値はnull
 */
function SpcToNull(str,replaceStr = null){
    if(str == ''){
        return replaceStr;
    }else{
        return str.trim();
    }
}

/**
 * 文字列を変換する関数
 * @param {string} encodedText   変換したい文字列
 */
function decodeString(encodedText){
    // Base64文字列をデコードしてUint8Arrayを取得
    const binaryData = Uint8Array.from(atob(encodedText), c => c.charCodeAt(0));

    // Uint8ArrayをUTF-8文字列にデコード
    const decoder = new TextDecoder('utf-8');
    return decoder.decode(binaryData);

}

// 日付をYYYY-MM-DD形式の文字列に変換するための関数
function formatDate(date) {
    date = new Date(date);
    if(!date instanceof Date)return null;
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0ベースのため+1
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

// 日付をYYYY年MM月DD日に変換する関数
function formatDate_for_japanese(date){
    date = new Date(date);
    if(isNaN(date)){
        throw new Error;
    }
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}年${month}月${day}日`;
}

// 時間をhh時mm分形式で返す
function formatTime_for_japanese(date){
    date = new Date(date);
    if(!date instanceof Date){
        return '';
    }
    return date.getHours() + "時" + date.getMinutes() + "分";
}

// dateオーバーフローさせない
function not_date_overflow(old_date,new_date){
    if(isNaN(new_date.getTime())){
        return new_date;
    }
    else if(old_date.getMonth() == new_date.getMonth()){
        return new_date;
    }
    else{
        return new Date(new_date.setMonth(new_date.getMonth(),0));
    }
}

function getFirstDateOfScope(wdate, wshime, defaultVal = "") {

    wshime = Number(wshime);

    // wdateが日付型かチェックし、日付型でなければ変換
    const date = (wdate instanceof Date) ? wdate : new Date(wdate);

    // 変換が失敗した場合、defaultValを返す
    if (isNaN(date.getTime())) return defaultVal;

    switch (wshime) {
        case 0:
            return formatDate(date);  // そのままの日付を返す
        case 99:
            return formatDate(new Date(date.getFullYear(), date.getMonth(), 1));  // 月初日を返す
        default:
            // 日付が締め日を超える場合とそれ以下の場合で分岐
            let bufDate = new Date(`${date.getFullYear()}-${date.getMonth()+1}-${wshime}`);
            if(isNaN(bufDate.getTime()) || new Date(date.getFullYear(),date.getMonth(),wshime).getMonth() != date.getMonth()){
                return defaultVal;
            }
            if (date.getDate() > wshime) {
                let after_date = new Date(bufDate.getFullYear(), bufDate.getMonth(), wshime + 1);
                return formatDate(after_date);
            }
            else {
                let tmp_date1 = new Date(bufDate);
                let tmp_date2 = new Date(bufDate);

                // 締日が月末だったら月初を返す
                if(bufDate.getMonth() != new Date(tmp_date1.setDate(tmp_date1.getDate()+1)).getMonth()){
                    return formatDate(new Date(bufDate.setDate(1)));
                }
                // 請求日付と締日から1か月前、1日を足した値が月をまたいでいないか
                else if(bufDate.getMonth() == new Date(tmp_date2.getFullYear(),tmp_date2.getMonth()-1,tmp_date2.getDate()+1).getMonth()){
                    return formatDate(new Date(bufDate.getFullYear(),bufDate.getMonth(),0));
                }
                else{
                    return formatDate(new Date(bufDate.getFullYear(),bufDate.getMonth()-1,bufDate.getDate()+1));
                }
            }
    }
}

function getLastDateOfScope(wdate, wshime, Default = '') {

    wshime = Number(wshime);
    let bufDate;

    // wdateをDateオブジェクトとして変換
    const date = new Date(wdate);
    if (isNaN(date)) {
        return Default;
    }

    switch (wshime) {
        case 0:
            // Case 0: そのまま日付を返す
            return formatDate(date); // "YYYY-MM-DD" 形式

        case 99:
            // Case 99: 次月の初日から1日減らして末日を取得
            const nextMonth = new Date(date.getFullYear(), date.getMonth() + 1, 1);
            nextMonth.setDate(nextMonth.getDate() - 1);
            return formatDate(nextMonth);
        default:
            // それ以外のケース
            if(new Date(date.getFullYear(),date.getMonth(),wshime).getMonth() != date.getMonth()){
                return Default;
            }
            if (date.getDate() > wshime) {
                // 指定日の翌月にシメ日を設定
                let tmp = new Date(`${date.getFullYear()}-${date.getMonth()+2}-1`);
                bufDate = new Date(`${date.getFullYear()}-${date.getMonth()+2}-${wshime}`);
                if (isNaN(bufDate.getTime())) {
                    return Default;
                }
                else if(tmp.getMonth() != bufDate.getMonth()){
                    return formatDate(new Date(bufDate.getFullYear(),bufDate.getMonth(),0));
                }
                else {
                    return formatDate(bufDate);
                }
            } else {
                // 指定日の月にシメ日を設定
                let tmp = `${date.getFullYear()}-${date.getMonth()+1}-${wshime}`;
                bufDate = new Date(tmp);
                if (!isNaN(bufDate.getTime())) {
                    return formatDate(bufDate);
                } else {
                    return Default;
                }
            }
    }
}


// 日付の月間を計算する関数
function DateDiff(date1, date2) {
    const year1 = date1.getFullYear();
    const month1 = date1.getMonth();  // 0-11の範囲
    const year2 = date2.getFullYear();
    const month2 = date2.getMonth();

    // 年の差を12倍し、月の差を足し合わせる
    const monthDifference = (year2 - year1) * 12 + (month2 - month1);
    return monthDifference;
}

let now_IME = false;
// IME入力中か判断する関数（ルックアップで使用）
$(document).on('compositionstart',function(){
    now_IME = true;
})
$(document).on('compositionend',function(){
    now_IME = false;
    $(':focus').trigger('input');
})


class cMitumoriH{
    constructor(estimateNo){
        this.estimateNo = estimateNo;
        this.m_isDsp_estimate_category = 0; //見積区分
        this.m_isDsp_inCompony_communication = 0; //社内伝
    }
    async isU_category(u_category){
        let res_flg = false;
        const sql = `
            SELECT COUNT(U区分) AS 区分カウント
            FROM TD見積シートM
            WHERE 見積番号 = ${this.estimateNo}
            AND U区分 = '${u_category}'
            AND 見積数量 <> 0
        `;
        const res = await fetchSql(sql);
        if(res.results.length == 0){
            return false;
        }
        else{
            if(res.results.区分カウント == 0){
                return false;
            }
            else{
                return true;
            }
        }
    }
    async GetById(){
        const sql = `select * from TD見積 where 見積番号 = ${this.estimateNo}`;
        const res = await fetchSql(sql);
        if(res.results.length == 0){
            return false;
        }
        res = res.results[0];
        switch(this.m_isDsp_estimate_category){
            case 0:
                if(res.見積区分 == 1){
                    alert('指定の見積番号は本見積ではありません。');
                    return false;
                }
                break;
            case 1:
                if(res.見積区分 == 0){
                    alert('指定の見積番号は仮見積もりではありません。');
                    return false;
                }
        }
        switch(this.m_isDsp_inCompony_communication){
            case 1:
                if(res.得意先CD == "9999"){
                    alert('社内在庫分です。');
                    return false;
                }
            case 2:
                if(res.得意先CD == "9999"){
                    alert('社内在庫意外です。');
                    return false;
                }
        }

        return true;
    }
}
/*
`m_isDsp_estimate_category
仮見積 = 0
本見積 = 1
全て = 2
`
`m_isDsp_inCompony_communication
全て = 0
社内伝以外 = 1
社内伝のみ = 2
`*/


class clsDates{
    constructor(dateID){
        this.dateID = dateID;
    }

    async GetbyID(){
        let dates = await fetchSql(`SELECT * FROM TMDates WHERE DateID = '${this.dateID}'`);
        if(dates.results.length != 0){
            this.updateDate = new Date(dates.results[0].更新日付);
        }
    }
}

function null_to_zero(value,defaultvalue = 0){
    if(value == null || value == ""){
        return defaultvalue;
    }else{
        return value;
    }
}


async function LockData(name,no,check=0,code=''){
    const param = {
        iDataName:name,
        iNumber:no,
        iDataCode:code,
        iCheck:check,
        iPCName:$p.loginId()
    }
    let res = await fetch(LOCK_URL,{
        method:'POST',
        headers:{
            "content-type": "application/json",
        },
        body:JSON.stringify(param)
    })
    if(!res.ok){
        throw new Error;
    }
    res = await res.json();
    const st = res["output_values"]["@RetST"];
    const msg = res["output_values"]["@RetMsg"];
    if(st != 0){
        alert(`${st}:${msg}`);
        return false;
    }

    return true;
}

async function UnLockData(name,no,code=""){
    if(no == "")return;
    const param = {
        iDataName:name,
        iNumber:no,
        iDataCode:code,
        iPCName:$p.userName()
    }
    let res = await fetch(UNLOCK_URL,{
        method:'POST',
        headers:{
            "content-type": "application/json",
        },
        body:JSON.stringify(param)
    })
    if(!res.ok){
        throw new Error;
    }
    res = await res.json();
    const st = res["output_values"]["@RetST"];
    const msg = res["output_values"]["@RetMsg"];
    if(st != 0){
        alert(`${st}:${msg}`);
        return false;
    }

    return true;
}

async function GetTax(date){
    const param = {
        iDate:date
    }
    let res = await fetch(GET_TAX_URL,{
        method:'POST',
        headers:{
            "content-type": "application/json",
        },
        body:JSON.stringify(param)
    })

    if (!res.ok) {
        // レスポンスがエラーの場合
        throw new Error(`HTTP error! status: ${res.status}`);
    }

    const data = await res.json();
    return data;
}

class ClsMitumoriH{
    constructor(estimate_no,mitumori_category = 2,syanaiden_category = 0){
        this.estimate_category = mitumori_category;
        this.syanaiden = syanaiden_category;
        this.estimate_no = estimate_no;
    }
    async IsUCategory(category){
        const query = `
            SELECT COUNT(U区分) AS 区分カウント
            FROM TD見積シートM
            WHERE 見積番号 = ${this.estimate_no}
            AND U区分 = '${category}'
            AND 見積数量 <> 0
        `.replaceAll('\n','');
        const res = await fetchSql(query);

        if(res.count == 0){
            return false;
        }
        if(res.results[0].区分カウント == 0){
            return false;
        }
        return true;
    }

    async GetbyID(){
        try{
            const query = `SELECT * FROM TD見積 WHERE 見積番号 = ${this.estimate_no}`.replaceAll('\n',' ');
            const res = await fetchSql(query);

            const mitsumorikubun_dic = {
                "karimitsumori":0,
                "honmitsumori":1,
                "subete":2
            }

            const syanaiden_dic = {
                "subete":0,
                "syanaiden_igai":1,
                "syanaiden_nomi":2
            }
            if(this.estimate_category == mitsumorikubun_dic["karimitsumori"] &&
                res['results'][0]['見積区分'] == mitsumorikubun_dic['honmitsumori']
            ){
                alert('指定の見積番号は本見積ではありません。');
                return false;
            }

            if(this.estimate_category == mitsumorikubun_dic['honmitsumori'] &&
                res['results'][0]['見積区分'] == mitsumorikubun_dic['karimitsumori']
            ){
                alert('指定の見積番号は仮見積ではありません。');
                return false;
            }

            if(this.syanaiden == syanaiden_dic['syanaiden_igai'] &&
                res['results'][0]['得意先CD'] == 9999
            ){
                alert('社内在庫分です。');
                return false;
            }

            if(this.syanaiden == syanaiden_dic['syanaiden_nomi'] &&
                res['results'][0]['得意先CD'] != 9999
            ){
                alert('社内在庫意外です。');
                return false;
            }

            const result = res['results'][0];

            this.見積番号 = this.estimate_no
            this.担当者CD = result["担当者CD"]
            this.見積日付 = result["見積日付"]
            this.見積件名 = result["見積件名"]
            this.得意先CD = null_to_zero(result["得意先CD"], "")
            this.得意先名1 = null_to_zero(result["得意先名1"], "")
            this.得意先名2 = null_to_zero(result["得意先名2"], "")
            this.得TEL = null_to_zero(result["得TEL"], "")
            this.得FAX = null_to_zero(result["得FAX"], "")
            this.得担当者 = null_to_zero(result["得意先担当者"], "")
            this.納得意先CD = null_to_zero(result["納入得意先CD"], "")
            this.納入先CD = null_to_zero(result["納入先CD"], "")
            this.納入先名1 = null_to_zero(result["納入先名1"], "")
            this.納入先名2 = null_to_zero(result["納入先名2"], "")
            this.納郵便番号 = null_to_zero(result["郵便番号"], "")
            this.納住所1 = null_to_zero(result["住所1"], "")
            this.納住所2 = null_to_zero(result["住所2"], "")
            this.納TEL = null_to_zero(result["納TEL"], "")
            this.納FAX = null_to_zero(result["納FAX"], "")
            this.納担当者 = null_to_zero(result["納入先担当者"], "")
            this.納期S = result["納期S"]
            this.納期E = result["納期E"]
            this.備考 = null_to_zero(result["備考"], "")
            this.規模金額 = null_to_zero(result["物件規模金額"], "")
            this.OPEN日 = result["オープン日"]
            this.物件種別 = null_to_zero(result["物件種別"], "")
            this.現場名 = null_to_zero(result["現場名"], "")
            this.支払条件 = null_to_zero(result["支払条件"], "")
            this.支払条件他 = null_to_zero(result["支払条件その他"], "")
            this.納期表示 = null_to_zero(result["納期表示"], "")
            this.納期表示他 = null_to_zero(result["納期その他"], "")
            this.見積日出力 = null_to_zero(result["見積日出力"], "")
            this.有効期限 = null_to_zero(result["有効期限"], "")
            this.受注区分 = null_to_zero(result["受注区分"], "")
            this.受注日付 = result["受注日付"]
            this.大小口区分 = null_to_zero(result["大小口区分"], "")
            this.出精値引 = null_to_zero(result["出精値引"], "")

            this.受注日付 = result["税集計区分"]
            this.売上端数 = result["売上端数"]
            this.消費税端数 = result["消費税端数"]

            this.合計金額 = result["合計金額"]
            this.原価合計 = result["原価合計"]
            this.原価率 = result["原価率"]
            this.外税額 = result["外税額"]

            this.得意先別見積番号 = null_to_zero(result["得意先別見積番号"], "")

            this.見積区分 = null_to_zero(result["見積区分"], "")


            return true;
        }catch(e){
            alert('予期せぬエラーが発生しました。');
            console.error(e);
            return;
        }
    }
}

function ISHasuu_rtn(kubun, num, keta) {
    const factor = Math.pow(10, keta);
    
    switch (kubun) {
        case 0: // 四捨五入
        case '0':
            return Math.round(num * factor) / factor;
        case 1: // 切り上げ
        case '1':
            return Math.ceil(num * factor) / factor;
        case 2: // 切り捨て
        case '2':
            return Math.floor(num * factor) / factor;
        default:
            throw new Error("無効な区分");
    }
}

// input要素にfocusが当たったら全選択状態にする
$(document).on('focus','input[type="text"],input[type="number"]',function(){
    $(this).select();
})


//得意先情報のクラス
class ClsCustmer{
    constructor(custmerCD){
        this.custmerCD = custmerCD;
    }

    async GetByData(){
        try{
            const query = `select * from TM得意先 where 得意先CD = '${this.custmerCD}'`;
            const res = await fetchSql(query);
            

            if(res["results"].length != 1){
                // alert('指定した得意先は存在しません。');
                return false;
            }
            
            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
            console.log(res);
        }catch(e){
            console.error('ClsCustmer',e)
            throw e;
        }
    }
}



// 納入先情報のクラス
class ClsRecipient{
    constructor(custmerCD,recipientCD){
        this.custmerCD = custmerCD;
        this.recipientCD = recipientCD;
    }

    async GetByData(){
        try{
            const query = `
                select * 
                from TM納入先 
                where 得意先CD = '${this.custmerCD}' and
                    納入先CD = '${this.recipientCD}'
            `;
            const res = await fetchSql(query);
            

            if(res["results"].length != 1){
                return false;
            }
            
            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
        }catch(e){
            console.error('ClsRecipient',e)
            throw e;
        }
    }
}


// 科目マスタのクラス
class ClsSubject{
    constructor(subjectCD){
        this.subjectCD = subjectCD;
    }

    async GetByData(){
        try{
            const query = `
                select * 
                from TM科目 
                where 科目CD = ${this.subjectCD}
            `;
            const res = await fetchSql(query);
            

            if(res["results"].length != 1){
                return false;
            }
            
            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
        }catch(e){
            console.error('ClsSubject',e)
            throw e;
        }
    }
}

// 補助科目マスタのクラス
class ClsSupportSubject{
    constructor(subjectCD,supportSubjectCD){
        this.subjectCD = subjectCD;
        this.supportSubjectCD = supportSubjectCD;
    }

    async GetByData(){
        try{
            const query = `
                select * 
                from TM補助科目 
                where 科目CD = ${this.subjectCD} and
                    補助CD = ${this.supportSubjectCD}
            `;
            const res = await fetchSql(query);

            if(res["results"].length != 1){
                return false;
            }

            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
        }catch(e){
            console.error('ClsSupportSubject',e)
            throw e;
        }
    }
}


// 担当者のクラス
class ClsPerson{
    constructor(personCD){
        this.personCD = personCD;
    }

    async GetByData(){
        try{
            const query = `
                select * 
                from TM担当者
                where 担当者CD = ${this.personCD}
            `;
            const res = await fetchSql(query);

            if(res["results"].length != 1){
                return false;
            }

            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
        }catch(e){
            console.error('ClsPerson',e)
            throw e;
        }
    }
}

class ClsProduct{
    constructor(productNo,specNo){
        this.productNo = productNo;
        this.specNo = specNo;
    }

    async GetByData(strictFlg = false){
        try{
            let query = `
                select * 
                from TM製品
                where 製品NO = '${this.productNo}' 
            `;

            if(strictFlg || this.specNo != ""){
                query += `and 仕様NO = '${this.specNo}'`;
            }

            const res = await fetchSql(query);

            if(res["results"].length == 0){
                return false;
            }

            const result = res["results"][0]
            Object.keys(result).forEach(e =>{
                this[e] = result[e];
            })
            return true;
        }catch(e){
            console.error('ClsPerson',e)
            throw e;
        }
    }
}