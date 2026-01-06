/* ============================================================
    一覧画面：ボタン追加
============================================================ */
(function () {
    const fnMainCommands = document.getElementById("MainCommands");
    if (!fnMainCommands) return;
    if (document.getElementById("fn-download-usage-excel")) return;

    const fnBtn = document.createElement("button");
    fnBtn.id = "fn-download-usage-excel";
    fnBtn.type = "button";
    fnBtn.className = "button ui-button ui-corner-all ui-widget applied";
    fnBtn.textContent = "利用状況をExcelでダウンロード";
    fnBtn.addEventListener("click", fnOpenModal);

    fnMainCommands.appendChild(fnBtn);
})();

/* ============================================================
    モーダルを開く
============================================================ */
function fnOpenModal() {
    if (document.getElementById("sdt-usage-excel-modal")) return;

    const fnWrap = document.createElement("div");
    fnWrap.id = "sdt-usage-excel-modal";
    fnWrap.className = "sdt-modal-wrap";

    fnWrap.innerHTML = `
        <div class="sdt-modal">
            <button type="button" class="sdt-modal__close" aria-label="close">×</button>

            <div class="sdt-modal__header">
                <div class="sdt-modal__title">利用状況Excelダウンロード</div>
            </div>

            <div class="sdt-modal__content">
                <div class="sdt-field">
                    <div class="sdt-field__label">
                        ダウンロード対象の年月 <span class="sdt-field__required">*</span>
                    </div>
                    <input id="fn-target-month" class="sdt-field__input" type="month" />
                </div>

                <div class="sdt-actions">
                    <button
                        id="fn-modal-download"
                        type="button"
                        class="sdt-btn sdt-btn--primary"
                    >
                        ダウンロード
                    </button>
                </div>
            </div>
        </div>
    `;

    document.body.appendChild(fnWrap);

    // 初期値：今月
    const fnNow = new Date();
    const fnY = fnNow.getFullYear();
    const fnM = String(fnNow.getMonth() + 1).padStart(2, "0");
    fnWrap.querySelector("#fn-target-month").value = `${fnY}-${fnM}`;

    // ダウンロード
    fnWrap
        .querySelector("#fn-modal-download")
        .addEventListener("click", fnDownloadUsageExcel);

    // ×ボタンで閉じる
    fnWrap
        .querySelector(".sdt-modal__close")
        .addEventListener("click", fnCloseModal);
}

/* ============================================================
    ダウンロード押下（ここからExcel処理へ）
============================================================ */
function fnDownloadUsageExcel() {
    const fnInput = document.getElementById("fn-target-month");
    const fnYm = fnInput ? String(fnInput.value || "") : ""; // 例 "2026-01"

    if (!fnYm) {
        alert("年月を選択してください");
        return;
    }

    console.log("OK", { fnYm: fnYm });

    // いったん閉じる（好み。閉じないなら消してOK）
    // fnCloseModal();
}

/* ============================================================
    モーダルを閉じる
============================================================ */
function fnCloseModal() {
    document.getElementById("sdt-usage-excel-modal")?.remove();
}
