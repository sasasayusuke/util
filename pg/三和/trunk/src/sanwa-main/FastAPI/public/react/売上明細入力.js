let tax_category   = "";
let sales_fraction = "";
let tax_fraction   = "";

// モーダル環境でのパラメータ取得
function getModalParams() {
    if (window.uriageModalContext) {
        // モーダル環境
        return window.uriageModalContext.modalParams;
    } else {
        // 従来の別タブ環境（後方互換性）
        const queryParams_name = new URLSearchParams(window.location.search);
        const session_storage_name = queryParams_name.get("uriage_id");
        const session_json = JSON.parse(sessionStorage.getItem(session_storage_name));
        return session_json;
    }
}

const modalParams = getModalParams();
const category = modalParams["category"];
const title = modalParams["title"];
const user = modalParams["user"];
const opentime = modalParams["opentime"];

const update_flg = modalParams['更新フラグ'] === 'true' ? true:false; //更新可能の場合はtrue
const process_category = modalParams["@i処理区分"];


let view_list;
let ZEIRITU;
window.onload = async function () {
    try{
		showLoading();

        await get_taxRate(modalParams['売上日付']);

        let itemParams = {
            "処理区分名": modalParams["処理区分名"],
            "抽出区分名": modalParams["抽出区分名"],
            "見積件名": modalParams["見積件名"],
            "得意先CD": modalParams["得意先CD"],
            "得意先名": modalParams["得意先名"],
            "納入得意先CD": modalParams["納入得意先CD"],
            "納入先CD": modalParams["納入先CD"],
            "納入先名": modalParams["納入先名"],
            "売上日付": modalParams['売上日付'],
            "更新フラグ": modalParams['更新フラグ'],

            "@i処理区分": modalParams["@i処理区分"],
            "@i見積番号": modalParams["@i見積番号"],
            "@i売上番号": modalParams["@i売上番号"],
            "@i抽出区分": modalParams["@i抽出区分"],
            "@is仕入日付": modalParams["@is仕入日付"],
            "@ie仕入日付": modalParams["@ie仕入日付"],
        };

        let fetchParams = {
            "category": modalParams["category"],
            "title": modalParams["title"],
            "button": modalParams["button"],
            "user": modalParams["user"],
            "opentime": modalParams["opentime"],
            "params": itemParams
        }

        tax_category   = modalParams['税集計区分'];
        sales_fraction = modalParams['売上端数'];
        tax_fraction   = modalParams['消費税端数'];

        console.log('パラメータ',fetchParams)

        let res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
                // "Accept": accept,
            },
            body: JSON.stringify(fetchParams)
        });

        if(!res.ok){
            const err = await res.json();
            throw new Error(err);
        }

        // 初期表示用データ
        let context = await res.json();
        console.log("結果リザルト",context);

        // viewsを作成
        const columns = context.content.columns;
        const view_names = columns[0].views;
        let views = {};
        view_names.forEach((e)=>{
            views[e] = [];
        })
        
        columns.forEach((e) =>{
            if(!('views' in e)){
                for(const view in views){
                    views[view].push(e);
                }
            }else{
                e.views.forEach((j) =>{
                    views[j].push(e);
                })
            }
        })
        view_list = views;
        
        if(context["content"]["gUcnt"] != 0){
            alert('原価売価未確定が存在します。');
        }

        // Reactをマウントするための要素の取得
        const mainId = 'MainContainer';
        const container = document.getElementById(mainId);


        // コンテナの初期化
        container.innerHTML = "";

        // Reactバンドルが読み込まれた後、window.FormLib があるはず
        if (window.FormLib) {
            try {
                // modalParamsをcontextに追加
                context.modalParams = modalParams;
                context.view_list = view_list;
                window.FormLib.initFormUriage(mainId, context);
            } catch (error) {
                console.error('FormLib の初期化中にエラーが発生しました:', error);
            }
        } else {
            console.error('FormLib が見つかりません');
        }
    }catch(e){
        console.error('明細読み込み処理エラー',e);
        alert('予期せぬエラーが発生しました。');
    }finally{
        hideLoading();
    }
    

}
  // ===================税率取得=====================
async function get_taxRate(date){
    try {
        var res = await GetTax(new Date(date));
        ZEIRITU = res.results[0].税率;
    } catch (e) {
        console.error("Error fetching tax:", e);
    }
}

  // ===================チェック処理=====================
function checkForm(records,sales_date,gGetuDate){
    try{
        if(sales_date== ""){
            alert('売上日付を入力して下さい。');
            return false;
        }
        if(new Date(sales_date) <= gGetuDate){
            alert('更新済みの為、修正できません。');
            return false;
        }
        if(records.length == 0){
            alert('明細がありません。');
            return false;
        }

        let j = 0;
        let i = 1;
        for(const record of records){
            if(record["見積明細連番"] != 0 && record["CHECK"]){
                j++;
            }
            if(record["CHECK"] && record["伝票区分"] == ""){
                alert(`伝票区分（${i}行目）を入力して下さい。`);
                return false;
            }
            i++;
        }
        if(j == 0){
            alert('選択されていません。');
            return false;
        }
        return true;


    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return false;
    }
}
  // ===================登録処理=====================
async function upload(records,sales_date,gGetuDate,税抜金額,外税対象額,外税,非課税金額,合計金額){
    try{
        // 更新処理時のリアルタイム判定（別タブ版と同じ動き）
        if(modalParams["@i処理区分"] == 1 && modalParams["月次更新日"] && new Date(sales_date) <= new Date(modalParams["月次更新日"])){
            alert('更新済みの為、修正できません。');
            return;
        }
        
        if(checkForm(records,sales_date,gGetuDate) == false){
            return;
        }
        
        const confirmOk = confirm("保存します。\n請求済みの場合、金額の再計算を行なって下さい。");
        if (!confirmOk) {
            return;
        }

        const itemParams = {
            "@i売上番号": modalParams["@i売上番号"],
            "@i見積番号": modalParams["@i見積番号"],
            "@i売上日付": sales_date,

            "@i合計金額"  : 合計金額,
            "@i税抜金額"  : 税抜金額,
            "@i外税対象額": 外税対象額,
            "@i外税額"    : 外税,

            "@i税集計区分": modalParams['税集計区分'],
            "@i売上端数": modalParams['売上端数'],
            "@i消費税端数": modalParams['消費税端数'],

            "@i得意先CD": modalParams['得意先CD'],

            "入力データ": records
        };

        showLoading()
        let fetchParams = {
            "category": '売上・入金',
            "title": '売上入力',
            "button": "登録",
            "user": user,
            "opentime": opentime,
            "params": itemParams
        }

        console.log(fetchParams)

        let res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
                // "Accept": accept,
            },
            body: JSON.stringify(fetchParams)
        });

        if (!res.ok) {
            const errorData = await res.json();
            console.error(errorData);
            alert(errorData.message);
            return false
        } 
        else {
            // アンロック
            if(modalParams['@i処理区分'] == '1'){
                await UnLockData('売上番号',modalParams["@i売上番号"]);
            }
            
            // モーダルの場合はコールバック関数を呼び出し
            if (window.uriageModalContext) {
                window.uriageModalContext.onComplete();
            } else {
                // 従来の別タブ環境（後方互換性）
                localStorage.setItem(`${session_storage_name}_delete`,'false');
                window.close();
            }
        }
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return false;
    }
    finally{
        hideLoading();
    }

}

  // ===================削除処理=====================
async function purge(){
    try{
        if(!confirm('表示されている明細情報をすべて削除します。')){
            return;
        }

        showLoading();
        // ===================チェック処理=====================
        let itemParams = {
            "@i見積番号": modalParams["@i見積番号"]
        }
        let fetchParams = {
            "category": '売上・入金',
            "title": '売上入力',
            "button": "削除チェック",
            "user": user,
            "opentime": opentime,
            "params": itemParams
        }

        let res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
                // "Accept": accept,
            },
            body: JSON.stringify(fetchParams)
        });


        if(!res.ok){
            const err = await res.json();
            throw new Error(err);
        }
        res = await res.json();
        flg = res.flg;
        if(flg){
            if(!confirm('請求済みなので請求情報も削除されます。\n削除後、金額の再計算を行ってください。')){
                return;
            }
        }
        

        // ===================削除処理=====================
        itemParams = {
            "@i売上番号": modalParams["@i売上番号"],
            "@i見積番号": modalParams["@i見積番号"]
        }
    
        fetchParams = {
            "category": '売上・入金',
            "title": '売上入力',
            "button": "削除",
            "user": user,
            "opentime": opentime,
            "params": itemParams
        }
    
        console.log(fetchParams)
    
        res = await fetch(SERVICE_URL, {
            method: "POST",
            headers: {
                "content-type": "application/json",
                // "Accept": accept,
            },
            body: JSON.stringify(fetchParams)
        });

        if(!res.ok){
            return;
        }
        res = await res.json();

        // ロック解除
        await UnLockData('売上番号',modalParams["@i売上番号"]);
        
        // モーダルの場合はコールバック関数を呼び出し
        if (window.uriageModalContext) {
            window.uriageModalContext.onComplete();
        } else {
            // 従来の別タブ環境（後方互換性）
            localStorage.setItem(`${session_storage_name}_delete`,'false');
            window.close();
        }

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return false;
    }
    finally{
        hideLoading();
    }
}

  // ===================合計計算処理=====================
async function calc_total(data,sales_date,set税抜金額,set外税対象額,set外税,set非課税金額,set合計金額){
    try{
        // チェックされている行のデータを取得
        const checked_rows = data.filter(e => e.CHECK);
        
        let check = null;
        let getdata = "";
        let wDenKB  = 0;
        let wZeiKB  = 0;
        let wTotal  = 0;
        let wKingak = 0;
        let wZeikin = 0;
        let wSiSoto = 0;
        let wSiUchi = 0;
        let wSiUZei = 0;
        let wHeSoto = 0;
        let wHeUchi = 0;
        let wHeUZei = 0;
        let wTeSoto = 0;
        let wTeUchi = 0;
        let wTeUZei = 0;
        let wZeiTotal = 0;
        let wSiHika = 0;
        let wHeHika = 0;

        for (const row of checked_rows){

            // 伝票区分
            check = row["伝票区分"];
            getdata = check;
            wDenKB = check ?  getdata : "";

            // 合計金額
            check = row["売上金額"];
            getdata = check;
            wKingak = check ? getdata : 0;

            // 売上税区分
            check = row["売上税区分"];
            getdata = check;
            wZeiKB = check ? getdata : 0;

            // 消費税額
            check = row["消費税額"];
            getdata = check;
            wZeikin = check ? getdata : 0;

            switch(wDenKB){
                case '1': //売上
                    switch(wZeiKB){
                        case 0: //外税
                            wSiSoto += wKingak; //売上外税対象額
                            wZeiTotal += wZeikin; //明細単位の外税
                            break;
                        case 1: //内税
                            wSiUchi += wKingak; //売上内税対象額
                            wSiUZei += wZeikin; //売上内税額
                            break;
                        default: //非課税
                            wSiHika += wKingak;
                            break;
                    }
                    wTotal += wKingak;
                    break;
                case '2': //返品
                    switch(wZeiKB){
                        case 0://外税
                            wHeSoto += wKingak;//返品外税対象額
                            wZeiTotal += wZeikin;//明細単位の外税
                            break;
                        case 1://内税
                            wHeUchi += wKingak; // 返品内税対象額
                            wHeUZei += wZeikin; // 返品内税額
                            break;
                        default: //非課税
                            wHeHika += wKingak;
                            break;
                    }
                    wTotal += wKingak;
                    break;
                case '3': //訂正
                    if(wZeiKB == 0){//外税
                        wTeSoto += wKingak; //訂正外税対象額
                    }else{
                        wTeUchi += wKingak; //訂正内税対象額
                    }
                    wTeUZei += wZeikin; //訂正内税額
                    wTotal += wKingak;
                    break;
            }

            switch(tax_category){
                case '0': //伝票単位
                    wZeiTotal = ISHasuu_rtn(tax_fraction,(wSiSoto + wHeSoto) / 100 * ZEIRITU,0);
                    break;
                case '1': //請求単位
                    wZeiTotal = 0;
                    break;
                case '2': //明細単位
                    wZeiTotal = 0;
                    break;
                case '3': //税対象外（免税）
                    wZeiTotal = 0;
                    break;
            }
        }
        set税抜金額(
            (wSiSoto + wSiUchi - wSiUZei) + 
            (wHeSoto + wHeUchi - wHeUZei) +
            (wSiHika + wHeHika) +
            (wTeSoto + wTeUchi - wTeUZei)
        );
        set外税対象額(tax_category == '3' ? 0 : (wSiSoto + wHeSoto).toLocaleString());
        set非課税金額((wSiHika + wHeHika).toLocaleString());
        set外税(wZeiTotal.toLocaleString());
        set合計金額(wTotal.toLocaleString());


    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}