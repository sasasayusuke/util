// === JS読み込み（ExcelJSのみ） ===
const scripts = [
  // Excel出力用（ExcelJS）
  "https://cdn.jsdelivr.net/npm/exceljs/dist/exceljs.min.js"
];

for (const script of scripts) {
  const elm = document.createElement("script");
  elm.src = script + "?p=" + new Date().getTime();
  document.head.appendChild(elm);
}
