
// 各画面ロード時に実行するメソッドを格納する
onGridLoadFuncs = []
onEditorLoadFuncs = []

// 格納したメソッドを実行するメソッド
$p.events.on_grid_load = () => {
    console.log("start!! onGridLoadFuncs!!!")
    onGridLoadFuncs.forEach(func => {
        // console.log(func)
        func()
    })
}

$p.events.on_editor_load = () => {
    console.log("start!! onEditorLoadFuncs!!!")
    onEditorLoadFuncs.forEach(func => {
        // console.log(func)
        func()
    })
}



// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝タイトル修正処理＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
// 一覧画面
let siteTitle = JSON.parse(document.getElementById("JoinedSites").value)[0].Title

onGridLoadFuncs.push(() => {
    // 一覧画面のタイトルを変更
    document.title = siteTitle + " - 一覧"
})

// 編集画面

onEditorLoadFuncs.push(() => {
    // サイトタイトル編集
    if ($p.action() == "edit") {
        // 編集画面のタイトルを変更
        document.title = siteTitle + " - 編集"
    } else if ($p.action() == "new") {
        // 新規作成画面のタイトルを変更
        document.title = siteTitle + " - 新規作成"
    }
})

// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝ボタン編集禁止調整処理＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
let removeButtons = [
	"EditOutgoingMail",
	"OpenCopyDialogCommand",
	"BulkDeleteCommand",
	"EditImportSettings",
	"CreateCommand",
	"DeleteCommand",
]
onGridLoadFuncs.push(() => {
    removeButtons.forEach(id => document.getElementById(id)?.remove());
})
onEditorLoadFuncs.push(() => {
    removeButtons.forEach(id => document.getElementById(id)?.remove());
})
// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝同行者人数連動処理＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
onEditorLoadFuncs.push(() => {
    // 同行者フィールドの定義
    const companionFields = [
        ['Issues_ClassCField', 'Issues_ClassDField'], // 同行者1
        ['Issues_ClassEField', 'Issues_ClassFField'], // 同行者2
        ['Issues_ClassGField', 'Issues_ClassHField'], // 同行者3
        ['Issues_ClassIField', 'Issues_ClassJField'], // 同行者4
        ['Issues_ClassKField', 'Issues_ClassLField']  // 同行者5
    ];

    // フィールドIDリストを作成
    const allCompanionFieldIds = companionFields.flat();

    // 同行者フィールドを最初から非表示にするスタイルを追加
    const hideCompanionFieldsStyle = document.createElement('style');
    hideCompanionFieldsStyle.textContent = `
        #${allCompanionFieldIds.join(',\n        #')} {
            display: none !important;
        }
    `;
    document.head.appendChild(hideCompanionFieldsStyle);

    // 少し遅延を入れてスピナーの初期化を待つ
    setTimeout(() => {
        // 同行者人数の入力要素を取得
        const companionCountInput = document.getElementById('Issues_NumA');

        // 要素が存在しない場合は処理を終了
        if (!companionCountInput) return;

        // フィールドの表示/非表示を制御する関数
        function updateFieldsVisibility() {
            const count = parseInt(companionCountInput.value) || 0;

            // 各同行者のフィールドを処理
            companionFields.forEach((fieldPair, index) => {
                const nameField = document.getElementById(fieldPair[0]);
                const emailField = document.getElementById(fieldPair[1]);

                if (index < count) {
                    // 必要なフィールドを表示（!importantを上書き）
                    if (nameField) nameField.style.setProperty('display', 'block', 'important');
                    if (emailField) emailField.style.setProperty('display', 'block', 'important');
                } else {
                    // 不要なフィールドを非表示
                    if (nameField) {
                        nameField.style.setProperty('display', 'none', 'important');
                        // 入力値をクリア
                        const nameInput = nameField.querySelector('input');
                        if (nameInput) nameInput.value = '';
                    }
                    if (emailField) {
                        emailField.style.setProperty('display', 'none', 'important');
                        // 入力値をクリア
                        const emailInput = emailField.querySelector('input');
                        if (emailInput) emailInput.value = '';
                    }
                }
            });
        }

        // 入力値が変更されたときの処理
        companionCountInput.onchange = updateFieldsVisibility;

        // スピナーボタンのイベントを監視（イベントデリゲーション方式）
        companionCountInput.parentElement.addEventListener('click', function(e) {
            // クリックされた要素がスピナーボタンかチェック
            if (e.target.closest('.ui-spinner-button')) {
                setTimeout(updateFieldsVisibility, 100);
            }
        });

        // 初期状態で実行
        updateFieldsVisibility();

    }, 500); // スピナーの初期化を待つため500ms遅延
})