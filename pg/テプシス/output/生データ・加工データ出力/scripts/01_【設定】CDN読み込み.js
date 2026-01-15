// === JS読み込み ===
const scripts = [
  // exceljs
  "https://cdnjs.cloudflare.com/ajax/libs/exceljs/4.3.0/exceljs.min.js",
];


for (const script of scripts) {
  const elm = document.createElement("script");
  elm.src = script + "?p=" + new Date().getTime();
  document.head.appendChild(elm);
}