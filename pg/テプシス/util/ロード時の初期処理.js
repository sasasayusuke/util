
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
