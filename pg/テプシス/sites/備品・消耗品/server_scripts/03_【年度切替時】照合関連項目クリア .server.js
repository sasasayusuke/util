// 備品・消耗品テーブルのすべてのレコードから以下の項目を取得し、空白で上書きする。
// 現品照合実施者(ClassI)
// 現品照合結果(ClassK)
// 現品照合コメント(ClassG)
// 現品照合日(DateD)
// 照合承認者(ClassH)
// 現品照合承認日(DateB)


// =================== 備品・消耗品の全レコード取得 ===================
let offset = 0;         // 取得開始位置
let totalCount = 0;     // 総取得件数
const allItems = [];    // 取得したレコードを格納する配列

const timeout = 600 * 1000;
const startTime = new Date().getTime();


try {
    
    while (true) {

        check_Timeotut();  // タイムアウトチェック関数呼び出し

        const data = {
            Offset: offset,
        };

        context.Log("備品・消耗品全レコード取得開始");

        try {
            const results = Array.from(items.Get(BIHIN_SHOMOHIN_SITE_ID, JSON.stringify(data)));
            // const results = Array.from(items.Get(252479, JSON.stringify(data)));
            // const results = items.Get(site, JSON.stringify(data));

            // 取得件数が 0 になったら終了
            if (!results || results.length === 0) {
                break;
            }

            // 今回分を配列に追加
            for (const item of results) {
                allItems.push(item);
            }

            // ログ出力
            context.Log(`備品・消耗品レコード取得状況：Offset=${offset}, 取得件数=${results.length}`);
            totalCount += results.length;

            // Offset を「今回取得した件数ぶん」進める
            offset += results.length;

        } catch (error) {
            context.Log(`items.Get でエラーが発生しました: ${error.message}`);
            context.Log(`Offset=${offset} の時点でエラー`);
            context.Log(`スタックトレース: ${error.stack}`);

            // エラー発生時の処理を選択
            throw error;

        }
    }
    context.Log(`備品・消耗品テーブルレコード取得成功、最終的な取得件数: ${totalCount} 件`);


    // ===================== クリア処理用リクエストデータ =====================
    const data = {
        "ClassHash": {
            "ClassI": "",
            "ClassK": "",
            "ClassG": "",
            "ClassH": "",
        },
        "DateHash": {
            "DateD": "1899/12/31",
            "DateB": "1899/12/31"
        }
    }


    //  ==================== 取得した全レコードの照合関連項目クリア処理 ====================
    for (const item of allItems) {

        check_Timeotut();  // タイムアウトチェック関数呼び出し

        // 照合関連項目クリア処理
        const updateResult = items.Update(item.ResultId, JSON.stringify(data));
        context.Log(`照合関連項目クリア処理結果：{レコードID:${item.ResultId}, 成功?: ${updateResult}}`);

    }

} catch (error) {

    context.Log(error.stack);

}


// ================= タイムアウト処理関数 ================= 
function check_Timeotut() {
    const now = new Date().getTime();

    if (timeout < now - startTime) {
        throw new Error('タイムアウトです');
    }
}
