
// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝ボタン編集禁止調整処理＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
let removeButtons = [
	"Navigations",
	"EditOutgoingMail",
	"OpenCopyDialogCommand",
	"BulkDeleteCommand",
	"OriginBulkDeleteCommand",
	"EditImportSettings",
	"UpdateCommand",
	"DeleteCommand",
	"EditOnGridCommand",
]
onGridLoadFuncs.push(() => {
    removeButtons.forEach(id => document.getElementById(id)?.remove());
})
onEditorLoadFuncs.push(() => {
    removeButtons.forEach(id => document.getElementById(id)?.remove());
})
