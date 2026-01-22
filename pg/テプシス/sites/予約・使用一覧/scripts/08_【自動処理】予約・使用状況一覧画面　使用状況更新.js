(function($){
    // =====================================
    // トリガー関数
    // =====================================
    // 画面読み込み完了後にオーケストレーション関数を起動する
    document.addEventListener('DOMContentLoaded', function () {
        try {
            window.force && console.log('使用状況バッジ処理開始', { fn: 'sdtIndex' });

            // オーケストレーション起点
            sdtIndex();

        } catch (e) {
            // 想定外エラー
            showCommonError('画面描画処理に失敗しました');
        }
    });

    // =====================================
    // index関数
    // =====================================
    function sdtIndex(){
            // 処理開始
            window.force && console.log('index開始');
            fetchSiteKeyValues(reservationList).then(function (data) {
                    incLoading(); // 表示（count = +1）

                    console.log('次の処理', data);
                    window.force && console.log('使用状況バッジ処理開始', { fn: 'sdtIndex' });

                    // データを分類わけ
                    const grouped = groupReservationStatus(data);

                    // データを整形
                    const reservationNoGroups = extractReservationNumbers(grouped);

                    // アップデート
                    updateAllStatuses(reservationNoGroups);
                    
                    // 一覧適応の為フィルタボタン押下
                    $p.send($('#FilterButton'));
                    
                    decLoading(); // 非表示は count が 0 になった時

            });
    };



    // =====================================================
    // =======
    // 関数名: fetchSiteKeyValues
    // =======
    // 目的:
    //   指定 siteId に対して $p.apiGet を実行し、レスポンスを返す
    //
    // 引数:
    //   - 取得先のサイトID
    //
    // 戻り値:
    //   - 取得した配列
    //
    // 備考:
    //   - 接続エラー時は error() を呼びエラー番号を表示します。
    // =====================================================
    function fetchSiteKeyValues(siteId) {
        window.force && console.log('処理開始', { siteId });

        return new Promise(function (resolve, reject) {
            // オフセット（取得開始位置）
            var offset = 0;
            // 全ページ分を蓄積する配列
            var allData = [];
            // 最初に返ってきたレスポンスオブジェクト（最終返却時にベースとして使う）
            var firstResponse = null;

            // 1ページ分を取得する再帰関数
            function getPage() {
                // $p.apiGet に渡す POST データ（View は元コードと同じ）
                var postData = {
                    View: {
                        ApiDataType: "KeyValues",
                        GridColumns: ["ResultId", "DateC", "DateD", "DateA", "DateB"]
                    },
                    // Offset を指定してページングを行う（公式 FAQ に記載の方法）
                    Offset: offset
                };

                // 実際の API 呼び出し
                $p.apiGet({
                    id: siteId,
                    data: postData,
                    done: function (data) {
                        // data.Response にページング情報とデータが入る想定（公式マニュアル／FAQ）
                        // safety: 必要なプロパティが存在するか最低限チェックする
                        var resp = (data && data.Response) ? data.Response : null;
                        if (!resp) {
                            // 想定外のレスポンスフォーマット
                            showCommonError(500);
                            return reject(500);
                        }

                        // 最初のレスポンスは保持しておく（返却時に形を保つため）
                        if (!firstResponse) {
                            firstResponse = data;
                        }

                        // 今回取得したデータ配列（存在チェック）
                        var pageData = Array.isArray(resp.Data) ? resp.Data : [];
                        // 取得分を結合
                        Array.prototype.push.apply(allData, pageData);

                        // ページサイズ（サーバが返す PageSize を尊重）
                        var pageSize = (typeof resp.PageSize === 'number' && resp.PageSize > 0)
                            ? resp.PageSize
                            : pageData.length; // フォールバック

                        // 全件数（サーバが返す TotalCount を使用）
                        var totalCount = (typeof resp.TotalCount === 'number') ? resp.TotalCount : allData.length;

                        window.force && console.log('ページ取得', { offset: offset, pageSize: pageSize, totalCount: totalCount, got: pageData.length });

                        // 次ページが必要か判定：
                        // 「開始位置 + 一度に取得できる件数」が全件より小さい場合、続きがある。
                        if (offset + pageSize < totalCount) {
                            // 次ページの開始位置をセットして再帰で取得
                            offset += pageSize;
                            // 小ブレイクを入れる必要がある場合はここで検討（現在は直列で続ける）
                            getPage();
                        } else {
                            // 全件取得完了 — 最初のレスポンスオブジェクトをベースに Response.Data を差し替えて返す
                            if (firstResponse && firstResponse.Response) {
                                firstResponse.Response.Data = allData;
                                firstResponse.Response.Offset = 0;
                                firstResponse.Response.PageSize = allData.length;
                                firstResponse.Response.TotalCount = totalCount;
                                resolve(firstResponse);
                            } else {
                                // 最悪ケース：元の形を保てない場合は配列だけ返す
                                resolve(allData);
                            }
                        }
                    },
                    fail: function (err) {
                        // エラー表示関数を呼び出す（err はエラー番号を想定）
                        showCommonError(err);
                        reject(err);
                    }
                });
            }

            // 取得開始
            getPage();
        });
    }

    // ＝＝＝＝＝＝＝＝＝
    // groupReservationStatus（日時を日付＋時刻で比較する版）
    // ＝＝＝＝＝＝＝＝＝
    /**
     * 予約データを5グループに分類する（日時は日付＋時刻で厳密比較）
     * @param {Object} apiResponse - $p.apiGet のレスポンス
     * @returns {Object} groupedResult
     */
    function groupReservationStatus(apiResponse) {
        window.force && console.log('処理開始', { apiResponse });

        try {
            // 念のためデータ存在チェック
            if (!apiResponse || !apiResponse.Response || !Array.isArray(apiResponse.Response.Data)) {
                showCommonError('E-DATA-INVALID');
                return null;
            }

            const dataList = apiResponse.Response.Data;

            // 現在の日時（ミリ秒）
            const now = Date.now();

            // グループ初期化
            const result = {
                usingOverdue: [], // ① 使用中（期限超過）
                using: [],        // ② 使用中
                scheduled: [],    // ③ 予定期間中（未使用）
                returned: [],     // ④ 返却済み
                empty: []         // ⑤ 空白（未来予定 / 予定ありで未使用 / 全フィールド空白）
            };

            // 空白判定ユーティリティ（null/undefined/空文字/半角スペースのみ を空白とみなす）
            function isBlank(v) {
                if (v === null || v === undefined) return true;
                if (typeof v !== 'string') return false;
                return v.trim() === '';
            }

            // 日時パースユーティリティ（安全に年月日時分秒を抜いて Date オブジェクトを返す）
            // 対応例: "2025/12/07 20:05", "2025-12-07 20:05:30", "2025/12/07", "2025-12-07T20:05"
            function parseDateTime(raw) {
                if (isBlank(raw)) return null;
                var s = String(raw).trim();

                // 正規表現で分解して new Date(year, month-1, day, hour, min, sec)
                var re = s.match(/^(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})(?:[ T](\d{1,2}):(\d{1,2})(?::(\d{1,2}))?)?$/);
                if (!re) {
                    // フォールバック： Date に任せる（例えば既に ISO 形式の場合等）
                    var d = new Date(s);
                    return isNaN(d.getTime()) ? null : d;
                }
                var y = parseInt(re[1], 10);
                var m = parseInt(re[2], 10) - 1;
                var dday = parseInt(re[3], 10);
                var hh = re[4] !== undefined ? parseInt(re[4], 10) : 0;
                var mm = re[5] !== undefined ? parseInt(re[5], 10) : 0;
                var ss = re[6] !== undefined ? parseInt(re[6], 10) : 0;
                return new Date(y, m, dday, hh, mm, ss);
            }

            dataList.forEach(function (item) {
                // 生データ文字列（空白判定用）
                const plannedStartRaw = item["予定使用日時"];
                const plannedEndRaw   = item["予定返却日時"];
                const usedAtRaw       = item["使用日時"];
                const returnedAtRaw   = item["返却日時"];

                // 日付（時刻含む）変換（存在する文字列のみ）
                const plannedStart = parseDateTime(plannedStartRaw);
                const plannedEnd   = parseDateTime(plannedEndRaw);
                const usedAt       = parseDateTime(usedAtRaw);
                const returnedAt   = parseDateTime(returnedAtRaw);

                // ミリ秒化（存在するもののみ）
                const plannedStartTime = plannedStart ? plannedStart.getTime() : null;
                const plannedEndTime   = plannedEnd   ? plannedEnd.getTime()   : null;
                const usedAtTime       = usedAt       ? usedAt.getTime()       : null;
                const returnedAtTime   = returnedAt   ? returnedAt.getTime()   : null;

                // ----------------------------
                // 優先判定（重要）
                // 1) 返却日時が入力されている → ④返却済み（厳格）
                // 2) 使用中（期限超過）
                // 3) 使用中
                // 4) 予定期間中（本日が期間内で未使用）
                // 5) 空白（未来予定 OR 予定ありで未使用 OR 全フィールド空白）
                // ----------------------------

                // 1) 返却済み：返却日時が入力されている場合のみ
                if (returnedAtTime !== null) {
                    result.returned.push(item);
                    return;
                }

                // 2) 使用中（期限超過）
                if (
                    plannedStartTime !== null && plannedEndTime !== null &&
                    usedAtTime !== null &&
                    returnedAtTime === null &&
                    plannedEndTime < now
                ) {
                    result.usingOverdue.push(item);
                    return;
                }

                // 3) 使用中（使用日時が登録され、返却未入力）
                if (
                    usedAtTime !== null &&
                    returnedAtTime === null
                ) {
                    result.using.push(item);
                    return;
                }

                // 4) 予定期間中（未使用）：予定があり、未使用（使用/返却未入力）、かつ現在が期間内（時刻含む）
                if (
                    plannedStartTime !== null && plannedEndTime !== null &&
                    usedAtTime === null && returnedAtTime === null &&
                    plannedStartTime <= now && now <= plannedEndTime
                ) {
                    result.scheduled.push(item);
                    return;
                }

                // 5) 空白カテゴリ判定（優先度最後）
                const allBlank = isBlank(plannedStartRaw) && isBlank(plannedEndRaw) && isBlank(usedAtRaw) && isBlank(returnedAtRaw);
                const futurePlanned = plannedStartTime !== null && now < plannedStartTime;
                const plannedButNotUsed = (plannedStartTime !== null && plannedEndTime !== null) && usedAtTime === null && returnedAtTime === null;

                if (allBlank || futurePlanned || plannedButNotUsed) {
                    result.empty.push(item);
                    return;
                }

                // フォールバック（上のいずれにも当てはまらない場合は空白に入れる）
                result.empty.push(item);
            });

            window.force && console.log('処理終了', result);
            return result;

        } catch (e) {
            showCommonError(e);
            return null;
        }
    }


    // ＝＝＝＝＝＝＝＝＝
    // extractReservationNumbers
    // ＝＝＝＝＝＝＝＝＝
    /**
     * グループ化された予約データから「予約番号」だけを抽出する
     * @param {Object} groupedData - groupReservationStatus の戻り値想定
     * @returns {Object} reservationNumberGroups
     */
    function extractReservationNumbers(groupedData) {
        window.force && console.log('処理開始', { groupedData });

        try {
            // 結果格納用
            const result = {};

            // 各グループをループ
            Object.keys(groupedData).forEach(function (groupKey) {
                const list = groupedData[groupKey];

                // 配列でなければ空配列
                if (!Array.isArray(list)) {
                    result[groupKey] = [];
                    return;
                }

                // 「予約番号」だけを抽出
                result[groupKey] = list
                    .map(function (item) {
                        return item["予約番号"];
                    })
                    .filter(function (val) {
                        // 念のため null / undefined を除外
                        return val !== null && val !== undefined;
                    });
            });

            window.force && console.log('処理終了', result);
            return result;

        } catch (e) {
            showCommonError(e);
            return null;
        }
    }


    /**
     * 指定されたID配列を元に ClassD を更新する
     * @param {number[]} idList - 更新対象のレコードID配列
     * @param {string} classDValue - ClassD に設定する値
     * @returns {Promise<void>}
     */
    function updateClassDByIds(idList, classDValue) {
        return new Promise(function (resolve) {
            // 対象が0件の場合は即完了
            if (!idList || idList.length === 0) {
                resolve();
                return;
            }

            let completed = 0;

            idList.forEach(function (recordId) {
                $p.apiUpdate({
                    id: recordId,
                    data: {
                        ClassHash: {
                            ClassD: classDValue
                        }
                    },
                    done: function () {
                        completed++;
                        if (completed === idList.length) {
                            resolve();
                        }
                    },
                    fail: function (data) {
                        // エラー番号があればそれを使用
                        const errNo = data && data.ErrorNo;
                        showCommonError(errNo);

                        completed++;
                        if (completed === idList.length) {
                            resolve();
                        }
                    }
                });
            });
        });
    }


    function updateAllStatuses(statusMap) {
        window.force && console.log('処理開始', { statusMap });

        Promise.all([
            updateClassDByIds(statusMap.usingOverdue, '使用中（期限超過）'),
            updateClassDByIds(statusMap.using, '使用中'),
            updateClassDByIds(statusMap.scheduled, '予定期間中（未使用）'),
            updateClassDByIds(statusMap.returned, '返却済'),
            updateClassDByIds(statusMap.empty, '') // 空白カテゴリは ClassD を空文字に設定
        ]).then(function () {
            // alert('完了しました');
            window.force && console.log('全更新完了');
        });
    }

})(jQuery);