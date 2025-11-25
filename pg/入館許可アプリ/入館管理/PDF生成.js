/**
 * QRコード付きPDF生成機能（日本語フォント対応）
 * 必須ライブラリ: jsPDF
 *
 * @module PDF生成
 * @description
 * このモジュールは入館証PDFの生成機能を提供します。
 * QRコードを含むPDFを生成し、日本語テキストも適切に表示します。
 */

// ====================
// メイン関数（ユーザーが主に使用）
// ====================

/**
 * PDFを生成
 *
 * @param {Object} [options={}] - PDF生成オプション
 * @param {boolean} [options.includeQRCode=false] - QRコードを含めるかどうか
 * @param {string} [options.qrText] - QRコードに埋め込むテキスト（includeQRCodeがtrueの場合必須）
 * @param {boolean} [options.returnBase64=false] - trueの場合、Base64文字列を返す。falseの場合、ファイルをダウンロード
 * @param {string|number} [options.uploadToRecord] - 指定した場合、このレコードIDにPleasanterにアップロード
 *
 * @returns {Promise<string|Object>} returnBase64がtrueの場合はBase64文字列、uploadToRecord指定時はPleasanterレスポンス、それ以外はファイル名
 *
 * @example
 * // シンプルPDFをダウンロード
 * await generatePDF(visitorData);
 *
 * @example
 * // QRコード付きPDFをダウンロード
 * await generatePDF(visitorData, { includeQRCode: true, qrText: qrUrl });
 *
 * @example
 * // PDFをBase64として取得
 * const base64 = await generatePDF(visitorData, { returnBase64: true });
 *
 * @example
 * // PDFを生成してPleasanterにアップロード
 * const result = await generatePDF(visitorData, {
 *   includeQRCode: true,
 *   qrText: qrUrl,
 *   uploadToRecord: recordId
 * });
 */
async function generatePDF(options = {}) {
    // オプションのデフォルト値
    const config = {
        includeQRCode: options.includeQRCode === true,
        qrText: options.qrText,
        returnBase64: options.returnBase64 === true,
        uploadToRecord: options.uploadToRecord
    };

    // QRコードが必要な場合のバリデーション
    if (config.includeQRCode && !config.qrText) {
        throw new Error('QRコードを含める場合はqrTextオプションが必須です');
    }

    // Pleasanterにアップロードする場合
    if (config.uploadToRecord) {
        return createQRCodePDFAndUpload(
            config.includeQRCode ? config.qrText : null,
            config.uploadToRecord,
            { includeQRCode: config.includeQRCode }
        );
    }

    // === 以下、統合されたPDF生成処理 ===

    try {
        const qrText = config.includeQRCode ? config.qrText : null;

        // デバッグログ
        console.log('generatePDF - qrText:', qrText, 'options:', options);

        // jsPDFライブラリの可用性をチェック、必要に応じて動的読み込み
        if (!checkJsPDFAvailability()) {
            console.log('jsPDF not found, attempting dynamic loading...');
            await loadJsPDFLibrary();

            // 再度チェック
            if (!checkJsPDFAvailability()) {
                throw new Error('jsPDF library could not be loaded. Please check your internet connection and CDN access.');
            }
        }

        commonMessage('PDF生成中...', 'success');

        let qrBase64 = null;

        // QRコードが必要な場合のみ生成
        if (qrText) {
            qrBase64 = await generateQRCodeAsBase64(qrText, {
                width: 256,
                height: 256,
                padding: 10
            });

            // デバッグ：QRコードの生成結果を確認
            console.log('QR Base64 length:', qrBase64 ? qrBase64.length : 'null');
            console.log('QR Base64 prefix:', qrBase64 ? qrBase64.substring(0, 50) : 'null');
        }

        // PDFインスタンスを作成（複数のアクセス方法を試行）
        let jsPDF;
        if (window.jspdf && window.jspdf.jsPDF) {
            jsPDF = window.jspdf.jsPDF;
        } else if (window.jsPDF) {
            jsPDF = window.jsPDF;
        } else if (window.jspdf) {
            jsPDF = window.jspdf;
        } else {
            throw new Error('jsPDF library is not loaded');
        }

        console.log('jsPDF found:', typeof jsPDF);
        const pdf = new jsPDF(PDF_CONFIG.orientation, PDF_CONFIG.unit, PDF_CONFIG.format);

        // jsPDFのネイティブ機能でPDFに直接描画
        try {
            // 日本語フォント設定
            await setupJapanesePDFFont(pdf);

            // システム設定から値を取得
            const pdfConfig = SYSTEM_CONFIG.PDF;
            const texts = SYSTEM_CONFIG.PDF.TEXT;
            const values = SYSTEM_CONFIG.PDF.TEXT.VALUES;

            // 色設定
            pdf.setTextColor(pdfConfig.COLORS.text);

            // ========== 上半分：ヘッダー部分 ==========
            let yPos = 30;
            const qrSize = pdfConfig.DESIGN.qrSize;

            // QRコードがある場合のみ配置
            if (qrBase64) {
                // 左上：QRコード配置
                pdf.addImage(`data:image/png;base64,${qrBase64}`, 'PNG', pdfConfig.MARGIN.left, yPos, qrSize, qrSize);
            }

            // 右寄り：受付番号（ラベルと値を別々に描画）
            const receptionX = 150; // 大幅に右寄りに変更

            // ラベル部分（大きくして太字に）
            const labelFontSize = pdfConfig.FONT_SIZE.label * 1.8; // さらに大きく
            await drawJapaneseText(pdf, texts.LABELS.RECEPTION, receptionX, yPos + 10, {
                fontSize: labelFontSize,
                color: pdfConfig.COLORS.secondary,
                fontWeight: 'bold',
                align: 'left'
            });

            // ラベルのアンダーライン（短く調整）
            pdf.setDrawColor(pdfConfig.COLORS.secondary);
            pdf.setLineWidth(0.5);
            const labelWidth = texts.LABELS.RECEPTION.length * (labelFontSize * 0.35); // 幅を短く
            pdf.line(receptionX, yPos + 14, receptionX + labelWidth, yPos + 14);

            // 受付番号の値（ラベルのすぐ下に配置）
            const receptionNumber = values.RECEPTION_ID;
            const numberFontSize = pdfConfig.FONT_SIZE.header * 2.5; // より大きく（2.5倍）
            await drawJapaneseText(pdf, receptionNumber, receptionX, yPos + 25, { // ラベルのすぐ下
                fontSize: numberFontSize,
                color: pdfConfig.COLORS.primary,
                fontWeight: 'bold',
                align: 'left'
            });

            // 値のアンダーライン（太く、短く）
            if (receptionNumber && receptionNumber.length > 0) {
                pdf.setDrawColor(pdfConfig.COLORS.primary);
                pdf.setLineWidth(1.2); // より太いライン
                const underlineY = yPos + 32; // 位置も調整
                const textWidth = receptionNumber.length * (numberFontSize * 0.35); // 幅を短く調整
                if (textWidth > 0) {
                    pdf.line(receptionX, underlineY, receptionX + textWidth, underlineY);
                }
            }

            // QRコードの下にスペースを設定（QRコードがない場合は調整）
            if (qrBase64) {
                yPos = yPos + qrSize + pdfConfig.DESIGN.sectionGap;
            } else {
                yPos = yPos + 25; // QRコードがない場合のスペース（縮小）
            }

            // 固定文言：承り通知
            await drawJapaneseText(pdf, texts.MESSAGES.CONFIRMATION, pdfConfig.MARGIN.left, yPos, {
                fontSize: pdfConfig.FONT_SIZE.normal,
                color: pdfConfig.COLORS.text
            });

            yPos += pdfConfig.DESIGN.sectionGap;

            // ========== 下半分：来訪情報（箇条書き） ==========
            const infoItems = [
                { label: texts.LABELS.DATE, value: values.VISIT_DATETIME_WITH_END || '' },
                { label: texts.LABELS.CUSTOMER, value: values.REP_NAME || '' },
                { label: texts.LABELS.LOCATION, value: `${SYSTEM_CONFIG.COMPANY_INFO.NAME}\n${SYSTEM_CONFIG.COMPANY_INFO.ADDRESS}\n${SYSTEM_CONFIG.COMPANY_INFO.BUILDING_NAME}` },
                { label: texts.LABELS.MEETING_ROOM, value: values.MEETING_ROOM || '' },
                { label: texts.LABELS.PURPOSE, value: values.PURPOSE || '' }
            ];

            // 箇条書きで情報を描画（ラベルと値をスペース区切りで）
            for (const item of infoItems) {
                if (item.value || item.label === texts.LABELS.DATE || item.label === texts.LABELS.CUSTOMER || item.label === texts.LABELS.LOCATION) { // 必須項目または値がある場合のみ
                    if (item.label === texts.LABELS.LOCATION) {
                        // 場所は複数行表示
                        await drawJapaneseText(pdf, `■ ${item.label}  :`, pdfConfig.MARGIN.left, yPos, {
                            fontSize: pdfConfig.FONT_SIZE.label,
                            color: pdfConfig.COLORS.text
                        });
                        yPos += pdfConfig.DESIGN.lineHeight;
                        
                        const locationLines = item.value.split('\n');
                        for (const line of locationLines) {
                            await drawJapaneseText(pdf, line, pdfConfig.MARGIN.left + 10, yPos, {
                                fontSize: pdfConfig.FONT_SIZE.label,
                                color: pdfConfig.COLORS.text
                            });
                            yPos += pdfConfig.DESIGN.lineHeight;
                        }
                    } else {
                        // ラベルと値をスペース区切りで同一行に
                        const itemText = `■ ${item.label}  :  ${item.value}`;
                        await drawJapaneseText(pdf, itemText, pdfConfig.MARGIN.left, yPos, {
                            fontSize: pdfConfig.FONT_SIZE.label,
                            color: pdfConfig.COLORS.text
                        });
                        yPos += pdfConfig.DESIGN.lineHeight;
                    }
                }
            }



            yPos += pdfConfig.DESIGN.sectionGap;

            // ========== 入館方法セクション ==========
            await drawJapaneseText(pdf, `■ ${texts.LABELS.ENTRANCE_METHOD}`, pdfConfig.MARGIN.left, yPos, {
                fontSize: pdfConfig.FONT_SIZE.label,
                color: pdfConfig.COLORS.primary
            });
            yPos += pdfConfig.DESIGN.lineHeight;

            // 日本語説明
            await drawJapaneseText(pdf, texts.MESSAGES.ENTRANCE_JA, pdfConfig.MARGIN.left + 10, yPos, {
                fontSize: pdfConfig.FONT_SIZE.normal,
                color: pdfConfig.COLORS.text
            });
            yPos += pdfConfig.DESIGN.lineHeight;

            // 英語説明
            await drawJapaneseText(pdf, texts.MESSAGES.ENTRANCE_EN, pdfConfig.MARGIN.left + 10, yPos, {
                fontSize: pdfConfig.FONT_SIZE.normal,
                color: pdfConfig.COLORS.text
            });
            yPos += pdfConfig.DESIGN.sectionGap;

            // ========== 注意事項セクション ==========
            await drawJapaneseText(pdf, `■ ${texts.LABELS.NOTICE}`, pdfConfig.MARGIN.left, yPos, {
                fontSize: pdfConfig.FONT_SIZE.label,
                color: pdfConfig.COLORS.primary
            });
            yPos += pdfConfig.DESIGN.lineHeight;

            // 注意事項リスト
            const noticeItems = [
                texts.MESSAGES.NOTICE_1,
                texts.MESSAGES.NOTICE_2,
                texts.MESSAGES.NOTICE_3,
                texts.MESSAGES.NOTICE_4,
                texts.MESSAGES.NOTICE_5
            ];

            for (const notice of noticeItems) {
                await drawJapaneseText(pdf, notice, pdfConfig.MARGIN.left + 5, yPos, {
                    fontSize: pdfConfig.FONT_SIZE.notice,
                    color: pdfConfig.COLORS.text
                });
                yPos += 6; // 注意事項は狭い行間
            }

            // ========== 最下部：締めの挨拶 ==========
            yPos = 285; // より下に配置
            await drawJapaneseText(pdf, texts.MESSAGES.CLOSING, 105, yPos, {
                fontSize: pdfConfig.FONT_SIZE.normal,
                color: pdfConfig.COLORS.primary,
                align: 'center'
            });

            console.log('Direct PDF generation completed');

            // デバッグログ追加
            console.log('Debug - config object:', config);
            console.log('Debug - config.returnBase64:', config.returnBase64, 'type:', typeof config.returnBase64);
            console.log('Debug - original options.returnBase64:', options.returnBase64, 'type:', typeof options.returnBase64);

            if (config.returnBase64) {
                return pdf.output('datauristring').split(',')[1];
            } else {
                const filename = generatePDFFilename(values.RECEPTION_ID);
                pdf.save(filename);
                return filename;
            }
        } catch (error) {
            throw error;
        }

    } catch (error) {
        commonMessage(`PDF生成エラー: ${error.message}`, 'error');
        console.error('PDF Generation Error:', error);
        throw error;
    }
}

// ====================
// サポート関数
// ====================

/**
 * QRコードとPDFを生成してPleasanterに保存
 *
 * @param {string|null} text - QRコードに埋め込むテキスト（nullの場合QRコードなし）
 * @param {string|number} targetID - 保存先のレコードID
 * @param {Object} [options={}] - オプション
 * @param {boolean} [options.includeQRCode=true] - QRコードをPDFに含めるかどうか
 * @returns {Promise<Object>} Pleasanter APIのレスポンス
 *
 * @example
 * // QRコード付きPDFを生成（デフォルト）
 * const result = await createQRCodePDFAndUpload(
 *   'https://example.com/qr?id=123',
 *   '123',
 *   visitorData
 * );
 *
 * @example
 * // QRコードなしのPDFを生成
 * const result = await createQRCodePDFAndUpload(
 *   null,
 *   '123',
 *   visitorData,
 *   { includeQRCode: false }
 * );
 */
async function createQRCodePDFAndUpload(text, targetID, options = {}) {
    try {
        // オプションのデフォルト値
        const config = {
            includeQRCode: options.includeQRCode !== false // デフォルトはtrue
        };

        // デバッグログ
        console.log('createQRCodePDFAndUpload - text:', text, 'targetID:', targetID, 'options:', options);

        // QRコードの有無に応じたメッセージ
        const message = config.includeQRCode ? 'QRコード付きPDF生成中...' : 'PDF生成中...';
        commonMessage(message, 'success');

        // PDFをBase64として生成
        const pdfBase64 = await generatePDF({
            includeQRCode: config.includeQRCode,
            qrText: text,
            returnBase64: true
        });

        // ファイル名を生成
        const filename = generatePDFFilename(targetID);

        commonMessage('Pleasanterにアップロード中...', 'success');

        // PDFをAttachmentsAに添付
        const data = {
            "ApiVersion": 1.1,
            "AttachmentsHash": {
                "AttachmentsA": [
                    {
                        "ContentType": "application/pdf",
                        "Name": filename,
                        "Base64": pdfBase64
                    }
                ]
            }
        };

        const result = await $.ajax({
            type: "POST",
            url: `/api/items/${targetID}/update`,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            scriptCharset: 'utf-8'
        });

        commonMessage(`PDF登録完了: ${filename}`, 'success');
        console.log('PDF登録完了:', result);

        return result;

    } catch (error) {
        commonMessage(`PDF処理エラー: ${error.message}`, 'error');
        console.error('PDF処理エラー:', error);
        throw error;
    }
}

// PDF生成の設定をシステム設定から取得
const PDF_CONFIG = {
    orientation: SYSTEM_CONFIG.PDF.PAGE.orientation,
    unit: SYSTEM_CONFIG.PDF.PAGE.unit,
    format: SYSTEM_CONFIG.PDF.PAGE.format,
    fontSize: SYSTEM_CONFIG.PDF.FONT_SIZE,
    margin: SYSTEM_CONFIG.PDF.MARGIN,
    colors: SYSTEM_CONFIG.PDF.COLORS
};

// jsPDFライブラリの読み込み状態をチェック
function checkJsPDFAvailability() {
    console.log('=== jsPDF Availability Check ===');
    console.log('window.jspdf:', typeof window.jspdf);
    console.log('window.jsPDF:', typeof window.jsPDF);
    console.log('window.jspdf.jsPDF:', window.jspdf ? typeof window.jspdf.jsPDF : 'N/A');

    if (window.jspdf) {
        console.log('window.jspdf keys:', Object.keys(window.jspdf));
    }

    return !!(window.jspdf || window.jsPDF);
}

// jsPDFライブラリを動的に読み込む
async function loadJsPDFLibrary() {
    return new Promise((resolve, reject) => {
        // すでに読み込まれている場合
        if (window.jspdf || window.jsPDF) {
            resolve(true);
            return;
        }

        console.log('Dynamically loading jsPDF library...');

        const script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js';
        script.onload = () => {
            console.log('jsPDF library loaded successfully');
            // 読み込み後少し待機
            setTimeout(() => {
                if (window.jspdf || window.jsPDF) {
                    resolve(true);
                } else {
                    reject(new Error('jsPDF library loaded but not accessible'));
                }
            }, 200);
        };
        script.onerror = () => {
            console.error('Failed to load jsPDF from CDN, trying alternative...');
            // 代替CDNを試行
            const altScript = document.createElement('script');
            altScript.src = 'https://unpkg.com/jspdf@2.5.1/dist/jspdf.umd.min.js';
            altScript.onload = () => {
                console.log('jsPDF library loaded from alternative CDN');
                setTimeout(() => {
                    if (window.jspdf || window.jsPDF) {
                        resolve(true);
                    } else {
                        reject(new Error('jsPDF library loaded but not accessible'));
                    }
                }, 200);
            };
            altScript.onerror = () => {
                console.error('Failed to load jsPDF from alternative CDN, trying older version...');
                // さらに古いバージョンを試行
                const legacyScript = document.createElement('script');
                legacyScript.src = 'https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.4.0/jspdf.umd.min.js';
                legacyScript.onload = () => {
                    console.log('jsPDF legacy version loaded');
                    setTimeout(() => {
                        if (window.jspdf || window.jsPDF) {
                            resolve(true);
                        } else {
                            reject(new Error('jsPDF library loaded but not accessible'));
                        }
                    }, 200);
                };
                legacyScript.onerror = () => {
                    reject(new Error('Failed to load jsPDF library from all CDNs and versions'));
                };
                document.head.appendChild(legacyScript);
            };
            document.head.appendChild(altScript);
        };
        document.head.appendChild(script);
    });
}

// 日本語フォント設定関数
async function setupJapanesePDFFont(pdf) {
    try {
        // jsPDF-AutoTableを利用した日本語フォント対応
        // Canvasでテキストを画像化してPDFに追加する手法
        return true;
    } catch (error) {
        console.error('Japanese font setup error:', error);
        // フォールバック：標準フォント
        pdf.setFont('helvetica', 'normal');
        return false;
    }
}

// 日本語文字列をPDFで表示可能な形式に変換
function convertJapaneseForPDF(text) {
    if (!text) return '';

    try {
        // jsPDFが対応できるように文字列を処理
        return text.toString();
    } catch (error) {
        console.error('Japanese text conversion error:', error);
        return text;
    }
}

// 日本語テキストをcanvasで画像化してPDFに描画
async function drawJapaneseText(pdf, text, x, y, options = {}) {
    try {
        // Canvasで日本語テキストを画像として描画
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');

        // フォントサイズを取得（デフォルト14）
        const fontSize = options.fontSize || 14;
        const devicePixelRatio = window.devicePixelRatio || 1;
        const scaledFontSize = fontSize * devicePixelRatio;

        // Canvas設定（高解像度対応）
        const fontWeight = options.fontWeight || 'normal';
        ctx.font = `${fontWeight} ${scaledFontSize}px "Noto Sans JP", "Hiragino Sans", "Yu Gothic", "Meiryo", sans-serif`;
        ctx.fillStyle = options.color || '#000000';
        ctx.textAlign = options.align || 'left';

        // テキストサイズを測定
        const metrics = ctx.measureText(text);
        const textWidth = metrics.width / devicePixelRatio;
        const textHeight = fontSize * 1.2; // 行間を考慮

        // Canvasサイズを設定（高解像度対応）
        canvas.width = (textWidth + 20) * devicePixelRatio; // 余白
        canvas.height = (textHeight + 10) * devicePixelRatio; // 余白
        canvas.style.width = (textWidth + 20) + 'px';
        canvas.style.height = (textHeight + 10) + 'px';

        // 背景を透明に
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        // フォント設定を再適用（Canvasサイズ変更でリセットされるため）
        ctx.scale(devicePixelRatio, devicePixelRatio);
        ctx.font = `${fontWeight} ${fontSize}px "Noto Sans JP", "Hiragino Sans", "Yu Gothic", "Meiryo", sans-serif`;
        ctx.fillStyle = options.color || '#000000';
        ctx.textAlign = 'left';
        ctx.textBaseline = 'alphabetic';

        // アンチエイリアシング設定
        ctx.imageSmoothingEnabled = true;
        ctx.imageSmoothingQuality = 'high';

        // テキストを描画
        ctx.fillText(text, 10, fontSize + 5);

        // CanvasをPNGとしてPDFに追加
        const imageData = canvas.toDataURL('image/png');

        // PDF座標系での適切なサイズ計算（アスペクト比を保持）
        const scaleFactor = 0.75; // ピクセルからmm変換
        const imgWidth = (textWidth + 20) * scaleFactor / 3.78; // 3.78 = 96dpi/25.4mm
        const imgHeight = (textHeight + 10) * scaleFactor / 3.78;

        // アライメント調整
        let finalX = x;
        if (options.align === 'center') {
            finalX = x - (imgWidth / 2);
        } else if (options.align === 'right') {
            finalX = x - imgWidth;
        }

        pdf.addImage(imageData, 'PNG', finalX, y - (imgHeight * 0.8), imgWidth, imgHeight);

    } catch (error) {
        console.error('Japanese text drawing error:', error);
        // フォールバック：UTF-8エンコーディングで直接描画を試行
        try {
            pdf.text(text, x, y, options);
        } catch (fallbackError) {
            console.error('Fallback text drawing also failed:', fallbackError);
            // 最終フォールバック：問号で置換
            const fallbackText = text.replace(/[^\x00-\x7F]/g, '?');
            pdf.text(fallbackText, x, y, options);
        }
    }
}

// 不要になった変換関数（削除済み）

// PDFファイル名を生成する共通関数
function generatePDFFilename(recordId) {
    const yyyymmdd = commonGetDate(undefined, undefined, undefined, 'YYYYMMDD');
    return `entrypass_${yyyymmdd}_${recordId}.pdf`;
}


/**
 * メール送信用にPDFを添付ファイルとして準備
 *
 * @param {Object} data - PDFに含める来訪者データ
 * @param {string} qrText - QRコードに埋め込むテキスト
 * @returns {Promise<Object>} 添付ファイルオブジェクト
 * @returns {string} returns.Name - ファイル名
 * @returns {string} returns.Base64 - Base64エンコードされたPDFデータ
 * @returns {string} returns.ContentType - MIMEタイプ（'application/pdf'）
 *
 * @example
 * const attachment = await preparePDFAttachment(visitorData, qrUrl);
 * // { Name: "entrypass_20241210_123.pdf", Base64: "...", ContentType: "application/pdf" }
 */
async function preparePDFAttachment(qrText) {
    try {
        // PDFをBase64として生成
        const pdfBase64 = await generatePDF({
            includeQRCode: !!qrText,
            qrText: qrText,
            returnBase64: true
        });

        // 添付ファイル形式で返す
        return {
            Name: generatePDFFilename(SYSTEM_CONFIG.PDF.TEXT.VALUES.RECEPTION_ID),
            Base64: pdfBase64,
            ContentType: 'application/pdf'
        };

    } catch (error) {
        console.error('PDF Attachment Error:', error);
        throw error;
    }
}

/**
 * 複数の来訪者のPDFを一括生成
 *
 * @param {Array<Object>} dataList - 生成対象のデータリスト
 * @param {string} dataList[].qrText - QRコードテキスト
 * @param {string} dataList[].targetId - 保存先レコードID
 * @param {Object} dataList[].visitorData - 来訪者データ
 * @returns {Promise<Array>} 処理結果の配列
 *
 * @example
 * const results = await batchCreatePDFs([
 *   { qrText: 'url1', targetId: '1', visitorData: {...} },
 *   { qrText: 'url2', targetId: '2', visitorData: {...} }
 * ]);
 */
async function batchCreatePDFs(dataList) {
    const results = [];

    for (const data of dataList) {
        try {
            await commonCheckPoint(['PDF生成中...', `${data.visitorData.代表者氏名}を処理中...`], 'progress');

            const result = await createQRCodePDFAndUpload(
                data.qrText,
                data.targetId
            );

            results.push({
                success: true,
                targetId: data.targetId,
                visitorName: data.visitorData.代表者氏名,
                result: result
            });

        } catch (error) {
            results.push({
                success: false,
                targetId: data.targetId,
                visitorName: data.visitorData.代表者氏名,
                error: error.message
            });
        }
    }

    return results;
}