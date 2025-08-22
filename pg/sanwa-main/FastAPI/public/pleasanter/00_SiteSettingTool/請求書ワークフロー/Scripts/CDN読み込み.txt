// js読み込み
const scripts = [
    "https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.10.6/moment.js",
]
for ( const script of scripts ) {
    console.log(script)
    var elm = document.createElement("script");
    elm.src = script;
    document.head.appendChild(elm);
}
