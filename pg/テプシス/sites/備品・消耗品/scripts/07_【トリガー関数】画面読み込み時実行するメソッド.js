// 一覧画面ロード時に実行するメソッドを格納する
$p.events.on_grid_load_arr = []

// 格納したメソッドを実行するメソッド
$p.events.on_grid_load = function() {
    $p.events.on_grid_load_arr.forEach(func => {
        func()
    })
}

// 編集画面ロード時に実行するメソッドを格納する
$p.events.on_editor_load_arr = []

// 格納したメソッドを実行するメソッド
$p.events.on_editor_load = function() {
    $p.events.on_editor_load_arr.forEach(func => {
        func()
    })
}