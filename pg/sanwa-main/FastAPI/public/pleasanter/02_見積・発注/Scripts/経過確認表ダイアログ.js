{
    // 経過確認票
    let dialogId = "progressCheck";
    let dialogName = "経過確認表出力";
    let category = "見積・発注";
    let btnLabel = "出力";
    createAndAddDialog(dialogId, dialogName,[
        { type: 'text-set', id: 'processingCategory', label: '処理種別', options: {
            newLine:true,
            valueFrom:'0',
            type:"number",
            values:[
                {value:'0',text:'未処理'},
                {value:'1',text:'発注済'},
                {value:'2',text:'売上済'},
                {value:'3',text:'全て'}
            ],
            width:'wide'
        }},
        { type: 'text', id: 'custmerCD', label: '得意先CD',forColumnName:'得意先CD', options: {
            digitsNum:4,
            searchDialog:{id:'custmerNoSearch',title:'得意先CD検索'}
        }},
        { type: 'text', id: 'deliveryCD', label: '納入先CD',forColumnName:'納入先CD', options: {
            digitsNum:4,
            searchDialog:{id:'recipientNoSearch',title:'納入先CD検索'}
        }},
        { type: 'text', id: 'personCD', label: '担当者CD',forColumnName:'担当者CD', options: {
            digitsNum:2,
            searchDialog:{id:'personSearch',title:'担当者検索',multiple:false},
        }},
        // { type: 'text', id: 'adjustmentMoney', label: '調整金額', options: {
        // }},
        { type: 'text-set', id: 'industryCategory', label: '業種区分',forColumnName:'業種区分',options: {
            type:"number",
            width:'wide',
            values:[
                {value:'0',text:'什器'},
                {value:'1',text:'内装'},
            ]
        }},
        { type: 'text-set', id: 'objectType', label: '物件種別',forColumnName:'物件種別',options: {
            type:"number",
            width:'wide',
            values:[
                {value:'0',text:'新店'},
                {value:'1',text:'改装'},
                {value:'2',text:'メンテ'},
                {value:'6',text:'委託'},
            ],
        }},
        { type: 'range-text', id: 'estimateNo', label: '見積番号', forColumnName:'MT.見積番号',options: {
            newLine:true,
            digitsNum:6,
            width:'wide',
            searchDialog:{id:'estimateNoSearch5',title:'見積番号検索',multiple:false}
        }},
        { type: 'text', id: 'estimateName', label: '見積件名',forColumnName:'見積件名', options: {
            width:'wide'
        }},
        { type: 'range-text', id: 'estimatedAmount', label: '見積金額',forColumnName:'MT.合計金額 + MT.出精値引' ,options: {
            alignment:'end',
            width:'wide'
        }},
        { type: 'range-text', id: 'objectNo', label: '物件No', forColumnName:'物件番号',options: {
            width:'wide'
        }},
        { type: 'text-set', id: 'WLeaseClassification', label: 'Wリース区分',forColumnName:'ウエルシアリース区分', options: {
            type:"number",
            width:'wide',
            values:[
                {value:'1',text:'通常'},
                {value:'2',text:'リース'},
            ]
        }},
        { type: 'text-set', id: 'WRequestJurisdiction', label: 'W請求管轄', forColumnName:'MT.ウエルシア物件区分CD',options: {
            searchDialog:{id:'WPropertyClassificationSearch',title:'ウエルシア物件区分検索'},
            lookupOrigin:{tableId:'TMウエルシア物件区分',keyColumnName:'ウエルシア物件区分CD',forColumnName:'ウエルシア物件区分名'},
            digitsNum:1,
            width:'wide',
        }},
        { type: 'range-date', id: 'estimateDates', label: '見積日',forColumnName:'見積日付', options: {
            newLine:true,
        }},
        { type: 'range-date', id: 'startedApointDates', label: '開始納期',forColumnName:'納期S', options: {
        }},
        { type: 'range-date', id: 'stockingDates', label: '仕入日',forColumnName:'仕入日付', options: {
        }},
        { type: 'range-date', id: 'complateDates', label: '完了日',forColumnName:'完了日付', options: {
        }},
        { type: 'range-date', id: 'scheduledBillingDates', label: '請求予定日',forColumnName:'請求予定日', options: {
        }},
        { type: 'text-set', id: 'confirmationEstimateDates', label: '見積確定', options: {
            type:"number",
            width:'wide',
            values:[
                {value:'0',text:'未確定'},
                {value:'1',text:'確定'}
            ]
        }},
        { type: 'search-table', id: 'searchTable', label: '', options: {
            individualSql:true,
            newLine:false,
            multiple:true,
            allCheck:true,
            row:15,
            t_heads:[
                {label:'見積番号',ColumnName:'見積番号'},
                {label:'見積日付',ColumnName:'見積日付'},
                {label:'種別',ColumnName:'物件種別'},
                {label:'得意先',ColumnName:'得意先CD'},
                {label:'納入先',ColumnName:'納入先CD'},
                {label:'見積件名',ColumnName:'見積件名'},
                {label:'見積金額',ColumnName:'合計金額',alignment:'end'},
                {label:'Wリース区分',ColumnName:'ウエルシアリース区分'},
                {label:'W物件区分',ColumnName:'ウエルシア物件区分名'},
                {label:'YK管轄',ColumnName:'YKサプライ区分'},
                {label:'YK物件',ColumnName:'YK物件区分'},
                {label:'YK請求',ColumnName:'YK請求区分'},
                {label:'B管轄',ColumnName:'B請求管轄区分'},
                {label:'BtoB番号',ColumnName:'BtoB番号'},
                {label:'出力日',ColumnName:'見積書出力日',},
                {label:'発注日',ColumnName:'発注書発行日付'},
                {label:'開始納期',ColumnName:'納期S'},
                {label:'仕入日',ColumnName:'仕入日付'},
                {label:'未仕入行数',ColumnName:'未仕入件数',alignment:'end'},
                {label:'売上日',ColumnName:'売上日付'},
                {label:'完了日',ColumnName:'完了日付'},
                {label:'請求予定',ColumnName:'請求予定日'},
                {label:'経過備考1',ColumnName:'経過備考1'},
                {label:'経過備考2',ColumnName:'経過備考2'},
                {label:'請求書',ColumnName:'請求書発行日付'},
                {label:'原価率',ColumnName:'原価率',alignment:'end'},
                {label:'更新日付',ColumnName:'登録変更日'},
                {label:'物件番号',ColumnName:'物件番号'},
                {label:'見積確定',ColumnName:'見積確定区分'},
            ]
        }},
    ],
    [
        { type: 'button_inline', id: 'create', label: '出力', options: {
            icon:'disk',
            onclick: `progressCheck_report('${dialogId}','${category}','${dialogName}','${btnLabel}');`,
        }},
    ]
);
    // ダイアログを開くボタンを追加
    commonModifyLink(dialogName, function() {openDialog(dialogId,'95','%',function(){
        $('#progressCheck table').css({
            "width":'max-content',
            'table-layout':'auto',
            'min-width':'100%'
        });
        $('#progressCheck th').css('padding','0 5px');
    })});
    // useIndividualSqlDialogs.push(dialogId);
}

// 納入先CD検索ダイアログを開く際に得意先情報を検索ダイアログに登録
$(document).on('click','#progressCheck_deliveryCDField .ui-icon-search',async function(e){
    let custmerCD = $('#progressCheck_custmerCD').val();
    
    const cls_custmer = new ClsCustmer(custmerCD);
    const res = await cls_custmer.GetByData();

    if(!res){
        alert('得意先CDを入力して下さい');
        $('#progressCheck_custmerCD').focus();
        return;
    }

    openDialog_for_searchDialog('recipientNoSearch','progressCheck_deliveryCD','750px','納入先検索');

    $('#recipientNoSearch_custmerFrom').val(cls_custmer["custmerCD"]);
    $('#recipientNoSearch_custmerTo').val(cls_custmer["得意先名1"]);
})


const progressCheck_baseSQL = e =>{
    return `
        SELECT TOP 1000
            MT.見積番号, MT.見積日付, CASE MT.物件種別 WHEN 0 THEN '新店' WHEN 1 THEN '改装' WHEN 2 THEN 'ﾒﾝﾃ' WHEN 3 THEN '補充' WHEN 4 THEN '内装' WHEN 5 THEN 'その他' WHEN 6 THEN '委託' END as 物件種別, MT.得意先CD, MT.納入先CD, MT.見積件名
            ,CASE WHEN MT.合計金額 + MT.出精値引 = 0 THEN '0' ELSE FORMAT(MT.合計金額 + MT.出精値引, '#,###') END AS 合計金額
            ,CASE MT.ウエルシアリース区分 WHEN 1 THEN '通常' WHEN 2 THEN 'リース' ELSE '' END as ウエルシアリース区分
            ,MT.ウエルシア物件区分CD,WB.ウエルシア物件区分名
            ,CASE MT.YKサプライ区分 WHEN 0 THEN '' ELSE CAST(MT.YKサプライ区分 AS VARCHAR) END as YKサプライ区分
            ,CASE MT.YK物件区分 WHEN 0 THEN '' ELSE CAST(MT.YK物件区分 AS VARCHAR) END as YK物件区分
            ,CASE MT.YK請求区分 WHEN 0 THEN '' ELSE CAST(MT.YK請求区分 AS VARCHAR) END as YK請求区分
            ,CASE MT.B請求管轄区分 WHEN 0 THEN '' ELSE CAST(MT.B請求管轄区分 AS VARCHAR) END as B請求管轄区分
            ,CASE MT.BtoB番号 WHEN 0 THEN '' ELSE CAST(MT.BtoB番号 AS VARCHAR) END as BtoB番号
            ,MT.見積書出力日,MHK.発注書発行日付
            ,MT.納期S
            ,MT.仕入日付
            ,未仕入件数 = ISNULL(CNT.件数,0)
            ,MT.売上日付
            ,MHK.完了日付,MHK.請求予定日,MHK.経過備考1,MHK.経過備考2
            ,USH.請求書発行日付
            ,MT.原価率
            ,MT.登録変更日
            ,物件番号
            ,見積確定区分 = ISNULL(MHK.見積確定区分,0)
        FROM TD見積 AS MT
            LEFT JOIN TD売上請求H AS USH
                ON MT.見積番号 = USH.見積番号
            LEFT JOIN TD見積_経過 AS MHK
                ON MT.見積番号 = MHK.見積番号
            LEFT JOIN TMウエルシア物件区分 AS WB
                ON MT.ウエルシア物件区分CD = WB.ウエルシア物件区分CD
            LEFT JOIN (
                    SELECT 見積番号,件数 = COUNT(見積明細連番)
                        FROM (
                            SELECT  MT.見積番号, MT.見積明細連番
                                FROM    TD見積シートM AS MT
                                    LEFT JOIN
                                        (SELECT   見積明細連番, SUM(仕入数量) AS 仕入数量
                                            From TD仕入明細内訳
                                            GROUP BY    見積明細連番
                                        ) AS SRU
                                        ON MT.見積明細連番 = SRU.見積明細連番
                                WHERE (MT.発注数 - IsNull(SRU.仕入数量, 0) <> 0)
                            ) AS DATA_A
                        GROUP BY 見積番号
                    ) AS CNT
                    ON MT.見積番号 = CNT.見積番号
        ${e}
        ORDER BY MT.得意先CD,MT.見積日付 DESC, MT.見積番号 DESC
    `
}

/**
 * 経過確認表出力の処理
 */
async function progressCheck_report(dialogId, category, dialogName, btnLabel) {
    try {
        // バリデーションチェック
        if (!validateDialog(dialogId)) {
            return;
        }

        console.log("start : 経過確認表出力");

        // チェックされた行のデータを取得
        const item_params = getCheckedRowsFromProgressCheck(); // 配列を取得

        // チェックされた行がない場合、警告ダイアログを表示して処理を中止
        if (item_params.length === 0) {
            alert("対象がチェックされていません。");
            return;
        }

        // item_params を所定の形式に変換
        let item_params_transformed = {};
        item_params.forEach((row, index) => {
            item_params_transformed[`data${index + 1}`] = row;
        });

        console.log("データ取得完了", item_params_transformed);

        // パラメータ設定
        const param = {
            category,
            title: dialogName,
            button: btnLabel,
            user: $p.userName(),
            opentime: dialog_openTime,
            params: item_params_transformed, // 変換後のオブジェクトを送信
        };

        // ファイル名の定義
        let dt = new Date().toLocaleDateString().replaceAll("/", "").slice(2); // 日付を整形
        let filename =
            "経過確認表出力" +
            "-" +
            SpcToNull($("#progressCheck_estimateName").val()) +
            dt;

        // API通信でレポートをダウンロード
        try{
            await download_report(param,"経過確認表");
        }catch{
            return;
        }

        console.log("end : 経過確認表出力");
    } catch (err) {
        // エラーハンドリング
        alert("予期せぬエラーが発生しました。");
        console.error(dialogName + "エラー", err);
    }
}

/**
 * チェックされた行のデータを取得する処理
 * @returns {Array} チェックされた行のオブジェクト配列
 */
function getCheckedRowsFromProgressCheck() {
    console.log("データ取得開始");

    const table = $("#progressCheck .table.searchTable"); // 対象テーブルを選択
    const checkedRows = []; // チェックされた行のデータを格納する配列

    // テーブルのヘッダーを取得（thの内容）
    const headers = table
        .find("thead th")
        .map(function () {
            return $(this).text().trim();
        })
        .get(); // `.get()`で配列に変換

    console.log("ヘッダーの取得:", headers);

    // tbody内の各行を処理
    table.find("tbody tr").each(function () {
        const row = $(this); // 行の要素を取得
        const isChecked = row.find('input[type="checkbox"]').is(":checked"); // チェックボックスの状態を確認

        // チェックされた行のみ処理
        if (isChecked) {
            const rowObj = {}; // 行のデータを格納するオブジェクト

            // 各列（td）のデータをヘッダーに基づいて格納
            row.find("td").each(function (i) {
                if (headers[i]) {
                    // ヘッダーが存在する場合のみデータを追加
                    rowObj[headers[i]] = $(this).text().trim();
                }
            });

            // チェックされた行を配列に追加
            checkedRows.push(rowObj);
        }
    });

    console.log("チェックされた行のデータ取得完了", checkedRows);
    return checkedRows; // 配列を返す
}