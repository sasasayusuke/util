// スクリプトに記載しないと動作しないと思われる

// 保存押下時に実行するメソッドを格納する
beforeCreateFuncs = []
beforeUpdateFuncs = []

$p.events.before_send_Create = function (e) {
    console.log("start!! beforeCreateFuncs!!!")
    // falseをreturnするまで繰り返す
    return beforeCreateFuncs.every(func => {
        console.log(func)
        return func(e)
    })
}

$p.events.before_send_Update = function (e) {
    console.log("start!! beforeUpdateFuncs!!!")
    // falseをreturnするまで繰り返す
    return beforeUpdateFuncs.every(func => {
        console.log(func)
        return func(e)
    })
}