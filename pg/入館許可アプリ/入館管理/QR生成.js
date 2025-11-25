/**
 * QRコード生成モジュール
 * 
 * @module QR生成
 * @description
 * QRコードを生成し、PNGまたはJPEG形式でBase64エンコードしたり、
 * Pleasanterにアップロードしたりする機能を提供します。
 * 
 * 必須: QRCode.jsライブラリ
 */

// ====================
// メイン関数（ユーザーが主に使用）
// ====================

/**
 * QRコードを生成してPleasanterにアップロード
 * 
 * @param {string} text - QRコードに埋め込むテキスト
 * @param {string|number} targetID - アップロード先のレコードID
 * @param {Object} options - QRコード生成オプション（generateQRCodeAsBase64と同じ）
 * @param {string} [options.attachmentField='AttachmentsA'] - 添付先の項目名
 * @returns {Promise<Object>} Pleasanter APIのレスポンス
 * 
 * @example
 * // デフォルト設定でQRコードをアップロード
 * await createQRCodeAndUpload('https://example.com', '123');
 * 
 * @example
 * // カスタム設定で別の項目にアップロード
 * await createQRCodeAndUpload('https://example.com', '123', {
 *   width: 512,
 *   format: 'jpg',
 *   attachmentField: 'AttachmentsB'
 * });
 */
async function createQRCodeAndUpload(text, targetID, options = {}) {
    try {
        commonMessage('QRコード生成中...', 'success');

        // QRコードをBase64形式で生成
        const base64 = await generateQRCodeAsBase64(text, options);

        // ファイル名を生成（タイムスタンプ付き）
        const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
        const format = options.format || 'png';
        const extension = format === 'jpg' || format === 'jpeg' ? 'jpg' : 'png';
        const filename = `qrcode_${timestamp}.${extension}`;

        commonMessage('Pleasanterにアップロード中...', 'success');

        // 指定された項目に添付（デフォルト: AttachmentsA）
        const attachmentField = options.attachmentField || 'AttachmentsA';
        const result = await uploadQRCodeToPleasanter(targetID, attachmentField, base64, filename, format);

        commonMessage(`QRコード登録完了: ${filename}`, 'success');
        console.log('登録完了:', result);

        return result;

    } catch (error) {
        commonMessage(`処理エラー: ${error.message}`, 'error');
        console.error('処理エラー:', error);
        throw error;
    }
}

/**
 * 編集画面で現在のレコードにQRコードを添付
 * 
 * @param {string} text - QRコードに埋め込むテキスト
 * @param {Object} options - QRコード生成オプション
 * @returns {Promise<Object>} Pleasanter APIのレスポンス
 * 
 * @example
 * // 現在編集中のレコードにQRコードを添付
 * await attachQRCodeToCurrentRecord('https://example.com/record/123');
 */
async function attachQRCodeToCurrentRecord(text, options = {}) {
    if ($p.action() !== "edit" && $p.action() !== "new") {
        commonMessage('この機能は編集画面でのみ使用できます', 'error');
        return;
    }

    const recordId = $p.id();
    if (!recordId) {
        commonMessage('レコードIDが取得できません', 'error');
        return;
    }

    return createQRCodeAndUpload(text, recordId, options);
}

// ====================
// サポート関数
// ====================

/**
 * QRコードをPNG/JPEGのBase64として生成
 * 
 * @param {string} text - QRコードに埋め込むテキスト
 * @param {Object} options - QRコード生成オプション
 * @param {number} [options.width=256] - QRコードの幅（ピクセル）
 * @param {number} [options.height=256] - QRコードの高さ（ピクセル）
 * @param {string} [options.colorDark='#000000'] - QRコードの暗色部分の色
 * @param {string} [options.colorLight='#ffffff'] - QRコードの明色部分の色
 * @param {number} [options.correctLevel=QRCode.CorrectLevel.M] - 誤り訂正レベル
 * @param {number} [options.padding=20] - 白枠のピクセル数
 * @param {string} [options.format='png'] - 出力形式（'png' または 'jpg'/'jpeg'）
 * @returns {Promise<string>} Base64エンコードされた画像データ
 * 
 * @example
 * // デフォルト設定でQRコード生成
 * const base64 = await generateQRCodeAsBase64('https://example.com');
 * 
 * @example
 * // カスタム設定でQRコード生成
 * const base64 = await generateQRCodeAsBase64('https://example.com', {
 *   width: 512,
 *   height: 512,
 *   colorDark: '#0000FF',
 *   padding: 30,
 *   format: 'jpg'
 * });
 */
function generateQRCodeAsBase64(text, options = {}) {
    return new Promise((resolve, reject) => {
        // デバッグログ
        console.log('QRコード生成開始 - text:', text, 'type:', typeof text);
        
        // textが文字列でない場合はエラー
        if (typeof text !== 'string') {
            reject(new Error(`Type of text must be string. "${text}" (type: ${typeof text}) is not recognized.`));
            return;
        }
        // 一時的なdiv要素を作成
        const tempDiv = document.createElement('div');
        tempDiv.style.display = 'none';
        document.body.appendChild(tempDiv);

        // デフォルトオプション
        const defaultOptions = {
            width: 256,
            height: 256,
            colorDark: '#000000',
            colorLight: '#ffffff',
            correctLevel: QRCode.CorrectLevel.M,
            padding: 20,  // 白枠のピクセル数
            format: 'png'  // 'png' または 'jpg'
        };

        const finalOptions = { ...defaultOptions, ...options };
        const padding = finalOptions.padding;

        // QRコード生成（パディングなしのサイズで）
        const qrcode = new QRCode(tempDiv, {
            text: text,
            width: finalOptions.width,
            height: finalOptions.height,
            colorDark: finalOptions.colorDark,
            colorLight: finalOptions.colorLight,
            correctLevel: finalOptions.correctLevel
        });

        // 画像が生成されるまで待機
        setTimeout(() => {
            const img = tempDiv.querySelector('img');
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');

            const imgObj = new Image();
            imgObj.onload = function() {
                // キャンバスサイズをパディング込みで設定
                const totalWidth = imgObj.width + (padding * 2);
                const totalHeight = imgObj.height + (padding * 2);
                canvas.width = totalWidth;
                canvas.height = totalHeight;

                // 背景を白で塗りつぶす（JPGの場合は必須）
                ctx.fillStyle = '#ffffff';
                ctx.fillRect(0, 0, totalWidth, totalHeight);

                // QRコードを中央に配置
                ctx.drawImage(imgObj, padding, padding);

                // CanvasをBase64に変換
                let dataURL;
                if (finalOptions.format === 'jpg' || finalOptions.format === 'jpeg') {
                    dataURL = canvas.toDataURL('image/jpeg', 0.95); // 品質95%
                    const base64 = dataURL.replace(/^data:image\/jpeg;base64,/, '');
                    document.body.removeChild(tempDiv);
                    resolve(base64);
                } else {
                    dataURL = canvas.toDataURL('image/png');
                const base64 = dataURL.replace(/^data:image\/png;base64,/, '');
                document.body.removeChild(tempDiv);
                resolve(base64);
                }
            };

            imgObj.onerror = function() {
                document.body.removeChild(tempDiv);
                reject(new Error('QRコード画像の生成に失敗しました'));
            };

            imgObj.src = img.src;
        }, 100);
    });
}

/**
 * QRコードをPleasanterにアップロード
 * 
 * @param {string|number} targetID - アップロード先のレコードID
 * @param {string} className - 添付ファイル項目名（例: 'AttachmentsA'）
 * @param {string} base64 - Base64エンコードされた画像データ
 * @param {string} filename - ファイル名
 * @param {string} [format='png'] - 画像形式（'png' または 'jpg'/'jpeg'）
 * @returns {Promise<Object>} Pleasanter APIのレスポンス
 * @private
 */
async function uploadQRCodeToPleasanter(targetID, className, base64, filename, format = 'png') {
    const url = `/api/items/${targetID}/update`;
    const contentType = format === 'jpg' || format === 'jpeg' ? 'image/jpeg' : 'image/png';

    const data = {
        "ApiVersion": 1.1,
        "AttachmentsHash": {
            [className]: [
                {
                    "ContentType": contentType,
                    "Name": filename,
                    "Base64": base64
                }
            ]
        }
    };

    return $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: 'application/json',
        dataType: 'json',
        scriptCharset: 'utf-8'
    });
}

/**
 * 複数のQRコードを一括生成
 * 
 * @param {Array<Object>} dataList - 生成対象のデータリスト
 * @param {string} dataList[].text - QRコードテキスト
 * @param {string} dataList[].targetId - アップロード先のレコードID
 * @param {Object} [dataList[].options] - QRコード生成オプション
 * @returns {Promise<Array>} 処理結果の配列
 * 
 * @example
 * const results = await batchCreateQRCodes([
 *   { text: 'url1', targetId: '1' },
 *   { text: 'url2', targetId: '2', options: { width: 512 } }
 * ]);
 */
async function batchCreateQRCodes(dataList) {
    const results = [];

    for (const data of dataList) {
        try {
            await commonCheckPoint(['QRコード生成中...', `${data.text}を処理中...`], 'progress');

            const result = await createQRCodeAndUpload(
                data.text,
                data.targetId,
                data.options
            );

            results.push({
                success: true,
                targetId: data.targetId,
                text: data.text,
                result: result
            });

        } catch (error) {
            results.push({
                success: false,
                targetId: data.targetId,
                text: data.text,
                error: error.message
            });
        }
    }

    return results;
}
