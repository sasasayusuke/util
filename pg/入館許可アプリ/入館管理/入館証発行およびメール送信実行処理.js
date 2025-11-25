function execute() {
    const errorFormChanged = '送信前に更新ボタンを押してレコードの最新の内容を保存してください';

    if ($p.formChanged == 'undefined' || $p.formChanged) {
        alert(errorFormChanged);
        return;
    }

    if (confirm('この内容でQR発行およびメール送信を行いますか？')) {
        // ローディングオーバーレイを表示
        commonShowLoadingOverlay('処理中...', 'QRコード付きPDF生成とメール送信を行っています');

        try {
        // PDF生成用のデータを取得
        const values = SYSTEM_CONFIG.PDF.TEXT.VALUES;

        // システム設定からQR着地サイトIDを取得してURLを生成
        const recordId = values.RECEPTION_ID;
        const qrSiteId = SYSTEM_CONFIG.SITE_INFO.QR_LANDING.ID;
        const qrUrl = `${SYSTEM_CONFIG.SITE_INFO.QR_LANDING.URL}?qr=1&id=${recordId}`;

        // デバッグログ
        console.log('実行処理 - qrSiteId:', qrSiteId, 'recordId:', recordId, 'qrUrl:', qrUrl);

        // QRコード付きPDFを生成してアップロード
        generatePDF({
            includeQRCode: true,
            qrText: qrUrl,
            uploadToRecord: recordId
        })
        .then(async function() {
            // PDF生成が成功した場合
            // 2秒待機してからメール送信
            await new Promise(resolve => setTimeout(resolve, 2000));

            const val = SYSTEM_CONFIG.PDF.TEXT.VALUES;
            const com = SYSTEM_CONFIG.COMPANY_INFO;

            // メール件名
            const subject = `入館手続き完了のご案内（${com.BUILDING_NAME}/受付ID:${val.RECEPTION_ID})`;

            // メール本文
            const body = commonTrimLines(`
                ${val.COMPANY_NAME}
                ${val.REP_NAME} 様

                いつも大変お世話になっております。
                ${com.NAME}です。

                このたびはご来訪のお手続きをいただき、誠にありがとうございます。
                入館システムへのご登録が完了いたしましたので、以下のとおりご案内申し上げます。

                【ご来訪情報】
                受付ID: ${val.RECEPTION_ID}
                来訪日時: ${val.VISIT_DATETIME_WITH_END}
                貴社名: ${val.COMPANY_NAME}
                代表者氏名: ${val.REP_NAME} 様
                同行者人数: ${val.SUB_COUNT}名

                【当日のご案内】
                ・本メールに添付の「入館用QRコード(PDF)」を印刷またはスマートフォン等で表示いただき、
                　ビル受付に設置のQRコードリーダーにかざしてください。
                ・ご不明な点がございましたら、当社受付までご連絡ください。

                なお、本メールは入館手続き完了のご案内として自動送信されています。
                今後ともよろしくお願いいたします。

                ${com.NAME}
                管理本部
                電話：${com.TEL}`
            )

            sendMail({
                to: val.REP_EMAIL,
                subject: subject,
                body: body,
                includeAttachments: true
            });
			await commonUpdateRecord(recordId, { 'Status': SYSTEM_CONFIG.STATUS.REGISTERED });
            commonHideLoadingOverlay();
            alert('PDF発行とメール送信が完了しました。\nステータス更新のため画面がリロードされます');
			window.location.reload();
        })
        .catch(function(error) {
            commonHideLoadingOverlay();
            commonMessage(`処理エラー: ${error.message}`, 'error');
            console.error('処理エラー:', error);
        });
        } catch (error) {
            commonHideLoadingOverlay();
            commonMessage(`処理エラー: ${error.message}`, 'error');
            console.error('処理エラー:', error);
        }
    }
}

