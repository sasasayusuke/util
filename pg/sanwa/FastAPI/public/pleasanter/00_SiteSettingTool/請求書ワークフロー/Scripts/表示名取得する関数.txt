/**
 * 
 * @param {String} label 項目名
 * @param {boolean} valueFlg trueなら表示名、falseならリンクID
 */
function commonGetVal(label, valueFlg = false) {
    let value = ""
    try {
        if ($p.getControl(label).prop("tagName") === "SELECT") {
            if ($p.getControl(label).attr("multiple")) {
                value = valueFlg ? $p.getControl(label).val() : $p.getControl(label).next().children().last().text()
            } else {
                value = valueFlg ? $p.getControl(label).children(':selected').val() : $p.getControl(label).children(':selected').text()
            }
        } else if ($p.getControl(label).prop("tagName") === "INPUT") {
            value = $p.getControl(label).val()
        } else if ($p.getControl($p.getColumnName(label)).prop("tagName") === "TEXTAREA") {
            value = $p.getControl(label).val()
        }

    } catch (e) {
        console.log(label)
        console.log(e)
        value = ""
    } finally {
        return value
    }
}