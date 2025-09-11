
// URLSearchParams を用いて、クエリパラメータをsessionStorageから取得

// 遷移元画面から渡されたsessionstorageの識別子を取得
const queryParams_name = new URLSearchParams(window.location.search);
const session_storage_name = queryParams_name.get("siire_id");
// sessionStorageからクエリパラメータをJSON形式で取得
const session_json = JSON.parse(sessionStorage.getItem(session_storage_name));
// URLsearchparamsに変換（仕様変更の為、再度URLsearchparamsに変換）
const queryParams = new URLSearchParams(session_json);

const category = queryParams.get("category");
const title = queryParams.get("title");
const user = queryParams.get("user");
const opentime = queryParams.get("opentime");
const permittion = queryParams.get("permittion");

let view_list;
let record_count = 0;

window.onload = async function () {
	try {
		showLoading()

		let itemParams = {
			"処理区分名": queryParams.get("処理区分名"),
			"見積件名": queryParams.get("見積件名"),
			"仕入先名": queryParams.get("仕入先名"),
			"配送先名": queryParams.get("配送先名"),
			"仕入日付": queryParams.get("@i仕入日付"),
			"支払日付": queryParams.get("@i支払日付"),
            "外税対象額": queryParams.get("@i外税対象額"),
            "外税額": queryParams.get("@i外税額"),

			"@i処理区分": queryParams.get("@i処理区分"),
			"@i見積番号": queryParams.get("@i見積番号"),
			"@i仕入先CD": queryParams.get("@i仕入先CD"),
			"@i配送先CD": queryParams.get("@i配送先CD"),
			"@i仕入番号": queryParams.get("@i仕入番号"),

		};

		let fetchParams = {
			"category": category,
			"title": title,
			"button": queryParams.get("button"),
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


		// 初期表示用データ
		if (!res.ok) {
			const errorData = await res.json();
			console.error('[API-018]', errorData);
			throw new Error('[API-018] ' + JSON.stringify(errorData));
		}


		let context = await res.json();
		console.log(context)
		context["permittion"] = permittion

		record_count = context.data.length;

		// データが0件の場合はエラーメッセージを表示して画面を閉じる
		if (record_count === 0) {
			alert('[DB-004] 表示できる明細データがありません。');
			window.close();
			return;
		}

		// viewsを作成
        const columns = context.columns;
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

		
		context["gGetuDateFLG"] = false;
		if(queryParams.get('gGetuDateFLG') === 'true'){
			alert('更新済みの為、修正できません。');
			context["gGetuDateFLG"] = true;
		}
		
		if(queryParams.get('@i処理区分') == 1 && queryParams.get('支払更新FLG') == '1'){
			alert('検収処理済みデータです。');
		}

		if(context["gUcnt"] != 0){
			alert('原価未確定が存在します。');
		}

		// Reactをマウントするための要素の取得
		const mainId = 'MainContainer';
		const container = document.getElementById(mainId);



		// コンテナの初期化
		container.innerHTML = "";

		// Reactバンドルが読み込まれた後、window.FormLib があるはず
		if (window.FormLib) {
			try {
				window.FormLib.initFormShiire(mainId, context);
			} catch (error) {
				console.error('[GEN-019] FormLib の初期化中にエラーが発生しました:', error);
			}
		} else {
			console.error('[GEN-020] FormLib が見つかりません');
		}
		hideLoading()

    } catch(error) {
        console.error('[GEN-021] 予期せぬエラー：', error.message);
        // alert('予期せぬエラーが発生しました。');
        alert('[GEN-030]' + error.message);
        throw error;
    } finally {
        hideLoading();
    }

}

// ===================チェック処理=====================
async function checkForm(records) {
	try {

        // TD仕入明細内訳の見積引当数は未使用だがNullを許容していないので適当な定数を設定
		records.forEach(record => {
			record["仕入番号"] = queryParams.get("@i仕入番号");
			record["見積引当数"] = 1;
		});

		let itemParams = {
			"@i見積番号": queryParams.get("@i見積番号"),
			"@i仕入先CD": queryParams.get("@i仕入先CD"),
			"@i配送先CD": queryParams.get("@i配送先CD"),
			"@i得意先CD": queryParams.get("@i得意先CD"),
			"@i大小口区分": queryParams.get("@i大小口区分"),
			"@CompName": user,
			"入力データ": records
		};

		showLoading();
		let fetchParams = {
			"category": category,
			"title": title,
			"button": "チェック",
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

		hideLoading()
		if (!res.ok) {
			const errorData = await res.json();
			console.error('[API-018]', errorData);
			alert('[API-008] ' + errorData.message);
			return false
		} else {
			// alert("チェックが完了しました。");
			return true
		}

    } catch(error) {
        console.error('[GEN-023]', error.message);
        alert('[GEN-023]' + error.message);
        throw error;
    } finally {
        hideLoading();
    }

}

// ===================登録処理=====================
async function uploadForm(records,stocking_date,payment_date) {
	try {
		// TD仕入明細内訳の見積引当数は未使用だがNullを許容していないので適当な定数を設定
		records.forEach(record => {
			record["仕入番号"] = queryParams.get("@i仕入番号");
			record["見積引当数"] = 1;
		});

		let itemParams = {
            "@i仕入番号": queryParams.get("@i仕入番号"),
			"@i見積番号": queryParams.get("@i見積番号"),
			"@i仕入先CD": queryParams.get("@i仕入先CD"),
			"@i配送先CD": queryParams.get("@i配送先CD"),
			"@i得意先CD": queryParams.get("@i得意先CD"),

			"@i仕入日付": stocking_date,
            "@i支払日付": payment_date,
            "@i仕入先名1": queryParams.get("@i仕入先名1"),
            "@i仕入先名2": queryParams.get("@i仕入先名2"),
            "@i配送先名1": queryParams.get("@i配送先名1"),
            "@i配送先名2": queryParams.get("@i配送先名2"),

            "@i合計金額": queryParams.get("@i合計金額"),
            "@i税抜金額": queryParams.get("@i税抜金額"),
            "@i外税対象額": queryParams.get("@i外税対象額"),
            "@i外税額": queryParams.get("@i外税額"),

            "@i支払締日": queryParams.get("@i支払締日"),
            "@i税集計区分": queryParams.get("@i税集計区分"),
            "@i仕入端数": queryParams.get("@i仕入端数"),
            "@i消費税端数": queryParams.get("@i消費税端数"),

			"入力データ": records
		};

		showLoading()
		let fetchParams = {
			"category": category,
			"title": title,
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

		hideLoading()
		if (!res.ok) {
			const errorData = await res.json();
			console.error('[API-018]', errorData);
			alert('[API-008] ' + errorData.message);
			return false
		} else {

			// アンロック
			// if(queryParams.get('@i処理区分') == '1'){
			// 	await UnLockData('仕入番号',queryParams.get("@i仕入番号"));
			// }

			// localestorageに削除フラグを登録
			// localStorage.setItem(`${session_storage_name}_delete`,records.filter(e => e.CHECK == true).length  == record_count && queryParams.get('@i処理区分') == 0 ? 'false':'true');
			localStorage.setItem(`${session_storage_name}_delete`,'false');
			window.close();
		}

    } catch(error) {
        console.error('[GEN-024] 予期せぬエラー：', error.message);
        alert('[GEN-024] 登録処理' + error.message);
        throw error;
    } finally {
        hideLoading();
    }

}

// ===================全削除処理=====================
async function deleteForm() {
	try {

		let itemParams = {
			"@i見積番号": queryParams.get("@i見積番号"),
            "@i仕入番号": queryParams.get("@i仕入番号"),
			"@i得意先CD": queryParams.get("@i得意先CD"),
		};

		showLoading()
		let fetchParams = {
			"category": category,
			"title": title,
			"button": "全削除",
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

		hideLoading()
		if (!res.ok) {
			const errorData = await res.json();
			console.error('[API-018]', errorData);
			alert('[API-008] ' + errorData.message);
			return false
		} else {
			// 番号を確認
			// await UnLockData('仕入番号',queryParams.get("@i仕入番号"));
			// localestorageに削除フラグを登録
			localStorage.setItem(`${session_storage_name}_delete`,'false');
			window.close();
		}

    } catch(error) {
        console.error('[GEN-025] 予期せぬエラー：', error.message);
        alert('[GEN-025] 削除処理' + error.message);
        throw error;
    } finally {
        hideLoading();
    }

}

let ZEIRITU;
// ===================計算処理=====================
async function calc_total(data,payment_date,set税抜金額,set外税対象額,set外税,set内税対象額,set非課税金額,set合計金額,set売上合計){
	try{
		await sleep(0);
		const checked_rows = data.filter(e => e.CHECK);

		let check = 0;
		let getdata = 0;
		let i = 0;
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
		let sales_All = 0;

		for(const row of checked_rows){

			// 売上合計（ソース上にロジックが見当たらない）
			sales_All += row["仕入数量"] * row["売上単価"];


			// 伝票区分
            check = row["伝票区分"];
            getdata = check;
            wDenKB = check ?  getdata : "";

            // 合計金額
            check = row["仕入金額"];
            getdata = check;
            wKingak = check ? getdata : 0;

            // 仕入税区分
            check = row["仕入税区分"];
            getdata = check;
            wZeiKB = check ? getdata : 0;

            // 消費税額
            check = row["消費税額"];
            getdata = check;
            wZeikin = check ? getdata : 0;

			switch(wDenKB){
				case '1': //仕入
					switch(wZeikin){
						case 0: //外税
							wSiSoto += wKingak; //売上外税対象額
							wZeiTotal += wZeikin; //'明細単位の外税
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
						case 0: //外税
							wHeSoto += wKingak; //返品外税対象額
							wZeiTotal += wZeikin; //明細単位の外税
							break;
						case 1: //内税
							wHeUchi += wKingak; //返品内税対象額
							wHeUZei += wZeikin; //返品内税額
							break;
						default: // 非課税
							wHeHika += wKingak;
							break;
					}
					wTotal += wKingak;
					break;
			}
		}

		const tax_fraction = queryParams.get('税集計区分');
		switch(tax_fraction){
			case '0'://伝票単位
				wZeiTotal = ISHasuu_rtn(tax_fraction,(wSiSoto + wHeSoto) / 100 * ZEIRITU,0);
				break;
			case '1'://請求単位
				wZeiTotal = 0
				break;
			case '2':
				break;
		}

		set税抜金額(
			(wSiSoto + wSiUchi - wSiUZei) +
			(wHeSoto + wHeUchi - wHeUZei) +
			(wSiHika + wHeHika) +
			(wTeSoto + wTeUchi - wTeUZei)
		);

		set外税対象額((wSiSoto + wHeSoto).toLocaleString());
		set内税対象額((wSiUchi + wHeUchi).toLocaleString());
		set非課税金額((wSiHika + wHeHika).toLocaleString());
		set外税((wZeiTotal).toLocaleString());
		set合計金額(wTotal.toLocaleString());
		set売上合計(sales_All.toLocaleString());


	}catch(e){
		// alert('予期せぬエラーが発生しました。');
		console.error('[GEN-031]', e);
		alert('[GEN-031] 計算処理' + e.message);
		return;
	}
}

// 未使用のためコメントアウト
// async function getSeihin(filter) {

// 	showLoading()
// 	let data = await commonGetData(MASTER_TABLES['製品マスタ'], [], filter)

// 	hideLoading();
// 	return data
// }


function get_tax(data,rowIndex){
	try{
		const row = data[rowIndex];
		let check = 0;
		let getdata = 0;
		let wSirKin = 0;
		let wZeikin = 0;
		let wZeiKB  = 0;
		let wDenKB  = 0;

		check = row["伝票区分"];
		getdata = check;
		wDenKB = check ? getdata : "";

		 // 仕入金額
		check = row["仕入金額"];
		getdata = check;
		wSirKin = check ? getdata : 0;

		// 仕入税区分
		check = row["仕入税区分"];
		getdata = check;
		wZeiKB = check ? getdata : 0;

		switch(wZeiKB){
			case 0:	//外税
				switch(wDenKB){
					case "3":
						wZeikin = 0;
						break;
					default:
						switch(queryParams.get('税集計区分')){
							case "2":
								wZeikin = ISHasuu_rtn(queryParams.get('消費税端数'),(null_to_zero(wSirKin,0) / 100 * ZEIRITU),0);
								break;
							default:
								wZeikin = 0;
								break;
						}
				}
				break;
			case 1: //内税
				switch(wDenKB){
					case "3"://訂正
						wZeikin = 0;
						break;
					default:
						wZeikin = ISHasuu_rtn(queryParams.get('消費税端数'),(null_to_zero(wSirKin,0) / (100 + ZEIRITU) * ZEIRITU),0);
						break;
				}
				break;
		}
		return wZeikin;
	}catch(e){
		// alert('予期せぬエラーが発生しました。');
		console.error('[GEN-032]', e);
		alert('[GEN-032] 税取得処理' + e.message);
		return;
	}
}