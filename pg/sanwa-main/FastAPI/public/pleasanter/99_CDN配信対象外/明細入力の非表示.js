["仕入明細入力", "売上明細入力"].forEach(v => {
    const link = Array.from(document.getElementsByTagName('a')).find(a =>
        a.textContent.trim() === v
    );
    if (link) {
        const listItem = link.closest('li'); // 親の <li> を取得
        if (listItem) {
            listItem.remove(); // <li> を削除
        }
    }
});