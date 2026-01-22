let fnExcelJsReady = false;

(function () {
  const elm = document.createElement("script");
  elm.src = "https://cdn.jsdelivr.net/npm/exceljs/dist/exceljs.min.js";
  elm.onload = function () {
    fnExcelJsReady = true;
    console.log("ExcelJS ready");
  };
  document.head.appendChild(elm);
})();

