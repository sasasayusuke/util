
{
	// 月次締め処理
	let category = "請求・売掛";
    let dialogId = "MonthlyClosing";
    let dialogName = "月次締め処理";
	createAndAddDialog(dialogId, dialogName, [
		{ type: 'radio', id: 'aggregateValue', label: '集計値', options: {
			width: 'wide',
			required:true,
			values:[
				{value:"買掛",text:'買掛',checked:true},
				{value:"売掛",text:'売掛'},
			]
		}},
		// { type: 'range-date', id: 'monthlyClosingDate', label: '月次締め日付', options: {
		// 	width: 'wide',
		// 	disabled:true,
		// }},
		{ type: 'free', id: 'monthlyClosingDate', label: '月次締め日付', options: {
			str:`
				<div id="MonthlyClosing_monthlyClosingDateField" class="field-wide both" style="">
                    <p class="field-label"><label for="MonthlyClosing_monthlyClosingDateFrom" class="">月次締め日付</label></p>
                    <div class="field-control">
                        <div class="container-normal">
                            <div class="range-text-container">
                                <input id="MonthlyClosing_monthlyClosingDateFrom" name="MonthlyClosing_monthlyClosingDateFrom" class="control-textbox " type="date" max="9999-12-31" disabled>
                                <span class="range-text-separator">→</span>
                                <input id="MonthlyClosing_monthlyClosingDateTo" name="MonthlyClosing_monthlyClosingDateTo" class="control-textbox " type="date" max="9999-12-31" disabled>
                            </div>
                        </div>
                    </div>
                    <div class="error-message" id="MonthlyClosing_monthlyClosingDate-error"></div>
                </div>
			`
		}
		},
		{ type: 'free', id: 'confirmation', label: '確認', options: {
			str:`
				<h3 style="color: #007bff; font-weight: bold; margin-top: 0; text-align: center;">確認事項</h3>
				<p style="margin-bottom: 10px; text-align: center;">上記年月の「月次締め処理」を行います。<br>
				よろしければ実行ボタンを押してください。</p>
				<p style="color: red; margin-bottom: 0; text-align: center;">この処理を行うと締め日付以前のデータは修正できません。<br>
				すべての情報が確定されていることを確認してください。</p>
			`
		}
		}
	],
	[
		{ type: 'button_inline', id: 'output', label: '実行', options: {
            icon:'disk',
			onclick:`UploadMonthlyClosing('${dialogId}','${category}','${dialogName}','実行');`
        }},
		{ type: 'button_inline', id: 'rollback', label: '戻し', options: {
            icon:'disk',
			onclick:`UploadBackMonthlyClosing('${dialogId}','${category}','${dialogName}','戻し');`
        }},
	]);

	// ダイアログを開くボタンを追加
	commonModifyLink(dialogName, function() {openDialog(dialogId,550,'px',async function(){
		try{
			const sql = `SELECT TOP 1 DataName FROM AppLockData`;

			try{
				var res = await fetchSql(sql);
			}catch(err){
				closeDialog(dialogId,true);
				return;
			}

			res = res.results;

			if(res.length != 0){
				alert("現在データベースが使用中です。処理は続行します");
			}

			await GetUriKaiDate()

		}catch(err){
			alert('予期せぬエラーが発生しました。');
			console.error(dialogName + "エラー" ,err);
			closeDialog(dialogId,true);
			return;
		}
	})})
}



let dates_for_MonthlyClosing = null;
async function GetUriKaiDate(){
	const nextMonthEnd = (date)=>{
		let tmp = new Date(date);
		return new Date(tmp.setMonth(tmp.getMonth()+2,0));
	}
	const cls_buy_dates = new clsDates('買掛月次更新日');
	const cls_sales_dates = new clsDates('売掛月次更新日');

	await cls_buy_dates.GetbyID();
	await cls_sales_dates.GetbyID();

	dates_for_MonthlyClosing = {'買掛月次更新日':cls_buy_dates.updateDate,'売掛月次更新日':cls_sales_dates.updateDate};

	$('#MonthlyClosing_monthlyClosingDateFrom').val(dates_for_MonthlyClosing.買掛月次更新日.toLocaleDateString('sv-SE'));
	$('#MonthlyClosing_monthlyClosingDateTo').val(nextMonthEnd(dates_for_MonthlyClosing.買掛月次更新日).toLocaleDateString('sv-SE'));
}

$(document).on('change','#MonthlyClosing_aggregateValueField input[type="radio"]',async function(){

	// 翌月末を取得
	const nextMonthEnd = (date)=>{
		let tmp = new Date(date);
		return new Date(tmp.setMonth(tmp.getMonth()+2,0));
	}

	const item = $(this).val();

	if(item == '買掛'){
		$('#MonthlyClosing_monthlyClosingDateFrom').val(dates_for_MonthlyClosing.買掛月次更新日.toLocaleDateString('sv-SE'));
		$('#MonthlyClosing_monthlyClosingDateTo').val(nextMonthEnd(dates_for_MonthlyClosing.買掛月次更新日).toLocaleDateString('sv-SE'));
	}else if(item == '売掛'){
		$('#MonthlyClosing_monthlyClosingDateFrom').val(dates_for_MonthlyClosing.売掛月次更新日.toLocaleDateString('sv-SE'));
		$('#MonthlyClosing_monthlyClosingDateTo').val(nextMonthEnd(dates_for_MonthlyClosing.売掛月次更新日).toLocaleDateString('sv-SE'));
	}

})

async function UploadMonthlyClosing(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

		if (confirm("処理を実行します。\n\n※データは確定されますので、再度確認を行って下さい。")) {
			if (confirm("月次締め処理を実行します。")) {

				let fromDt = new Date($(`#${dialogId}_monthlyClosingDateFrom`).val());
        		let toDt   = new Date($(`#${dialogId}_monthlyClosingDateTo`).val());

				const formatDate = (date) => {
					const year = date.getFullYear();
					const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
					const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
					return `${year}/${month}/${day}`;
				};

				let item_params = {
					"AggregateValue":$(`input[name="${dialogId}_aggregateValue"]:checked`).val(),
					"FromDt":formatDate(fromDt),
					"ToDt":formatDate(toDt),
					"LoginId":$("#LoginId").val(),
				};

				const param = {
					"category": category,
					"title": dialogName,
					"button": btnLabel,
					"user": $p.userName(),
					"opentime": dialog_openTime,
					"params": item_params
				};

				let res = await fetch(SERVICE_URL, {
					method: "POST",
					headers: {
						"content-type": "application/json",
						// "Accept": accept,
					},
					body: JSON.stringify(param)
				});

				if (!res.ok) {
					const errorData = await res.json();
					console.error(errorData);
					alert(errorData.message);
					return;
				} else {
					alert("月次締め処理は完了しました。");
					// 更新日付が変わるため画面をリロード
					location.reload();
				}
			}
		}

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}

async function UploadBackMonthlyClosing(dialogId,category,dialogName,btnLabel){

    try{
        // バリデーション
        if(!validateDialog(dialogId)){
            return;
        }

        console.log(`start : ${dialogName}`);

		let fromDt = new Date($(`#${dialogId}_monthlyClosingDateFrom`).val());
		let toDt   = new Date($(`#${dialogId}_monthlyClosingDateTo`).val());

		const firstDayOfCurrentMonth = new Date(fromDt.getFullYear(), fromDt.getMonth(), 1);
		const wDate = new Date(firstDayOfCurrentMonth - 1);

		const formatDate = (date) => {
			const year = date.getFullYear();
			const month = String(date.getMonth() + 1).padStart(2, '0'); // 月は0から始まるので+1
			const day = String(date.getDate()).padStart(2, '0'); // 日を2桁に
			return `${year}/${month}/${day}`;
		};

		if (confirm("戻し処理を行いますか？")) {
			if (confirm(`[${formatDate(fromDt)}] → [${formatDate(wDate)}] に戻ります。`)) {

				let item_params = {
					"AggregateValue":$(`input[name="${dialogId}_aggregateValue"]:checked`).val(),
					"FromDt":formatDate(fromDt),
					"ToDt":formatDate(toDt),
					"LoginId":$("#LoginId").val(),
				};

				const param = {
					"category": category,
					"title": dialogName,
					"button": btnLabel,
					"user": $p.userName(),
					"opentime": dialog_openTime,
					"params": item_params
				};

				let res = await fetch(SERVICE_URL, {
					method: "POST",
					headers: {
						"content-type": "application/json",
						// "Accept": accept,
					},
					body: JSON.stringify(param)
				});

				if (!res.ok) {
					const errorData = await res.json();
					console.error(errorData);
					alert(errorData.message);
					return;
				} else {
					alert("戻し処理は完了しました。");
					// 更新日付が変わるため画面をリロード
					location.reload();
				}
			}
		}

        console.log(`end : ${dialogName}`);

    }catch(err){
        alert('予期せぬエラーが発生しました。');
        console.error(dialogName + "エラー" ,err);
    }

}