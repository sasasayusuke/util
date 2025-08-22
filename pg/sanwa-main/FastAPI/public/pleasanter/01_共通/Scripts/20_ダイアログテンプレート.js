// ダイアログを作成し、ページに追加する関数
htmls = {};
function createAndAddDialog(dialogId, title, fields, btnFields = [], submitHandler,lastSql = "") {
    if (document.getElementById(dialogId)) {
        let message = `ダイアログID "${dialogId}" は既に存在します。一意のIDを使用してください。`;
        alert(message)
        throw new Error(message);
    }

    // 何行になるかを計算し、配列に格納
    let columnCountArr = [];
    let rowCount = 0;
    let newLineFlg = false;
    for(let field of fields){
        if(typeof(field.options) == 'object' && 'newLine' in field.options && field.options.newLine){
            newLineFlg = true;
            rowCount++;
        }
        else if(typeof(field.options) == 'object' && "newLine" in field.options && !field.options.newLine){
            if(newLineFlg){
                columnCountArr.push(rowCount);
                rowCount = 0;
            }
            else{
                newLineFlg = true;
            }
        }
    }

    let content = '';
    newLineFlg = false;
    let arrCount = 0;
    for(let field of fields){
        sqlArrayPush(field,dialogId);
        if(typeof(field.options) == 'object' && 'newLine' in field.options && field.options.newLine){
            if(newLineFlg){
                content += "</div>";
            }
            let conteiner_width = Math.floor(100 / columnCountArr[arrCount]);
            content += `<div style="display: inline-block;width: ${conteiner_width}%;vertical-align: top;">`;
            content += createField(dialogId, field.type, field.id, field.label, field.options);
            newLineFlg = true;
        }
        else if(typeof(field.options) == 'object' && "newLine" in field.options && !field.options.newLine){
            if(newLineFlg){
                content += "</div>";
                content += `<div style="display: block;">`;
                content += createField(dialogId, field.type, field.id, field.label, field.options);
                newLineFlg = true;
                arrCount++;
            }
        }
        else{
            content += createField(dialogId, field.type, field.id, field.label, field.options);
        }

    }
    if(newLineFlg)content += "</div>";

    let btnContent = '';
    if(btnFields.length > 0){
        btnContent = btnFields.map(e => createButton(e.label,e.options.onclick ? e.options.onclick : '',e.options.icon,dialogId,e.id,e.options.hidden ? true:false)).join('');
    }
    const dialogHTML = createDialogHTML(dialogId, title, content,btnContent);
    $('#Application').append(dialogHTML);

    $(`#${dialogId}`).dialog({
        autoOpen: false
    })
    htmls[dialogId] = dialogHTML;

    // バリデーションイベント登録
    // for(let field of fields){
    //     create_varidate_event(dialogId,field.id,field.type,field.options);
    // }

    createLookups(dialogId, title, fields);
    // submitHandlerを設定
    $(`#${dialogId}_submit`).on('click', function() {
        submitDialog(dialogId, submitHandler);
    });

    // searchArr作成
    let search_items = fields.filter(e => e.type == 'search-table');
    if(search_items.length == 0)return;
    for(item of search_items){
        let tmp_searchObj = {dialogId:dialogId,tableId:item.id,searchTableId:item.options.searchTableId,multiple:item.options.multiple ? true:false,lastSql:lastSql,allCheck:item.options.allCheck ? true:false};
        // toColumns作成
        tmp_searchObj.toColumns = item.options.t_heads.map(e =>{
            tmp = {ColumnName:e.ColumnName};

            if('specialTerms'in e)tmp["specialTerms"] = e.specialTerms;
            if('alignment' in e)tmp["alignment"] = e.alignment;
            if(e.hidden)tmp["hidden"] = true;
            if(e.type)tmp["type"] = e.type;
            return tmp;

        });
        // options作成
        tmp_searchObj.options = [];
        let tmpArr;
        fields.forEach(e => {
            if('forColumnName' in e){
                tmpArr = {searchColumn:e.forColumnName,type:e.type,id:e.id};
                if("digitsNum" in e.options)tmpArr["digitsNum"] = e.options.digitsNum;
                if("alignment" in e.options)tmpArr["alignment"] = e.options.alignment;
                tmp_searchObj.options.push(tmpArr);
            }
        });
        searchArr.push(tmp_searchObj);

    }
    $(`#${dialogId}_searchButton`).on('click',function(e){
        create_searchList($(this));
    })



}

// ダイアログのベース構造を定義
const createDialogHTML = (dialogId, title, content, btnContent) => `
    <div id="${dialogId}" class="dialog" aria-label="${title}" title="${title}">
        ${content}
        <div class="command-center">
            ${btnContent}
            <button id="${dialogId}_close" class="button button-icon ui-button ui-corner-all ui-widget applied customized" type="button" onclick="closeDialog('${dialogId}',true);"style='position:relative;'>
                <span class="ui-button-icon ui-icon ui-icon-cancel"></span>
                <span class="ui-button-icon-space"> </span>終了
            </button>
        </div>
    </div>
`;

function createButton(label,onclick,icon,dialogId,id,hiddenFlg){
    return `
        <button id="${dialogId}_${id}" style="${hiddenFlg ? "display:none":''}" class="button button-icon ui-button ui-corner-all ui-widget applied customized" type="button" onclick="${onclick == '' ? '':onclick}">
                    <span class="ui-button-icon ui-icon ui-icon-${icon}"></span>
                    <span class="ui-icon-${icon}"> </span>${label}
        </button>
    `
}

// ダイアログを開く関数
function openDialog(dialogId,width='550',unit = 'px',func = null) {
    $(`#${dialogId}`).dialog('destroy');
    $(`#${dialogId}`).dialog({
        modal:true,
        width: `${width}${unit}`,
        resizable: true,
        closeOnEscape:false,
        open: function() {
            $(this).find("input:visible:first").focus().select();
        }
    })
    $(`#${dialogId}`).css('max-height',"90vh");
    dialog_openTime = getJPISOTime()

    if(typeof func == 'function'){
        func();
    }

}
// ダイアログ固有の送信処理を格納するオブジェクト
const dialogSubmitHandlers = {};

// ダイアログ内のフォームをSubmitする関数
function submitDialog(dialogId, submitHandler) {
    // バリデーションを実行
    if (!validateDialog(dialogId)) {
        return;
    }

    // 送信ハンドラを設定
    if (typeof submitHandler === 'function') {
        dialogSubmitHandlers[dialogId] = submitHandler;
    }

    // ダイアログ固有の送信処理を実行
    if (dialogSubmitHandlers[dialogId]) {
        dialogSubmitHandlers[dialogId](dialogId);
    }
    closeDialog(dialogId);
}

// ダイアログのバリデーションを行う関数
function validateDialog(dialogId) {
    let isValid = true;
    $(`#${dialogId} [required]`).each(function() {
        const field = $(this);
        const errorElement = $(`#${field.attr('id').replace(/(From|To)$/, '')}-error`);
        if (!field.val()) {
            isValid = false;
            field.addClass('error');
            errorElement.text('この項目は必須です');
        } else {
            field.removeClass('error');
            errorElement.text('');
        }
    });
    return isValid;
}

function editMarkdown(dialogId, id) {

    const fullId = `${dialogId}_${id}`;
    $(`#${fullId}`).show().focus();
    $(`#${fullId}_viewer`).hide();
}

// フィールドの有効/無効を切り替える関数
function toggleFieldDisabled(fieldId, disabled) {
    const field = $(`#${fieldId}`);
    field.prop('disabled', disabled);

    if (disabled) {
        field.addClass('disabled');
    } else {
        field.removeClass('disabled');
    }
}

function sqlArrayPush(field,dialogId){
    if(field.type == 'search-table' && field.options.sql)useSqlDialogs.push(dialogId);
    if(field.type == 'search-table' && field.options.individualSql)useIndividualSqlDialogs.push(dialogId);
}

/**
 * 現在の日本時間を ISO 8601 形式 (YYYY-MM-DDTHH:mm:ss) で取得する
 * @returns {string} 日本時間の ISO 8601 形式の文字列 (例: "2024-12-11T01:41:33")
 */
function getJPISOTime() {
    const now = new Date();
    const jpISOTime = new Date(now.getTime() - now.getTimezoneOffset() * 60000)
        .toISOString()
        .slice(0, 19);  // 秒まで切り取り
    return jpISOTime;
}

/**
 * 前回出力履歴を取得する
 * @param {string} listName     帳票名
 * @param {number} number       番号（見積番号等）
 * @param {string} id           結果を入力するid
 *  */
async function get_history(listName,number,id){
    try{
        const sql = `
            SELECT CreateDate, PCName
            From TW見積出力履歴
            WHERE ListName = '${listName}'
            AND Number = ${number}
        `;

        let res = await fetchSql(sql);
        res = res.results;

        if(res.length == 0){
            $(`#${id} .input-container span`).text('');
            return;
        }

        // 入力
        let tmp = formatDate_for_japanese(res[0].CreateDate) + "　" + formatTime_for_japanese(res[0].CreateDate) + "　" + res[0].PCName;

        $(`#${id} .input-container span`).text(tmp);

    }catch(err){
        $(`#${id} .input-container span`).text('');
        console.error('前回出力情報取得処理',err);
        return;
    }
}