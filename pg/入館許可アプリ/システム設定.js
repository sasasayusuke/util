// システム設定ファイル
// 環境に応じて値を変更してください

const SYSTEM_CONFIG = {

    // 会社情報
    COMPANY_INFO: {
        NAME: 'SMSデータテック 株式会社',
        ADDRESS: '東京都中央区湊3-5-10',
        BUILDING_NAME: 'VORT新富町ビル 3F',
        TEL: '03-6222-0831'
    },

    // Pleasanterサイト情報
    SITE_INFO: {
        BASE_URL: location.origin,
        QR_LANDING: {
            ID: "2662465",
            get URL() { return `${SYSTEM_CONFIG.SITE_INFO.BASE_URL}/publishes/${this.ID}/index`; }
        },
        RECEPTION: {
            ID: "2662566",
            get URL() { return `${SYSTEM_CONFIG.SITE_INFO.BASE_URL}/publishes/${this.ID}/index`; }
        },
        QR_READER: {
            ID: "2662567",
            get URL() { return `${SYSTEM_CONFIG.SITE_INFO.BASE_URL}/publishes/${this.ID}/index`; }
        }
    },

    // ステータス定義
    STATUS: {
        RECEPTION:  100,    // 仮受付
        REGISTERED: 200,    // 登録済み
        CALLING:    300,    // 呼び出し中
        ENTERED:    900,    // 入館済み
        ERROR:      999     // エラー
    },

    // API設定
    API: {
        VERSION: 1.1,
        TIMEOUT: 300000,
        EMPTY_DATE: '1899-12-30T00:00:00'
    },

    // QRコード設定
    QR_CODE: {
        // デフォルトオプション
        DEFAULT_OPTIONS: {
            width: 256,
            height: 256,
            colorDark: '#000000',
            colorLight: '#ffffff',
            correctLevel: QRCode.CorrectLevel.M,
            padding: 20,
            format: 'png'
        }
    },

    // PDF設定
    PDF: {
        // ページ設定
        PAGE: {
            orientation: 'portrait',
            unit: 'mm',
            format: 'a4'
        },
        // フォントサイズ
        FONT_SIZE: {
            title: 24,
            subtitle: 20,
            normal: 16,
            small: 14,
            header: 22,
            label: 18,
            value: 18,
            notice: 14
        },
        // 余白設定
        MARGIN: {
            top: 5,
            left: 20,
            right: 20,
            bottom: 5
        },
        // カラー設定
        COLORS: {
            primary: '#2c3e50',
            secondary: '#34495e',
            accent: '#3498db',
            text: '#333333'
        },
        // デザイン設定
        DESIGN: {
            qrSize: 40,  // QRコードのサイズ（mm）
            lineHeight: 10, // 行間（mm）
            sectionGap: 15  // セクション間の余白（mm）
        },

        // テキスト設定
        TEXT: {
            // ラベル
            LABELS: {
                RECEPTION: '受付番号',
                DATE: 'ご来訪日時',
                CUSTOMER: 'お客様名',
                LOCATION: '場所',
                MEETING_ROOM: '会議室',
                PURPOSE: '来訪目的',
                ENTRANCE_METHOD: '入館方法',
                NOTICE: '注意事項/Notice'
            },
            // メッセージ（固定文言のみ）
            MESSAGES: {
                CONFIRMATION: '下記の通りご来訪を承りましたのでお知らせいたします。',
                ENTRANCE_JA: 'メールに添付されておりますQRコードをiPadにかざしていただき、入館をお願い致します。',
                ENTRANCE_EN: 'Enter the facility by holding the QR code attached to this email over the iPad',
                NOTICE_1: '・本システムで入力された情報は、入力目的の範囲内で利用し、目的以外で利用することはございません。',
                NOTICE_2: '　また、入力された情報は適切に保護されます。',
                NOTICE_3: '・Information entered in this system will be used only for the purpose',
                NOTICE_4: '　for which it was entered and will not be used for any other purpose.',
                NOTICE_5: '　The entered information will be properly protected.',
                CLOSING: '当日のお越しをお待ちしております。'
            },

            // 動的な値（関数として定義）
            VALUES: {
                get RECEPTION_ID() { return String(commonGetVal("受付ID") || '') },
                get COMPANY_NAME() { return String(commonGetVal("御社名") || '') },
                get REP_NAME() { return String(commonGetVal("代表者氏名") || '') },
                get REP_EMAIL() { return String(commonGetVal("代表者メールアドレス") || '') },
                get VISIT_DATETIME() { return String(commonGetVal("ご来訪日時") || '') },
                get OUR_STAFF() { return String(commonGetVal("弊社担当者") || '') },
                get SUB_COUNT() { return String(commonGetVal("同行者人数") || '0') },
                get MEETING_ROOM() { return String(commonGetVal("会議室") || '') },
                get PURPOSE() { return String(commonGetVal("ご来訪目的") || '') },
                get STAY_DURATION() { return Number(commonGetVal("滞在予定時間")) || 60; }, // デフォルト60分
                get LOCATION() { return `${SYSTEM_CONFIG.COMPANY_INFO.NAME}\n${SYSTEM_CONFIG.COMPANY_INFO.ADDRESS}\n${SYSTEM_CONFIG.COMPANY_INFO.BUILDING_NAME}`; },

                // 来訪日時を終了時刻付きで表示
                get VISIT_DATETIME_WITH_END() {
                    const visitDateTime = this.VISIT_DATETIME;
                    const stayDuration = this.STAY_DURATION;

                    if (!visitDateTime) return '';

                    try {
                        // 日時をDateオブジェクトに変換
                        const startDate = new Date(visitDateTime);
                        // 終了時刻を計算（分単位で加算）
                        const endDate = new Date(startDate.getTime() + stayDuration * 60000);

                        // 曜日の配列
                        const weekdays = ['日', '月', '火', '水', '木', '金', '土'];

                        // 日本語形式でフォーマット
                        const formatDateTimeJP = (date) => {
                            const year = date.getFullYear();
                            const month = date.getMonth() + 1;
                            const day = date.getDate();
                            const weekday = weekdays[date.getDay()];
                            const hours = String(date.getHours()).padStart(2, '0');
                            const minutes = String(date.getMinutes()).padStart(2, '0');
                            return `${year}年${month}月${day}日（${weekday}）${hours}:${minutes}`;
                        };

                        // 終了時刻のみ時分を取得
                        const endHours = String(endDate.getHours()).padStart(2, '0');
                        const endMinutes = String(endDate.getMinutes()).padStart(2, '0');

                        return `${formatDateTimeJP(startDate)} ～ ${endHours}:${endMinutes}`;
                    } catch (error) {
                        console.error('日時フォーマットエラー:', error);
                        return visitDateTime; // エラー時は元の値を返す
                    }
                }
            }
        }
    }
}

