{
    // 三和サービス実績入力
	let category = "マスタ";
    let dialogId = "sanwaServiceAchivementInput";
    let dialogName = "三和サービス実績入力";
	let table1Id = "tbl1"
	let table2Id = "tbl2"
	let table1Labels = [
		"三和商研",
		"店装技研",
		"サンワP",
		"日本運輸荷造",
		"トランコム",
		"その他",
		"事故",
	]
	let table2Labels = [
		"ルートサービス",
		"丸和運輸",
		"その他",
		"事故",
	]
    createAndAddDialog(dialogId, dialogName,
		[
			{ type: 'datepicker', id: 'AchivementDate', label: '実績日付', options: {
				width: 'normal',
				required:true,
				format:"month"
			}},
			{ type: 'input-table', id: table1Id, options: {
				row: table1Labels.length * 2,
				t_heads:[
					{label:'売上税抜金額',ColumnName:''},
					{label:'原価税抜金額',ColumnName:''},
				]
			}},
			{ type:'text', id:`${table1Id}PersonExpense`, label:'人件一般費', options:{
				width:"normal",
				varidate:{type:'zeroPadding',maxlength:8},
			}},
			{ type:'text', id:`${table1Id}NonSaleExpense`, label:'営業外費', options:{
				width:"normal",
				varidate:{type:'zeroPadding',maxlength:8},
			}},
			{ type: 'input-table', id: table2Id, options: {
				row: table2Labels.length,
				t_heads:[
					{label:'売上税抜金額',ColumnName:''},
					{label:'原価税抜金額',ColumnName:''},
				]
			}},
			{ type:'text', id:`${table2Id}PersonExpense`, label:'人件一般費', options:{
				width:"normal",
				varidate:{type:'zeroPadding',maxlength:8},
			}},
			{ type:'text', id:`${table2Id}NonSaleExpense`, label:'営業外費', options:{
				width:"normal",
				varidate:{type:'zeroPadding',maxlength:8},
			}},
		],

		[
            { type: 'button_inline', id: 'delete', label: '削除', options: {
				icon:'disk',
                onclick:`sanwaServiceAchivementInput_func('${dialogId}','${category}','${dialogName}','削除');`
			}},
            { type: 'button_inline', id: 'stop', label: '中止', options: {
				icon:'disk',
                onclick:'initialize_for_sanwaService(true)'
			}},
			{ type: 'button_inline', id: 'create', label: '保存', options: {
				icon:'disk',
                onclick:`sanwaServiceAchivementInput_func('${dialogId}','${category}','${dialogName}','保存');`
			}},
		]
	);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'750','px',function(){
        $('#sanwaServiceAchivementInput input[type="text"]').css('text-align','end').addClass('varidate_int').addClass('input-Number').attr('maxlength', 8);
    })})

	// テーブルを強化する関数
	function enhanceTable(id, headerHtml, categories) {
		const div = document.getElementById(id)
		if (!div) {
			let message = "テーブル取得に失敗しました"
			throw new Error(message)
		}
		const table = div.querySelector('.table')
		if (!table) {
			let message = "テーブル取得に失敗しました"
			throw new Error(message)
		}
		let style = document.createElement('style');
		style.textContent = `
			.category {
				background-color: #D3D3D3;
				font-weight: bold;
			}
			.subcategory {
				background-color: #F0F0F0;
				text-align: left;
				padding-left: 10px;
			}
			.vertical-text {
				writing-mode: vertical-rl;
				text-orientation: upright;
				white-space: nowrap;
				padding: 10px 5px;
				font-weight: bold;
				background-color: #D3D3D3;
			}
		`;
		document.head.appendChild(style);


		// ヘッダーを修正
		const thead = table.querySelector('thead');
		thead.innerHTML = headerHtml;

		// 既存の入力フィールドを数値入力に変更
		// table.querySelectorAll('input[type="text"]').forEach(input => {
		// 	input.type = 'text';
		// 	input.min = '0';
		// 	input.step = '1';
		// });

		// カテゴリーとサブカテゴリーを追加
		const tbody = table.querySelector('tbody');

		categories.forEach((category, categoryIndex) => {
			category.subcategories.forEach((subcategory, index) => {
				const row = tbody.rows[categoryIndex * 7 + index];
				if (!row) return;

				if (index === 0) {
					const categoryCell = row.insertCell(0);
					categoryCell.className = 'highlight vertical-text';
					categoryCell.rowSpan = category.subcategories.length;
					categoryCell.textContent = category.name;
				}

				const subcategoryCell = row.insertCell(index === 0 ? 1 : 0);
				subcategoryCell.className = 'subcategory';
				subcategoryCell.textContent = subcategory;
			});
		});
	}

	enhanceTable(`${dialogId}_${table1Id}_inputTable`,
		`
			<tr>
				<th colspan="4" style="text-align: center;border-collapse: collapse;">貨物</th>
			</tr>
			<tr>
				<th style="text-align: center;border-collapse: collapse;"></th>
				<th style="text-align: center;border-collapse: collapse;"></th>
				<th style="text-align: center;border-collapse: collapse;">売上税抜金額</th>
				<th style="text-align: center;border-collapse: collapse;">原価税抜金額</th>
			</tr>
		`,
		[
			{ name: '自車', subcategories: table1Labels},
			{ name: '庸車', subcategories: table1Labels},
		]
	)
	enhanceTable(`${dialogId}_${table2Id}_inputTable`,
		`
			<tr>
				<th colspan="4" style="text-align: center;border-collapse: collapse;">物流</th>
			</tr>
			<tr>
				<th style="text-align: center;border-collapse: collapse;"></th>
				<th style="text-align: center;border-collapse: collapse;"></th>
				<th style="text-align: center;border-collapse: collapse;">売上税抜金額</th>
				<th style="text-align: center;border-collapse: collapse;">原価税抜金額</th>
			</tr>
		`,
		[
			{ name: '', subcategories: table2Labels},
		]
	)
}

async function sanwaServiceAchivementInput_func(dialogId,category,dialogName,btnLabel){

	try{
		// バリデーション
		if(!validateDialog(dialogId)){
			return;
		}

		const result = document.getElementsByClassName('ui-dialog-title');
		var button_str = btnLabel
		if(!confirm(button_str + 'します。\nよろしいですか？'))return;
		if(result[0].textContent == '三和サービス実績入力  -- 修正'){
			if(button_str == '保存'){
				button_str = '更新'
			}
		}

		var AchivementDate = $(`#sanwaServiceAchivementInput_AchivementDate`).val() + '-01';

		console.log(`start : ${dialogName}`);

		// 画面項目のデータ取得
		let item_params = {
			"@年月":SpcToNull(AchivementDate.replaceAll('-','/')),
			"@売額_貨物自_三商":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_0_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_三商":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_0_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_店装":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_1_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_店装":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_1_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_サンプラ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_2_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_サンプラ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_2_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_三シャ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_3_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_三シャ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_3_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_トラン":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_4_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_トラン":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_4_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_その他":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_5_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_その他":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_5_1 input`).val().replaceAll(',','')),
			"@売額_貨物自_事故":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_6_0 input`).val().replaceAll(',','')),
			"@原額_貨物自_事故":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_6_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_三商":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_7_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_三商":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_7_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_店装":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_8_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_店装":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_8_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_サンプラ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_9_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_サンプラ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_9_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_三シャ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_10_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_三シャ":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_10_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_トラン":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_11_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_トラン":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_11_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_その他":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_12_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_その他":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_12_1 input`).val().replaceAll(',','')),
			"@売額_貨物庸_事故":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_13_0 input`).val().replaceAll(',','')),
			"@原額_貨物庸_事故":SpcToNull($(`#sanwaServiceAchivementInput_tbl1_13_1 input`).val().replaceAll(',','')),
			"@人件一般費_貨物":SpcToNull($('#sanwaServiceAchivementInput_tbl1PersonExpense').val().replaceAll(',','')),
			"@営業外費_貨物":SpcToNull($('#sanwaServiceAchivementInput_tbl1NonSaleExpense').val().replaceAll(',','')),
			"@売額_物流_ルート":SpcToNull($('#sanwaServiceAchivementInput_tbl2_0_0 input').val().replaceAll(',','')),
			"@原額_物流_ルート":SpcToNull($('#sanwaServiceAchivementInput_tbl2_0_1 input').val().replaceAll(',','')),
			"@売額_物流_丸和":SpcToNull($('#sanwaServiceAchivementInput_tbl2_1_0 input').val().replaceAll(',','')),
			"@原額_物流_丸和":SpcToNull($('#sanwaServiceAchivementInput_tbl2_1_1 input').val().replaceAll(',','')),
			"@売額_物流_その他":SpcToNull($('#sanwaServiceAchivementInput_tbl2_2_0 input').val().replaceAll(',','')),
			"@原額_物流_その他":SpcToNull($('#sanwaServiceAchivementInput_tbl2_2_1 input').val().replaceAll(',','')),
			"@売額_物流_事故":SpcToNull($('#sanwaServiceAchivementInput_tbl2_3_0 input').val().replaceAll(',','')),
			"@原額_物流_事故":SpcToNull($('#sanwaServiceAchivementInput_tbl2_3_1 input').val().replaceAll(',','')),
			"@人件一般費_物流":SpcToNull($('#sanwaServiceAchivementInput_tbl2PersonExpense').val().replaceAll(',','')),
			"@営業外費_物流":SpcToNull($('#sanwaServiceAchivementInput_tbl2NonSaleExpense').val().replaceAll(',','')),
			"LoginId":$("#LoginId").val(),
		};
		
		const param = {
			"category": category,
			"title": dialogName,
			"button": button_str,
			"user": $p.userName(),
			"opentime": dialog_openTime,
			"params": item_params
		};
		let filename = ""

		try{
            await download_report(param,filename);
        }catch{
            return;
        }

		console.log(`end : ${dialogName}`);

		if(button_str == '削除'){
			initialize_for_sanwaService(false)
		} else {
			// ヘッダー変更
			$('#sanwaServiceAchivementInput').prev().eq(0).children('span').text('三和サービス実績入力  -- 修正')
		}

	}catch(err){
		alert('予期せぬエラーが発生しました。');
		console.error(dialogName + "エラー" ,err);
	}

}

function initialize_for_sanwaService(flg){
    if(flg){
        if(!confirm('現在の編集内容を破棄します。\nよろしいですか？'))return;
    }
    $('#sanwaServiceAchivementInput input').val('');
    $('#sanwaServiceAchivementInput_AchivementDate').val(new Date().toLocaleDateString('sv-SE').slice(0,7));
	// ヘッダー変更
	$('#sanwaServiceAchivementInput').prev().eq(0).children('span').text('三和サービス実績入力')
}

$(document).on('blur','#sanwaServiceAchivementInput_AchivementDate',async function(){
    try{
        const date = $('#sanwaServiceAchivementInput_AchivementDate');

        if(date.val() == ""){
            return;
        }
        const query = `SELECT * FROM TMサービス実績情報 WHERE 年月 = '${date.val()}-01'`;
        try{
            var res = await fetchSql(query);
        }catch(e){
            return;
        }
        res = res.results;
        if(res.length == 0){
			$(`#sanwaServiceAchivementInput_tbl1_0_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_0_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_1_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_1_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_2_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_2_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_3_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_3_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_4_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_4_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_5_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_5_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_6_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_6_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_7_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_7_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_8_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_8_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_9_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_9_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_10_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_10_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_11_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_11_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_12_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_12_1 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_13_0 input`).val(0).trigger('blur');
			$(`#sanwaServiceAchivementInput_tbl1_13_1 input`).val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl1PersonExpense').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl1NonSaleExpense').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_0_0 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_0_1 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_1_0 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_1_1 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_2_0 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_2_1 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_3_0 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2_3_1 input').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2PersonExpense').val(0).trigger('blur');
			$('#sanwaServiceAchivementInput_tbl2NonSaleExpense').val(0).trigger('blur');
            // ヘッダー変更
			$('#sanwaServiceAchivementInput').prev().eq(0).children('span').text('三和サービス実績入力')
            return;
        }
        res = res[0];
        $(`#sanwaServiceAchivementInput_tbl1_0_0 input`).val(res.売額_貨物自_三商).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_0_1 input`).val(res.原額_貨物自_三商).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_1_0 input`).val(res.売額_貨物自_店装).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_1_1 input`).val(res.原額_貨物自_店装).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_2_0 input`).val(res.売額_貨物自_サンプラ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_2_1 input`).val(res.原額_貨物自_サンプラ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_3_0 input`).val(res.売額_貨物自_三シャ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_3_1 input`).val(res.原額_貨物自_三シャ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_4_0 input`).val(res.売額_貨物自_トラン).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_4_1 input`).val(res.原額_貨物自_トラン).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_5_0 input`).val(res.売額_貨物自_その他).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_5_1 input`).val(res.原額_貨物自_その他).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_6_0 input`).val(res.売額_貨物自_事故).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_6_1 input`).val(res.原額_貨物自_事故).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_7_0 input`).val(res.売額_貨物庸_三商).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_7_1 input`).val(res.原額_貨物庸_三商).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_8_0 input`).val(res.売額_貨物庸_店装).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_8_1 input`).val(res.原額_貨物庸_店装).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_9_0 input`).val(res.売額_貨物庸_サンプラ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_9_1 input`).val(res.原額_貨物庸_サンプラ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_10_0 input`).val(res.売額_貨物庸_三シャ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_10_1 input`).val(res.原額_貨物庸_三シャ).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_11_0 input`).val(res.売額_貨物庸_トラン).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_11_1 input`).val(res.原額_貨物庸_トラン).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_12_0 input`).val(res.売額_貨物庸_その他).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_12_1 input`).val(res.原額_貨物庸_その他).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_13_0 input`).val(res.売額_貨物庸_事故).trigger('blur');
        $(`#sanwaServiceAchivementInput_tbl1_13_1 input`).val(res.原額_貨物庸_事故).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl1PersonExpense').val(res.人件一般費_貨物).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl1NonSaleExpense').val(res.営業外費_貨物).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_0_0 input').val(res.売額_物流_ルート).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_0_1 input').val(res.原額_物流_ルート).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_1_0 input').val(res.売額_物流_丸和).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_1_1 input').val(res.原額_物流_丸和).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_2_0 input').val(res.売額_物流_その他).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_2_1 input').val(res.原額_物流_その他).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_3_0 input').val(res.売額_物流_事故).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2_3_1 input').val(res.原額_物流_事故).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2PersonExpense').val(res.人件一般費_物流).trigger('blur');
        $('#sanwaServiceAchivementInput_tbl2NonSaleExpense').val(res.営業外費_物流).trigger('blur');

        // ヘッダー変更
        $('#sanwaServiceAchivementInput').prev().eq(0).children('span').text('三和サービス実績入力  -- 修正')

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(err);
        return;
    }
})

$(document).on('blur','#sanwaServiceAchivementInput_tbl1_0_0 input',async function(){
	i = '1'
	if ($(`#sanwaServiceAchivementInput_tbl1_0_0 input`).val().length > 8){
		console.log('桁数オーバーです');
	}
})