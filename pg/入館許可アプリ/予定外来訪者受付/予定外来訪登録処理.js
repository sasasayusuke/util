// 予定外来訪受付の登録処理
// HTMLファイルを修正せずに動作させるための実装


// DOMが読み込まれたら初期化処理を実行
(function() {
    // 初期化処理
    async function initializeRegistration() {
        console.log('登録処理.js: 初期化開始');

        // 担当者一覧を取得してセレクトボックスを更新
        await loadStaffMembers();

        // リセットボタンのクリックイベントを登録
        const resetButton = document.getElementById('visitor-reset-button');
        if (resetButton) {
            resetButton.addEventListener('click', handleResetClick);
        } else {
            console.log('リセットボタンが見つかりません');
        }

        // フォームのsubmitイベントも登録（エンターキー対応）
        const form = document.getElementById('visitor-registration-form');
        if (form) {
            form.addEventListener('submit', handleFormSubmit);
        } else {
            console.log('フォームが見つかりません');
        }


    }

    // フォーム送信処理
    function handleFormSubmit(event) {
        event.preventDefault();
        collectAndSubmitFormData();
    }

    // リセットボタンクリック処理
    function handleResetClick(event) {
        event.preventDefault();
        resetForm();
    }

    // フォームデータ収集と送信
    function collectAndSubmitFormData() {

        const formData = {
            staffMember: document.getElementById('visitor-contact-form-staff-member')?.value,
            visitPurpose: document.getElementById('visitor-contact-form-visit-purpose')?.value,
            companyName: document.getElementById('visitor-contact-form-company-name')?.value,
            representativeName: document.getElementById('visitor-contact-form-representative-name')?.value
        };


        handleRegist(formData);
    }

    // フォームリセット処理
    function resetForm() {
        const fields = [
            'visitor-contact-form-staff-member',
            'visitor-contact-form-visit-purpose',
            'visitor-contact-form-company-name',
            'visitor-contact-form-representative-name'
        ];

        fields.forEach(fieldId => {
            const field = document.getElementById(fieldId);
            if (field) {
                field.value = '';
            }
        });

        // エラー表示をクリア
        const errorElements = document.querySelectorAll('.text-red-600');
        errorElements.forEach(el => el.remove());
    }


    // ページ読み込み完了時に初期化
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeRegistration);
    } else {
        initializeRegistration();
    }
})();

// 実際の登録処理を実行する関数
async function handleRegist(formData) {
    try {
        console.log('予定外来訪登録処理を開始:', formData);

        // 入力値の検証
        const errors = validateFormData(formData);
        if (Object.keys(errors).length > 0) {
            const errorMessages = Object.values(errors).join('\n');
            alert(errorMessages);
            return;
        }

        // Pleasanterに登録するデータを準備
        const createData = {
            // 基本情報
            Title: formData.visitPurpose,          // ご来訪目的
            ClassA: formData.representativeName,    // 代表者氏名
            ClassN: formData.staffMember,           // 弊社担当者
            ClassM: formData.companyName,           // 御社名

            CompletionTime: commonGetDate(),

            // 呼び出し中ステータスで作成
            Status: SYSTEM_CONFIG.STATUS.REGISTERED.VALUE
        };

        console.log('登録データ:', createData);

        // Pleasanter APIで登録
        const result = await commonCreateRecord(
            SYSTEM_CONFIG.SITE_INFO.ENTERE_CONTROL.ID,
            createData
        );

        if (!result || result._error) {
            const errorMessage = result?._message || "登録処理中にエラーが発生しました。";
            alert(errorMessage);
            console.error('登録エラー:', errorMessage);
        } else if (result.Id) {
            // 登録成功
            const successMessage = `来訪登録が完了しました！\n受付番号: ${result.Id}\n担当者にご連絡いたします。`;
            alert(successMessage);
            console.log('登録成功:', result);

            // QR着地サイトのURLを生成
            const qrUrl = `${SYSTEM_CONFIG.SITE_INFO.QR_LANDING.URL}?qr=1&id=${result.Id}`;

            // QR着地画面にリダイレクト
            console.log('QR着地画面にリダイレクトします:', qrUrl);
            window.location.href = qrUrl;


        } else {
            alert("登録に失敗しました。");
            console.error('登録失敗:', result);
        }

    } catch (error) {
        console.error("登録処理エラー:", error);
        alert("システムエラーが発生しました。管理者にお問い合わせください。");
    }
}

// フォームデータの検証
function validateFormData(formData) {
    const errors = {};

    // 必須項目のチェック
    if (!formData.staffMember) {
        errors.staffMember = "弊社担当者を選択してください";
    }

    if (!formData.visitPurpose || formData.visitPurpose.trim() === "") {
        errors.visitPurpose = "ご来訪目的を入力してください";
    }

    if (!formData.companyName || formData.companyName.trim() === "") {
        errors.companyName = "御社名を入力してください";
    }

    if (!formData.representativeName || formData.representativeName.trim() === "") {
        errors.representativeName = "代表者氏名を入力してください";
    }

    return errors;
}


// 担当者一覧を取得してセレクトボックスを更新
async function loadStaffMembers() {
    try {
        console.log('担当者一覧の取得を開始');

        const staffMembers = await commonGetRecords(
            SYSTEM_CONFIG.SITE_INFO.STAFF_MASTER.ID,
            {
                columns: ['ResultId', 'ClassB'],
            }
        );

        // セレクトボックスを更新
        const selectElement = document.getElementById('visitor-contact-form-staff-member');
        if (!selectElement) {
            console.error('担当者選択セレクトボックスが見つかりません');
            return;
        }

        // 既存のオプションをクリア（最初の「選択してください」は残す）
        while (selectElement.options.length > 1) {
            selectElement.remove(1);
        }

        console.log(`${staffMembers.length}件の担当者を取得`);

        // 取得したデータをセレクトボックスに追加
        staffMembers.forEach(staff => {
            const option = document.createElement('option');
            option.value = staff["ID"]
            option.textContent = staff["名前"];

            selectElement.appendChild(option);
        });

        console.log(`担当者一覧を取得しました（${staffMembers.length}名）`);

    } catch (error) {
        console.error('担当者一覧取得エラー:', error);
        alert('担当者一覧の取得に失敗しました。');
    }
}



// デバッグ用: 登録処理の動作確認
function testRegist() {
    const testData = {
        staffMember: "2662872",
        visitPurpose: "テスト来訪",
        companyName: "テスト会社",
        representativeName: "テスト太郎"
    };

    console.log('テストデータで登録処理を実行します:', testData);
    handleRegist(testData);
}
