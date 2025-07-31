// 経費入力
{
	let category = "経費"
	let dialogId = "ExpenseEntry";
    let dialogName = "経費入力";
	let tableColumns = [
		{label:'科目',ColumnName:'',type:'text-set',search:true,varidateFrom:{type:'int',maxLength:4}},
		{label:'補助科目',ColumnName:'',type:'text-set',search:true,varidateFrom:{type:'int',maxLength:4}},
		{label:'金額/消費税',ColumnName:'',type:'text-set',alignment:'end',to_disabled:false,varidateFrom:{type:'int'},varidateTo:{type:'int'}},
		{label:'担当者',ColumnName:'',type:'text-set',search:true,varidateFrom:{type:'int',maxLength:2}},
		{label:'購入先',ColumnName:'',varidate:{type:'str',maxLength:30}},
		{label:'摘要',ColumnName:'',type:'text-set',search:true,to_disabled:false,varidateFrom:{type:'int',maxLength:2}},
	]
	createAndAddDialog(dialogId, dialogName, [
        { type: 'datepicker', id: 'DepositDate', label: '経費日付', options: {
			varidate:{type:'str'},
            value:new Date().toLocaleDateString('sv-SE')
		}},
		{ type: 'number', id: 'DepositNo', label: '経費番号', options: {
			disabled:true,
            lock:'経費番号',
		}},
		{ type: 'input-table', id: 'inputtable', label: '', options: {
            row:99,
            t_heads:tableColumns,
			lineNumber:true,
        }},
    ],
    [
		{ type: 'button_inline', id: 'select', label: '選択', options: {
            icon:'select',
			onclick:`openDialog_for_searchDialog('ExpenseEntrySearch','ExpenseEntry_DepositNo',1000,'経費選択画面');`
        }},
		{ type: 'button_inline', id: 'delete', label: '削除', options: {
            icon:'cancel',
            hidden:true,
            onclick:`ExpenseEntry_deleteClick('${category}','${dialogId}','${dialogName}','削除')`
        }},
		{ type: 'button_inline', id: 'clear', label: '中止', options: {
            icon:'cancel',
            onclick:`ExpenseEntry_cancelClick()`
        }},
        { type: 'button_inline', id: 'output', label: '保存', options: {
            icon:'disk',
            onclick:`ExpenseEntry_saveClick('${category}','${dialogId}','${dialogName}','保存')`
        }},
    ]);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,1000,'px',function(){
		$('#ExpenseEntry_delete').css('display','none');
		$('#ExpenseEntry').prev().children('.ui-dialog-title').text('経費入力 -- 登録');
		$('#ExpenseEntry thead th').eq(5).css('width','20%');
		$('#ExpenseEntry thead th').eq(6).css('width','20%');
	})})
}


$(document).on('click','#ExpenseEntry tbody .ui-icon-search',function(){

	// 押された検索ボタンが何列目かを取得　1:科目検索 2:補助科目検索 4:担当者検索
	columnIdx = $(this).parent().index();

	let id;
	let name;
	switch(columnIdx){
		case 1:
			id = 'subjectSearch';
			name = '科目検索';
			break;
		case 2:
			id = 'auxiliarySubjectSearch';
			name = '補助科目検索';
			if($(this).parent().prev().children().val() == ""){
				alert('科目CDを入力してください');
				return;
			}
			$('#auxiliarySubjectSearch_subjectCD').val($(this).parent().prev().children().val());
			$('#auxiliarySubjectSearch_subjectName').val($(this).parent().prev().children().eq(3).val());
			break;
		case 4:
			id = 'personSearch';
			name = '担当者検索';
			break;
		case 6:
			id = 'summariesSearch';
			name = '科目摘要検索';
			let category = $(this).parent().prevAll().eq(4).children().eq(0).val();
			if(category == ""){
				alert('科目CDを入力してください');
				return;
			}
			$('#summariesSearch_categoryCD').val(category);
			break;
	}
	$(`#${id} td`).remove();
    openDialog_for_searchDialog(id,$(this).prev().attr('id'),'750px',name)

})


// 経費番号ルックアップ
$(document).on('blur','#ExpenseEntry_DepositNo',async function(){

	try{

        const DepositNo = $('#ExpenseEntry_DepositNo').val();

		if(DepositNo == ""){
            return;
        }        

		const query1 = `
			SELECT * FROM TD経費 
			WHERE 経費番号 = ${DepositNo}
		`;

		try{
            var res1 = await fetchSql(query1);
            res1 = res1.results;

        }catch(e){
            return;
        }

		if(res1.length != 0){
            $('#ExpenseEntry_DepositDate').val(new Date(res1[0].経費日付).toLocaleDateString('sv-SE'));
            // データロック
            var Lockres = await LockData("経費番号", SpcToNull(DepositNo, 0))
            if (!Lockres){
                $('#ExpenseEntry_DepositNo').val("");
                return;
            }
        }

		const query2 = `
			SELECT * FROM TD経費明細 
			WHERE 経費番号 = ${DepositNo} 
			order by 枝番
		`;

		try{
            var res2 = await fetchSql(query2);
            res2 = res2.results;

        }catch(e){
            return;
        }

        if(res2.length != 0){
			for(i = 0;i < res2.length; i++){
				$(`#ExpenseEntry_inputtable_${i}_0_From`).val(res2[i].科目CD);
				$(`#ExpenseEntry_inputtable_${i}_0_To`).val(res2[i].科目名);
				$(`#ExpenseEntry_inputtable_${i}_1_From`).val(res2[i].補助CD);
				$(`#ExpenseEntry_inputtable_${i}_2_From`).val(res2[i].金額);
				$(`#ExpenseEntry_inputtable_${i}_2_To`).val(res2[i].消費税額);
				$(`#ExpenseEntry_inputtable_${i}_3_From`).val(res2[i].担当者CD);
				$(`#ExpenseEntry_inputtable_${i}_3_To`).val(res2[i].担当者名);
				$(`#ExpenseEntry_inputtable_${i}_4 input`).val(res2[i].購入先名);
				$(`#ExpenseEntry_inputtable_${i}_5_From`).val(res2[i].科目摘要CD);
				$(`#ExpenseEntry_inputtable_${i}_5_To`).val(res2[i].科目摘要名);

                if(res2[i].補助CD != 0){

                    const query3 = `
                        SELECT 補助科目名 FROM TM補助科目 
                        WHERE 科目CD = ${res2[i].科目CD} 
                        AND 補助CD = ${res2[i].補助CD} 
                    `;

                    try{
                        var res3 = await fetchSql(query3);
                        res3 = res3.results;
            
                    }catch(e){
                        return;
                    }

                    if(res3.length == 1){
                        $(`#ExpenseEntry_inputtable_${i}_1_To`).val(res3[0].補助科目名);
                    }
                }else{
                    $(`#ExpenseEntry_inputtable_${i}_1_From`).val("");
                    $(`#ExpenseEntry_inputtable_${i}_1_To`).val("");
                }
			}
        }

		$('#ExpenseEntry_delete').css('display','inline-block');
		$('#ExpenseEntry').prev().children('.ui-dialog-title').text('経費入力 -- 修正');

        

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }

})

// 登録処理
async function ExpenseEntry_saveClick(category,dialogId,dialogName,btnLabel){
    try{
		// ヘッダー部のみチェック
        if(await ExpenseEntry_itemCheck()){
            return;
        }

		const res = await ExpenseEntry_uploadCheck();
        if(res[0]){
            return;
		}

        if(!confirm('保存します。')){
            return;
        }

        if($(`#ExpenseEntry_DepositNo`).val() == ""){
            // 新規
            modeF = 1
        }else{
            // 修正
            modeF = 2
        }

        const item_params = {
            modeF:modeF,
			DepositDate:$(`#ExpenseEntry_DepositDate`).val().replaceAll('-','/'),
            DepositNo:$(`#ExpenseEntry_DepositNo`).val(),
			input_tableData:res[1]
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        // 初期化
        ExpenseEntry_initialize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

async function ExpenseEntry_itemCheck(){

    try{

        const DepositDate = $('#ExpenseEntry_DepositDate')

        let resFlg = true;

		// 経費日付
        if(!DepositDate.val()){
            alert('経費日付が未入力です。');
            return resFlg;
        }

		return false

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}



async function ExpenseEntry_uploadCheck(){
    try{
		const resFlg = true;
		const tableData = [];

		const DepositDate = $('#ExpenseEntry_DepositDate')
		
		// 月次更新日チェック
		const cls_dates = new clsDates('買掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate = cls_dates.updateDate;
        if(new Date(DepositDate.val()) <= gGetuDate){
            alert('更新済みの為、保存できません。');
            DepositDate.focus();
            return [resFlg];
        }

		// 入力最終行チェック
		let row_count = 0
		const length = $('#ExpenseEntry_inputtable_inputTable tbody tr').length;
		for(i = 0; i < length; i++){
			const kamokuCD = $(`#ExpenseEntry_inputtable_${i}_0_From`)
			if(kamokuCD.val() == ""){
				break
			}
			row_count += 1
		}

		if(row_count == 0){
			alert('明細が未入力です')
			return [resFlg];
		}

		// 配列内・確定行値チェック
		for(i = 0; i < row_count; i++){

			const kamokuCD = $(`#ExpenseEntry_inputtable_${i}_0_From`)
			const kamokuName = $(`#ExpenseEntry_inputtable_${i}_0_To`)
			const hozyoCD = $(`#ExpenseEntry_inputtable_${i}_1_From`)
			const kingaku =$(`#ExpenseEntry_inputtable_${i}_2_From`)
			const syohizei = $(`#ExpenseEntry_inputtable_${i}_2_To`)
			const tantosyaCD = $(`#ExpenseEntry_inputtable_${i}_3_From`)
			const tantosyaName = $(`#ExpenseEntry_inputtable_${i}_3_To`)
			const kounyusyaName = $(`#ExpenseEntry_inputtable_${i}_4 input`)
			const tekiyoCD = $(`#ExpenseEntry_inputtable_${i}_5_From`)
			const tekiyoName = $(`#ExpenseEntry_inputtable_${i}_5_To`)

			if(kamokuCD.val() == ""){
				alert(`科目CD(${i+1}行目)を入力して下さい。`);
				kamokuCD.focus();
				return [resFlg];
			}

			if(!hozyoCD.val() == ""){
				const query = `
					SELECT *
					FROM TM補助科目 
					WHERE 科目CD = ${kamokuCD.val()} 
					AND 補助CD = ${hozyoCD.val()}
				`;

				try{
					var res = await fetchSql(query);
					res = res.results;
		
				}catch(e){
					return [resFlg];
				}
		
				if(res.length == 0){
					alert(`指定の補助科目(${i+1}行目)は存在しません。`);
					hozyoCD.focus();
					return [resFlg];
				}

			}

			if(kingaku.val() == "" || kingaku.val() == 0){
				alert(`金額(${i+1}行目)を入力して下さい。`);
				kingaku.focus();
				return [resFlg];
			}

			// 担当者
			if(!tantosyaCD.val() == ""){
				const query = `
					SELECT 担当者名 FROM TM担当者
					WHERE 担当者CD = ${SpcToNull(tantosyaCD.val())}
				`;

				try{
					var res = await fetchSql(query);
					res = res.results;
		
				}catch(e){
					return [resFlg];
				}
		
				if(res.length == 0){
					alert(`指定の担当者(${i+1}行目)は存在しません。`);
					tantosyaCD.focus();
					return [resFlg];
				}
			}

			// 科目摘要
			if(!tekiyoCD.val() == ""){
				const query = `
					SELECT 摘要名 FROM TM科目摘要
					WHERE 科目CD = ${SpcToNull(kamokuCD.val())}
					AND 摘要CD = ${SpcToNull(tekiyoCD.val())}
				`;

				try{
					var res = await fetchSql(query);
					res = res.results;
		
				}catch(e){
					return [resFlg];
				}
		
				if(res.length == 0){
					alert(`指定の科目摘要(${i+1}行目)は存在しません。`);
					tekiyoCD.focus();
					return [resFlg];
				}
			}

            // バイト数チェック
            if(!byte_check(`購入先${i+1}行目`,kounyusyaName.val(),30)){
                kounyusyaName.focus();
                return [resFlg];
            }
            if(!byte_check(`摘要${i+1}行目`,tekiyoName.val(),30)){
                tekiyoName.focus();
                return [resFlg];
            }

			tableData.push({
                kamokuCD:kamokuCD.val(),
                kamokuName:kamokuName.val(),
                hozyoCD:SpcToNull(hozyoCD.val(),0),
                kingaku:Number(SpcToNull(kingaku.val().replaceAll(',',''),0)),
				syohizei:Number(null_to_zero(syohizei.val().replaceAll(',',''))),
				tantosyaCD:SpcToNull(tantosyaCD.val()),
				tantosyaName:SpcToNull(tantosyaName.val()),
				kounyusyaName:SpcToNull(kounyusyaName.val()),
				tekiyoCD:SpcToNull(tekiyoCD.val()),
				tekiyoName:SpcToNull(tekiyoName.val())

            });
		}

		return [false,tableData];

		

	}catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

// 削除処理
async function ExpenseEntry_deleteClick(category,dialogId,dialogName,btnLabel){
    try{

		const DepositDate = $('#ExpenseEntry_DepositDate')
		
		// 月次更新日チェック
		const cls_dates = new clsDates('買掛月次更新日');
        await cls_dates.GetbyID();
        gGetuDate = cls_dates.updateDate;
        if(new Date(DepositDate.val()) <= gGetuDate){
            alert('更新済みの為、保存できません。');
            DepositDate.focus();
            return resFlg;
        }

        if(!confirm('削除します。')){
            return;
        }

        const item_params = {
            DepositNo:$(`#ExpenseEntry_DepositNo`).val()
        }

        const param = {
            "category": category,
            "title": dialogName,
            "button": btnLabel,
            "user": $p.userName(),
            "opentime": dialog_openTime,
            "params":item_params
        };

        try{
            await download_report(param);
        }catch{
            return;
        }

        // 初期化
        ExpenseEntry_initialize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}

// クリア関数
async function ExpenseEntry_initialize(){
    // ロックの解除
    UnLockData("経費番号", SpcToNull($(`#ExpenseEntry_DepositNo`).val(), 0))

    // 項目のクリア

	$(`#ExpenseEntry input`).val('')
	$(`#ExpenseEntry_DepositDate`).val(new Date().toLocaleDateString('sv-SE'))


    $('#ExpenseEntry_delete').css('display','none');

    $('#ExpenseEntry').prev().children('.ui-dialog-title').text('経費入力 -- 登録');

    $(`#ExpenseEntry_DepositDate`).focus();
}

// 中止処理
async function ExpenseEntry_cancelClick(){
    try{
        if(!confirm('現在の編集内容を破棄します。')){
            return;
        }

        // 初期化
        ExpenseEntry_initialize()

    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
}


$(document).on('click',`#ExpenseEntry_select`, function(){
    ExpenseEntry_initialize()
})

// 科目CDが入力されてないときは他の項目を入力させない
$(document).on('focus',`.ExpenseEntry_inputtable_1, .ExpenseEntry_inputtable_2, .ExpenseEntry_inputtable_2_to, .ExpenseEntry_inputtable_3, .ExpenseEntry_inputtable_4, .ExpenseEntry_inputtable_5, .ExpenseEntry_inputtable_5_to`,async function(){
	if ($(this).closest('tr').find('.ExpenseEntry_inputtable_0').val() == ""){
		alert(`科目CDを入力して下さい。`);
		$(this).closest('tr').find('.ExpenseEntry_inputtable_0').focus();
	}
})

// 科目
$(document).on('blur','.ExpenseEntry_inputtable_0',async function(){
    try{
        if(now_IME)return;
        const element = $(this);
        const value = $(this).val();
        const row_initialize = (e) =>{
            e.closest('tr').find('input').val('');
        }

        if(element.val() == ""){
            row_initialize(element);
            return;
        }
        const cls_subject = new ClsSubject(element.val());
        const res = await cls_subject.GetByData();

        if(!res){
            row_initialize(element);
            return;
        }
        element.nextAll('input').val(cls_subject["科目名"]);
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})
// // 補助科目
$(document).on('blur','.ExpenseEntry_inputtable_1',async function(){
    try{
        
        if(now_IME)return;
        const element = $(this);
        const value = $(this).val();
        const idx = $(this).closest('tr').index();
        const kamokuCD = $(`#ExpenseEntry_inputtable_${idx}_0_From`).val();
        const hojoKamokuCD = element.val();
        if(kamokuCD == ""){
            element.nextAll('input').val("");
            return;
        }
        if(hojoKamokuCD == ""){
            element.nextAll('input').val("");
            return;
        }

        const cls_supportSubject = new ClsSupportSubject(kamokuCD,hojoKamokuCD);
        const res = await cls_supportSubject.GetByData();

        if(!res){
            element.nextAll('input').val("");
            return;
        }
        element.nextAll('input').val(cls_supportSubject["補助科目名"]);
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})
let money_first_flg = false
$(document).on('blur','.ExpenseEntry_inputtable_2',async function(){
    if(money_first_flg == true)return;
    money_first_flg = true
    try{
        if($(this).closest('tr').find('.ExpenseEntry_inputtable_0').val() == "")return;
        const element = $(this);
        if(element.val() == "0" || element.val() == ''){
            alert('金額が未入力です。');
            element.val('');

            element.focus();
            await sleep(0);
        }
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }finally{
        money_first_flg = false
    }
})
$(document).on('blur','.ExpenseEntry_inputtable_3',async function(){
    try{
        
        if(now_IME)return;
        if($(this).closest('tr').find('.ExpenseEntry_inputtable_0').val() == "" || $(this).val() == "")return;
        const element = $(this);
        const value = element.val();
        
        const cls_person = new ClsPerson(value);
        const res = await cls_person.GetByData();

        if(!res){
            alert('指定の担当者は存在しません。');
            element.val('');
            element.nextAll('input').val('');
            return;
        }

        element.nextAll('input').val(cls_person["担当者名"]);
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})
// 摘要
$(document).on('blur','.ExpenseEntry_inputtable_5',async function(){
    try{
        
        if(now_IME)return;
        const element = $(this);
        const value = element.val();
        const idx = element.closest('tr').index();
        const kamokuCD = $(`#ExpenseEntry_inputtable_${idx}_0_From`);
        if(kamokuCD.val() == ""){
            // alert('科目CDを入力して下さい。');
            // element.val('');
            // kamokuCD.focus();
            return;
        }
        if(value == ""){
            return;
        }
        const query = `SELECT 摘要名 FROM TM科目摘要 WHERE 科目CD = ${kamokuCD.val()} AND 摘要CD = ${value}`;
        let res = await fetchSql(query);
        res = res.results;
        if(res.length != 1){
            alert('指定の摘要コードは存在しません');
            element.val('');
            return;
        }
        element.nextAll('input').val(res[0].摘要名);
    }catch(e){
        alert('予期せぬエラーが発生しました。');
        console.error(e);
        return;
    }
})

