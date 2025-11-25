// QRコード入館処理システム
// Pleasanterのスクリプト設定に配置

$p.events.on_grid_load = async function () {
    // URLパラメータを取得
    const urlParams = new URLSearchParams(window.location.search);
    const qrFlag = urlParams.get('qr');
    const entranceId = urlParams.get('id');
    const calledFlag = urlParams.get('called');

    // 処理中画面を表示
    showProcessingScreen();
    // QRコードアクセスの処理
    if (qrFlag === '1' && entranceId) {
        // 入館処理実行
        processEntrance(entranceId, calledFlag);
        return
    // パラメータが不正な場合
    } else {
        showEntranceError('invalid_params');
        return
    }
};

// 入館処理メイン関数
async function processEntrance(entranceId, calledFlag) {
    // 既存のページを非表示
    $('#MainContainer').hide();

    // 入館データの検証
    try {
        // 必要な列のみを取得
        const record = await commonGetRecord(entranceId, {
            columns: [
                'IssueId',              // 受付ID
                'Status',               // ステータス
                'ClassA',               // 代表者氏名
                'ClassM',               // 御社名
                'ClassN',               // 弊社担当者
                'ClassP',               // 担当者メールアドレス
                'ClassO',               // 会議室
                'Title',                // ご来訪目的
                'CompletionTime',       // ご来訪日時
            ],
            setLabelText: true,         // カラム名で取得
            setDisplayValue: true,      // 実際の値で取得
        });
        stat = record["状況"];
        // レコードが取得できなかった場合
        if (!record || Object.keys(record).length === 0) {
            showEntranceError('not_found')
            return
        }

        if (calledFlag === '1') {
            const now = commonGetDate(undefined, undefined, undefined, 'YYYY-MM-DDThh:mm:ss');
            await commonUpdateRecord(entranceId, {
                Status: SYSTEM_CONFIG.STATUS.ENTERED.VALUE,  // ステータスを入館済みに更新
                DateA: now, // 入館日時を現在時刻に更新
            })
            // ステータスが300の場合、入館済み画面を表示
            showEntranceSuccess(record, entranceId)
            return
        }

        if (stat === SYSTEM_CONFIG.STATUS.ENTERED.LABEL) {
            showEntranceError('already_entered')
            return
        } else if (stat === SYSTEM_CONFIG.STATUS.REGISTERED.LABEL || stat === SYSTEM_CONFIG.STATUS.CALLING.LABEL) {
            await commonUpdateRecord(entranceId, {
                Status: SYSTEM_CONFIG.STATUS.CALLING.VALUE,  // ステータスを呼び出し中に更新
            })

            // 呼び出し中の場合、担当者にメール送信
            try {
                const staffEmail = record["担当者メールアドレス"];
                const subject = `来訪者お出迎えのお知らせ(受付ID:${entranceId})`
                const body = commonTrimLines(`
                    ${record["弊社担当者"]} 様

                    来訪者がお見えになりました。

                    【来訪者情報】
                    ・お客様名: ${record["代表者氏名"]}
                    ・会社名: ${record["御社名"]}
                    ・来訪日時: ${record["ご来訪日時"]}
                    ・会議室: ${record["会議室"]}
                    ・来訪目的: ${record["ご来訪目的"]}

                    受付までお迎えをお願いいたします。

                    ※このメールは自動送信されています。`
                )

                sendMail({
                        to: staffEmail,
                        subject: subject,
                        body: body,
                });

            } catch (error) {
                console.error('担当者メール送信エラー:', error);
                // メール送信エラーでも処理は継続
            }

            showCallingScreen(record, entranceId)
            return
        } else {
            showEntranceError('not_registered_yet')
            return
        }
    } catch (error) {
        console.error('Validation Error:', error);
        showEntranceError('system_error')
        return
    }
}

// 処理中画面
function showProcessingScreen() {
    const processingHTML = `
        <div id="entrance-container" class="entrance-processing">
            <div class="entrance-card">
                <div class="spinner"></div>
                <h2>入館処理中...</h2>
                <p>しばらくお待ちください</p>
            </div>
        </div>

        <style>
            .entrance-processing {
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
                background: #f5f5f5;
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

            .entrance-card {
                background: white;
                padding: 3rem;
                border-radius: 20px;
                box-shadow: 0 10px 30px rgba(0,0,0,0.1);
                text-align: center;
                min-width: 400px;
            }

            .spinner {
                width: 60px;
                height: 60px;
                border: 4px solid #f3f3f3;
                border-top: 4px solid #3498db;
                border-radius: 50%;
                margin: 0 auto 2rem;
                animation: spin 1s linear infinite;
            }

            @keyframes spin {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
        </style>
    `;

    $('body').append(processingHTML);
}


// 担当者呼び出し中画面
function showCallingScreen(data, entranceId) {
    const callingHTML = `
        <div id="entrance-container" class="entrance-calling">
            <div class="entrance-card calling">
                <div class="calling-icon">
                    <div class="phone-ring">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                            <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"></path>
                        </svg>
                    </div>
                </div>

                <h1>担当者を呼び出し中です</h1>
                <p class="calling-message">しばらくお待ちください</p>

                <div class="visitor-info">
                    <div class="info-row">
                        <span class="label">会社名</span>
                        <span class="value">${data["御社名"]}</span>
                    </div>
                    <div class="info-row">
                        <span class="label">代表者名</span>
                        <span class="value">${data["代表者氏名"]} 様</span>
                    </div>
                    <div class="info-row">
                        <span class="label">受付ID</span>
                        <span class="value">${data["受付ID"]}</span>
                    </div>
                </div>

                <div class="waiting-message">
                    <p>担当者がお迎えに参ります</p>
                    <p>このままお待ちください</p>
                </div>

                <button class="proceed-button" onclick="reload({called: '1'})">
                    入館手続きへ進む
                </button>
            </div>
        </div>

        <style>
            .entrance-calling {
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
                background: linear-gradient(135deg, #ffa500 0%, #ff6347 100%);
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

            .entrance-card.calling {
                background: white;
                padding: 3rem;
                border-radius: 20px;
                box-shadow: 0 20px 40px rgba(0,0,0,0.2);
                text-align: center;
                max-width: 500px;
                width: 90%;
            }

            .calling-icon {
                margin-bottom: 2rem;
            }

            .phone-ring {
                width: 80px;
                height: 80px;
                margin: 0 auto;
                position: relative;
                animation: ring 1.5s ease-in-out infinite;
            }

            .phone-ring svg {
                width: 100%;
                height: 100%;
                color: #ffa500;
            }

            @keyframes ring {
                0%, 100% { transform: rotate(0deg) scale(1); }
                25% { transform: rotate(-10deg) scale(1.05); }
                75% { transform: rotate(10deg) scale(1.05); }
            }

            .entrance-card.calling h1 {
                color: #333;
                margin-bottom: 0.5rem;
                font-size: 2rem;
            }

            .calling-message {
                color: #666;
                font-size: 1.25rem;
                margin-bottom: 2rem;
            }

            .visitor-info {
                background: #f8f9fa;
                border-radius: 10px;
                padding: 1.5rem;
                margin: 2rem 0;
            }

            .info-row {
                display: flex;
                justify-content: space-between;
                padding: 0.75rem 0;
                border-bottom: 1px solid #e9ecef;
            }

            .info-row:last-child {
                border-bottom: none;
            }

            .waiting-message {
                color: #6c757d;
                margin: 2rem 0;
            }

            .waiting-message p {
                margin: 0.5rem 0;
            }

            .proceed-button {
                background: linear-gradient(135deg, #28a745 0%, #218838 100%);
                color: white;
                border: none;
                padding: 1rem 2.5rem;
                border-radius: 30px;
                font-size: 1.1rem;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.4s ease;
                box-shadow: 0 8px 25px rgba(40, 167, 69, 0.3);
            }

            .proceed-button:hover {
                transform: translateY(-3px) scale(1.02);
                box-shadow: 0 12px 35px rgba(40, 167, 69, 0.4);
            }
        </style>
    `;

    $('#entrance-container').remove();
    $('body').append(callingHTML);
}

// 成功画面表示
function showEntranceSuccess(data, now) {
    const successHTML = `
        <div id="entrance-container" class="entrance-success">
            <div class="entrance-card success">
                <div class="check-icon">
                    <svg viewBox="0 0 52 52" class="checkmark">
                        <circle cx="26" cy="26" r="25" fill="none" class="checkmark-circle"/>
                        <path fill="none" d="M14.1 27.2l7.1 7.2 16.7-16.8" class="checkmark-check"/>
                    </svg>
                </div>

                <h1>入館手続き完了</h1>
                <p class="success-message">ようこそ、${data["代表者氏名"]} 様</p>

                <div class="entrance-details">
                    <div class="detail-row">
                        <span class="label">会社名</span>
                        <span class="value">${data["御社名"]}</span>
                    </div>
                    <div class="detail-row">
                        <span class="label">入館時刻</span>
                        <span class="value">${now}</span>
                    </div>
                    <div class="detail-row">
                        <span class="label">受付ID</span>
                        <span class="value gate-number">${data["受付ID"]}</span>
                    </div>
                </div>

                <button class="reception-button" onclick="window.location.href='${SYSTEM_CONFIG.SITE_INFO.RECEPTION.URL}'">受付に戻る</button>

                <div class="redirect-notice">
                    <p>10秒後に自動的に受付画面へ戻ります</p>
                </div>
            </div>
        </div>

        <style>
            .entrance-success {
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

            .entrance-card.success {
                background: white;
                padding: 3rem;
                border-radius: 20px;
                box-shadow: 0 20px 40px rgba(0,0,0,0.2);
                text-align: center;
                max-width: 500px;
                width: 90%;
            }

            .checkmark {
                width: 80px;
                height: 80px;
                border-radius: 50%;
                display: block;
                stroke-width: 2;
                stroke: #4bb71b;
                stroke-miterlimit: 10;
                margin: 0 auto;
                box-shadow: inset 0px 0px 0px #4bb71b;
                animation: fill .4s ease-in-out .4s forwards, scale .3s ease-in-out .9s both;
            }

            .checkmark-circle {
                stroke-dasharray: 166;
                stroke-dashoffset: 166;
                stroke-width: 2;
                stroke-miterlimit: 10;
                stroke: #4bb71b;
                fill: #fff;
                animation: stroke 0.6s cubic-bezier(0.65, 0, 0.45, 1) forwards;
            }

            .checkmark-check {
                transform-origin: 50% 50%;
                stroke-dasharray: 48;
                stroke-dashoffset: 48;
                animation: stroke 0.3s cubic-bezier(0.65, 0, 0.45, 1) 0.8s forwards;
            }

            @keyframes stroke {
                100% {
                    stroke-dashoffset: 0;
                }
            }

            @keyframes scale {
                0%, 100% {
                    transform: none;
                }
                50% {
                    transform: scale3d(1.1, 1.1, 1);
                }
            }

            @keyframes fill {
                100% {
                    box-shadow: inset 0px 0px 0px 30px #4bb71b;
                }
            }

            h1 {
                color: #333;
                margin: 2rem 0 0.5rem;
                font-size: 2rem;
            }

            .success-message {
                color: #666;
                font-size: 1.25rem;
                margin-bottom: 2rem;
            }

            .entrance-details {
                background: #f8f9fa;
                border-radius: 10px;
                padding: 1.5rem;
                margin: 2rem 0;
            }

            .detail-row {
                display: flex;
                justify-content: space-between;
                padding: 0.75rem 0;
                border-bottom: 1px solid #e9ecef;
            }

            .detail-row:last-child {
                border-bottom: none;
            }

            .label {
                color: #6c757d;
                font-weight: 500;
            }

            .value {
                color: #212529;
                font-weight: 600;
            }

            .gate-number {
                color: #007bff;
                font-size: 1.25rem;
            }

            .pass-number {
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                color: white;
                padding: 1.5rem;
                border-radius: 15px;
                margin: 2rem 0;
            }

            .pass-number p {
                margin: 0 0 0.5rem;
                opacity: 0.9;
            }

            .pass-number h2 {
                margin: 0;
                font-size: 2.5rem;
                letter-spacing: 3px;
            }

            .instructions {
                color: #6c757d;
                margin: 1rem 0 2rem;
            }

            .close-button {
                background: #6c757d;
                color: white;
                border: none;
                padding: 0.75rem 2rem;
                border-radius: 25px;
                font-size: 1rem;
                cursor: pointer;
                transition: all 0.3s ease;
            }

            .close-button:hover {
                background: #5a6268;
                transform: translateY(-2px);
            }

            .entrance-success .reception-button {
                background: linear-gradient(135deg, #4bb71b 0%, #38a815 100%);
                color: white;
                border: none;
                padding: 1rem 2.5rem;
                border-radius: 30px;
                font-size: 1.1rem;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94);
                margin-top: 2rem;
                box-shadow: 0 8px 25px rgba(75, 183, 27, 0.3);
                position: relative;
                overflow: hidden;
            }

            .entrance-success .reception-button:before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
                transition: left 0.5s;
            }

            .entrance-success .reception-button:hover {
                transform: translateY(-3px) scale(1.02);
                box-shadow: 0 12px 35px rgba(75, 183, 27, 0.4);
            }

            .entrance-success .reception-button:hover:before {
                left: 100%;
            }

            @media (max-width: 600px) {
                .entrance-card.success {
                    padding: 2rem;
                }

                h1 {
                    font-size: 1.5rem;
                }

                .pass-number h2 {
                    font-size: 2rem;
                }
            }

            .redirect-notice {
                text-align: center;
                color: #6c757d;
                font-size: 0.85rem;
                margin-top: 1.5rem;
                padding: 0.5rem;
                background: rgba(255, 255, 255, 0.1);
                border-radius: 8px;
                opacity: 0.7;
            }
        </style>
    `;

    $('#entrance-container').remove();
    $('body').append(successHTML);

    // 10秒後に受付サイトに遷移
    setTimeout(() => {
        const receptionUrl = SYSTEM_CONFIG.SITE_INFO.RECEPTION.URL;
        window.location.href = receptionUrl;
    }, 10000);
}

// エラー画面表示
function showEntranceError(errorType, message) {
    const errorMessages = {
        'invalid_params': {
            title: 'アクセスエラー',
            message: '無効なQRコードです',
            icon: '❌'
        },
        'invalid_id': {
            title: 'IDエラー',
            message: message || '無効なIDが指定されました',
            icon: '🚫'
        },
        'not_found': {
            title: '予約が見つかりません',
            message: '該当する予約情報が見つかりませんでした',
            icon: '🔍'
        },
        'already_entered': {
            title: '処理済み',
            message: message || '既に入館処理が完了しています',
            icon: '✅'
        },
        'already_entered': {
            title: '処理済み',
            message: message || '既に入館処理が完了しています',
            icon: '✅'
        },
        'not_registered_yet': {
            title: '本登録が完了していません',
            message: 'まだ本登録が完了していません。受付で手続きをお願いします。',
            icon: '⚠️'
        },
        'cancelled': {
            title: 'キャンセル済み',
            message: message || 'この予約はキャンセルされています',
            icon: '🚫'
        },
        'wrong_time': {
            title: '時間エラー',
            message: message || '予約時間を確認してください',
            icon: '⏰'
        },
        'system_error': {
            title: 'システムエラー',
            message: 'システムエラーが発生しました。管理者にお問い合わせください。',
            icon: '⚠️'
        }
    };

    const error = errorMessages[errorType] || errorMessages['system_error'];

    const errorHTML = `
        <div id="entrance-container" class="entrance-error">
            <div class="entrance-card error">
                <div class="error-icon">${error.icon}</div>
                <h1>${error.title}</h1>
                <p class="error-message">${error.message}</p>

                <div class="error-details">
                    <p>エラーコード: ${errorType.toUpperCase()}</p>
                    <p>発生時刻: ${new Date().toLocaleTimeString('ja-JP')}</p>
                </div>



                <div class="support-info">
                    <p>お困りの場合は受付までお越しください</p>
                    <p>TEL: ${SYSTEM_CONFIG.COMPANY_INFO.TEL}</p>
                </div>

                <button class="reception-button" onclick="window.location.href='${SYSTEM_CONFIG.SITE_INFO.RECEPTION.URL}'">受付に戻る</button>

            </div>
        </div>

        <style>
            .entrance-error {
                display: flex;
                justify-content: center;
                align-items: center;
                min-height: 100vh;
                background: #f8d7da;
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

            .entrance-card.error {
                background: white;
                padding: 3rem;
                border-radius: 20px;
                box-shadow: 0 10px 30px rgba(0,0,0,0.1);
                text-align: center;
                max-width: 500px;
                width: 90%;
                border: 2px solid #f5c6cb;
            }

            .error-icon {
                font-size: 4rem;
                margin-bottom: 1rem;
            }

            .entrance-card.error h1 {
                color: #721c24;
                margin-bottom: 1rem;
            }

            .error-message {
                color: #721c24;
                font-size: 1.125rem;
                margin-bottom: 2rem;
                line-height: 1.5;
            }

            .error-details {
                background: #f8d7da;
                padding: 1rem;
                border-radius: 10px;
                margin-bottom: 2rem;
                font-size: 0.875rem;
                color: #721c24;
            }

            .error-details p {
                margin: 0.25rem 0;
            }


            .support-info {
                color: #6c757d;
                font-size: 0.875rem;
            }

            .support-info p {
                margin: 0.25rem 0;
            }

            .entrance-success .reception-button {
                background: linear-gradient(135deg, #4bb71b 0%, #38a815 100%);
                color: white;
                border: none;
                padding: 1rem 2.5rem;
                border-radius: 30px;
                font-size: 1.1rem;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94);
                margin-top: 2rem;
                box-shadow: 0 8px 25px rgba(75, 183, 27, 0.3);
                position: relative;
                overflow: hidden;
            }

            .entrance-success .reception-button:before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
                transition: left 0.5s;
            }

            .entrance-success .reception-button:hover {
                transform: translateY(-3px) scale(1.02);
                box-shadow: 0 12px 35px rgba(75, 183, 27, 0.4);
            }

            .entrance-success .reception-button:hover:before {
                left: 100%;
            }

            .entrance-error .reception-button {
                background: linear-gradient(135deg, #e74c3c 0%, #c0392b 100%);
                color: white;
                border: none;
                padding: 1rem 2.5rem;
                border-radius: 30px;
                font-size: 1.1rem;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94);
                margin-top: 2rem;
                box-shadow: 0 8px 25px rgba(231, 76, 60, 0.3);
                position: relative;
                overflow: hidden;
            }

            .entrance-error .reception-button:before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
                transition: left 0.5s;
            }

            .entrance-error .reception-button:hover {
                transform: translateY(-3px) scale(1.02);
                box-shadow: 0 12px 35px rgba(231, 76, 60, 0.4);
            }

            .entrance-error .reception-button:hover:before {
                left: 100%;
            }
        </style>
    `;

    $('#entrance-container').remove();
    $('#MainContainer').hide();
    $('body').append(errorHTML);

}

// ページリロード関数（クエリパラメータ保持）
function reload(params) {
    // 現在のURLを取得
    const url = new URL(window.location);

    // パラメータが指定されていれば追加
    if (params) {
        Object.keys(params).forEach(key => {
            url.searchParams.set(key, params[key]);
        });
    }

    // リロード（既存のクエリパラメータも保持）
    window.location.href = url.toString();
}
