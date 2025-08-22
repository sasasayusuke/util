const styleElement2 = document.createElement('style');
styleElement2.textContent = `
    .field-normal{
        width:340px ;
        max-width:340px ;
        height:45px ;
        padding:0 20px 10px 0 ;
    }
    .field-wide{
        width:100%;
        min-height:45px;
        float:left;
        padding:0 10px 10px 0;
        clear both;
            max-width:100%
    }
    .sdt_disabled input,
    .sdt_disabled select,
    .sdt_disabled textarea{
        background:#f5f5f5;
    }
    #productSearch{
        display:block;
        position:static;
        margin-left:320px;
        margin-top:6px;
        overflow:visible;
    }
    #productDelete{
        position:sticky;
        margin-left:335px;
        overflow:visible;
        margin-top:-50px;
    }
`;
document.head.appendChild(styleElement2);

lookups.push({
    id:"Results_ClassI",
    searchDialog:{id:"productSearch2",title:'製品No検索',multiple:false}
})

$(document).on('input','#Results_ClassI',async function(){
    try{
        const productNo = $('#Results_ClassI');
        const siyouNo = $('#Results_ClassD');
        if(productNo.val() == ""){
            initialize();
            return;
        }
        const arr = productNo.val().split('___');

        $p.set($p.getControl('ClassI'),arr[0].trim());
        $p.set($p.getControl('ClassD'),arr[1].trim());

        

        const cls_product = new ClsProduct(productNo.val().trim(),siyouNo.val().trim());
        const res = await cls_product.GetByData(true);

        if(!res){
            alert('指定の製品Noは存在しません。');
            initialize();
            return;
        }

        $p.set($p.getControl('ClassE'),cls_product["漢字名称"]);
        $p.set($p.getControl('NumA'),null_to_zero(cls_product["W"],0));
        $p.set($p.getControl('NumB'),null_to_zero(cls_product["D"],0));
        $p.set($p.getControl('NumC'),null_to_zero(cls_product["H"],0));
        $p.set($p.getControl('NumD'),null_to_zero(cls_product["D1"],0));
        $p.set($p.getControl('NumE'),null_to_zero(cls_product["D2"],0));
        $p.set($p.getControl('NumF'),null_to_zero(cls_product["H1"],0));
        $p.set($p.getControl('NumG'),null_to_zero(cls_product["H2"],0));

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})

function initialize(){
    $p.set($p.getControl('ClassI'),'');
    $p.set($p.getControl('ClassD'),'');
    $p.set($p.getControl('ClassE'),'');
    $p.set($p.getControl('NumA'),'');
    $p.set($p.getControl('NumB'),'');
    $p.set($p.getControl('NumC'),'');
    $p.set($p.getControl('NumD'),'');
    $p.set($p.getControl('NumE'),'');
    $p.set($p.getControl('NumF'),'');
    $p.set($p.getControl('NumG'),'');
}

// $p.events.on_editor_load = function () {
//     setDisabled('disabled');

//     $('.dialogSearch').off('click');

//     if($('#productSearch2Dialog').length == 0){
//         $('#Application').append(productSearch_html);
//     }

//     $('#productSearch').on('click',function(){
//         openDialog_for_searchDialog('productSearch2','Results_ClassI','1300px','製品検索ダイアログ');
//     })
// }

onEditorLoadFuncs.push(
  function () {
    setDisabled('disabled');

    $('.dialogSearch').off('click');

    if($('#productSearch2Dialog').length == 0){
        $('#Application').append(productSearch_html);
    }

    $('#productSearch').on('click',function(){
        openDialog_for_searchDialog('productSearch2','Results_ClassI','1300px','製品検索ダイアログ');
    })
  }
)

const productSearch_html = `

<div id="productSearch2Dialog" class="searchDialog ui-dialog-content ui-widget-content" style="display:none; margin-top: 10px; width: auto; min-height: 89.25px; max-height: 90vh; height: auto;">
        <div style="display: inline-block;width: 33%;vertical-align: top;">
            <div id="productSearch2_productNoField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_productNo" class="">製品No</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_productNo" name="productSearch2_productNo" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_productNo-error"></div>
            </div>
        
            <div id="productSearch2_methodNoField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_methodNo" class="">仕様No</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_methodNo" name="productSearch2_methodNo" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_methodNo-error"></div>
            </div>
        
            <div id="productSearch2_KANJINameField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_KANJIName" class="">漢字名称</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_KANJIName" name="productSearch2_KANJIName" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_KANJIName-error"></div>
            </div>
        
            <div id="productSearch2_mainSupplierField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_mainSupplier" class="">主仕入先</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_mainSupplier" name="productSearch2_mainSupplier" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_mainSupplier-error"></div>
            </div>
        </div><div style="display: inline-block;width: 33%;vertical-align: top;">
            <div id="productSearch2_WField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_W" class="">W</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_W" name="productSearch2_W" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_W-error"></div>
            </div>
        
            <div id="productSearch2_DField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_D" class="">D</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_D" name="productSearch2_D" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_D-error"></div>
            </div>
        
            <div id="productSearch2_HField" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_H" class="">H</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_H" name="productSearch2_H" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_H-error"></div>
            </div>
        </div><div style="display: inline-block;width: 33%;vertical-align: top;">
            <div id="productSearch2_D1Field" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_D1" class="">D1</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_D1" name="productSearch2_D1" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_D1-error"></div>
            </div>
        
            <div id="productSearch2_D2Field" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_D2" class="">D2</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_D2" name="productSearch2_D2" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_D2-error"></div>
            </div>
        
            <div id="productSearch2_H1Field" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_H1" class="">H1</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_H1" name="productSearch2_H1" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_H1-error"></div>
            </div>
        
            <div id="productSearch2_H2Field" class="field-wide both" style="">
                <p class="field-label"><label for="productSearch2_H2" class="">H2</label></p>
                <div class="field-control">
                    <div class="container-normal">
                        <div class="oneItem">
                            <input id="productSearch2_H2" name="productSearch2_H2" class="control-textbox     " type="text" style="">
                            
                            
                            
                    </div>
                    </div>
                </div>
                <div class="error-message" id="productSearch2_H2-error"></div>
            </div>
        
            
        </div><div style="display: block;">
            <div style="width:80%; display: flex;justify-content: end;">
                <button id="productSearch2_searchTable_searchButton" class="button button-icon ui-button ui-corner-all ui-widget applied customized" data-icon="ui-icon-search" type="button" style="z-index: 9999;" onclick="create_searchList($(this));">
                    <span class="ui-button-icon ui-icon ui-icon-search search" style="position: static;"></span>
                    <span class="ui-button-icon-space"> </span>検索
                </button>
            </div>
            <div id="productSearch2_searchTable_readTable" class="ui-dialog-content ui-widget-content" style="display: inline-block; margin-top:20px; width: 98%;margin-left:1%;">
                <div class="field-vertical " style="width: 100%;">
                    <div class="field-control">
                        <div class="container-selectable">
                            <div class="wrapper h300">
                                <table class="table searchTable">
                                    <thead style="position: sticky; top:0; background-color: gold; z-index:9999">
                                        <tr>
                                            <th style="text-align: center;border-collapse: collapse; display:none;"></th><th style="text-align: center;border-collapse: collapse; ">製品No</th><th style="text-align: center;border-collapse: collapse; ">仕様No</th><th style="text-align: center;border-collapse: collapse; ">漢字名称</th><th style="text-align: center;border-collapse: collapse; ">主仕入先</th><th style="text-align: center;border-collapse: collapse; ">W</th><th style="text-align: center;border-collapse: collapse; ">D</th><th style="text-align: center;border-collapse: collapse; ">H</th><th style="text-align: center;border-collapse: collapse; ">D1</th><th style="text-align: center;border-collapse: collapse; ">D2</th><th style="text-align: center;border-collapse: collapse; ">H1</th><th style="text-align: center;border-collapse: collapse; ">H2</th><th style="text-align: center;border-collapse: collapse; ">仕入先名</th>
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
        </div>
        <div class="command-center">
            
            <button id="productSearch2_close" class="button button-icon ui-button ui-corner-all ui-widget applied customized" type="button" onclick="closeDialog('productSearch2Dialog',false);" style="position:relative;">
                <span class="ui-button-icon ui-icon ui-icon-cancel"></span>
                <span class="ui-button-icon-space"> </span>終了
            </button>
        </div>
    </div>
`
htmls["productSearch2Dialog"] = productSearch_html;