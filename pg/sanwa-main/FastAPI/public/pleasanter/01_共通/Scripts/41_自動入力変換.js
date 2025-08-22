/**
 * 指定した項目に入力されたらｶﾅ文字に変換して別項目に入力する関数です。
 *
 * @param {string} label1 監視対象の項目名
 * @param {string} label2 入力対象の項目名
 * @param {object} passedOptions ひらがなにするかカタカナにするか
 */
function inputAutoKana(label1, label2, passedOptions = {}) {
    const options = {
        katakana: false,
        ...passedOptions
    };

    const kana_extraction_pattern = new RegExp('[^ 　ぁあ-んーa-zA-Zｱ-ﾝ0-9]', 'g');
    const kana_compacting_pattern = new RegExp('[ぁぃぅぇぉっゃゅょ]', 'g');
    let elName, elKana, active = false, timer = null, flagConvert = true,
        input, values, ignoreString, baseKana;

    elName = document.getElementById(commonGetId(label1))
    elKana = document.getElementById(commonGetId(label2))
    active = true;
    _stateClear();

    elName.addEventListener('blur', _eventBlur);
    elName.addEventListener('focus', _eventFocus);
    elName.addEventListener('keydown', _eventKeyDown);

    function _checkConvert(new_values) {
        if (!flagConvert) {
            if (Math.abs(values.length - new_values.length) > 1) {
                const tmp_values = new_values.join('').replace(kana_compacting_pattern, '').split('');
                if (Math.abs(values.length - tmp_values.length) > 1) {
                    _stateConvert();
                }
            } else {
                if (values.length == input.length && values.join('') != input) {
                    if (input.match(kana_extraction_pattern)) {
                        _stateConvert();
                    }
                }
            }
        }
    }

    function _checkValue() {
        let new_input = elName.value;
        if (new_input == '' && elKana.value != '') {
            _stateClear();
            _setKana();
        } else {
            new_input = _removeString(new_input);
            if (input != new_input) {
                input = new_input;
                if (!flagConvert) {
                    const new_values = new_input.replace(kana_extraction_pattern, '').split('');
                    _checkConvert(new_values);
                    _setKana(new_values);
                }
            }
        }
    }

    function _clearInterval() {
        clearInterval(timer);
    }

    function _eventBlur() {
        _clearInterval();
    }

    function _eventFocus() {
        _stateInput();
        _setInterval();
    }

    function _eventKeyDown() {
        if (flagConvert) {
            _stateInput();
        }
    }

    function _isHiragana(chara) {
        return ((chara >= 12353 && chara <= 12435) || chara == 12445 || chara == 12446);
    }

    function _removeString(new_input) {
        if (new_input.indexOf(ignoreString) !== -1) {
            return new_input.replace(ignoreString, '');
        } else {
            const ignoreArray = ignoreString.split('');
            const inputArray = new_input.split('');
            for (let i = 0; i < ignoreArray.length; i++) {
                if (ignoreArray[i] == inputArray[i]) {
                    inputArray[i] = '';
                }
            }
            return inputArray.join('');
        }
    }

    function _setInterval() {
        timer = setInterval(_checkValue, 10);
    }

    function _setKana(new_values) {
        if (!flagConvert) {
            if (new_values) {
                values = new_values;
            }
            if (active) {
                const _val = _toKatakana(baseKana + values.join(''));
                elKana.value = _val;
                elKana.dispatchEvent(new Event('change'));
            }
        }
    }

    function _stateClear() {
        baseKana = '';
        flagConvert = false;
        ignoreString = '';
        input = '';
        values = [];
    }

    function _stateInput() {
        baseKana = elKana.value;
        flagConvert = false;
        ignoreString = elName.value;
    }

    function _stateConvert() {
        baseKana = baseKana + values.join('');
        flagConvert = true;
        values = [];
    }

    function _toKatakana(src) {
        const fullWidthToHalfWidth = {
            'ァ': 'ｧ', 'ィ': 'ｨ', 'ゥ': 'ｩ', 'ェ': 'ｪ', 'ォ': 'ｫ',
            'ャ': 'ｬ', 'ュ': 'ｭ', 'ョ': 'ｮ', 'ッ': 'ｯ',
            'ア': 'ｱ', 'イ': 'ｲ', 'ウ': 'ｳ', 'エ': 'ｴ', 'オ': 'ｵ',
            'カ': 'ｶ', 'キ': 'ｷ', 'ク': 'ｸ', 'ケ': 'ｹ', 'コ': 'ｺ',
            'サ': 'ｻ', 'シ': 'ｼ', 'ス': 'ｽ', 'セ': 'ｾ', 'ソ': 'ｿ',
            'タ': 'ﾀ', 'チ': 'ﾁ', 'ツ': 'ﾂ', 'テ': 'ﾃ', 'ト': 'ﾄ',
            'ナ': 'ﾅ', 'ニ': 'ﾆ', 'ヌ': 'ﾇ', 'ネ': 'ﾈ', 'ノ': 'ﾉ',
            'ハ': 'ﾊ', 'ヒ': 'ﾋ', 'フ': 'ﾌ', 'ヘ': 'ﾍ', 'ホ': 'ﾎ',
            'マ': 'ﾏ', 'ミ': 'ﾐ', 'ム': 'ﾑ', 'メ': 'ﾒ', 'モ': 'ﾓ',
            'ヤ': 'ﾔ', 'ユ': 'ﾕ', 'ヨ': 'ﾖ',
            'ラ': 'ﾗ', 'リ': 'ﾘ', 'ル': 'ﾙ', 'レ': 'ﾚ', 'ロ': 'ﾛ',
            'ワ': 'ﾜ', 'ヲ': 'ｦ', 'ン': 'ﾝ',
            'ガ': 'ｶﾞ', 'ギ': 'ｷﾞ', 'グ': 'ｸﾞ', 'ゲ': 'ｹﾞ', 'ゴ': 'ｺﾞ',
            'ザ': 'ｻﾞ', 'ジ': 'ｼﾞ', 'ズ': 'ｽﾞ', 'ゼ': 'ｾﾞ', 'ゾ': 'ｿﾞ',
            'ダ': 'ﾀﾞ', 'ヂ': 'ﾁﾞ', 'ヅ': 'ﾂﾞ', 'デ': 'ﾃﾞ', 'ド': 'ﾄﾞ',
            'バ': 'ﾊﾞ', 'ビ': 'ﾋﾞ', 'ブ': 'ﾌﾞ', 'ベ': 'ﾍﾞ', 'ボ': 'ﾎﾞ',
            'パ': 'ﾊﾟ', 'ピ': 'ﾋﾟ', 'プ': 'ﾌﾟ', 'ペ': 'ﾍﾟ', 'ポ': 'ﾎﾟ'
        };
        if (options.katakana) {
            let str = '';
            for (let i = 0; i < src.length; i++) {
                const c = src.charCodeAt(i);
                if (_isHiragana(c)) {
                    str += String.fromCharCode(c + 96);
                } else {
                    str += src.charAt(i);
                }
            }
            return str.replace(/[\u30A1-\u30F6]/g, match => fullWidthToHalfWidth[match] || match);
        } else {
            return src;
        }
    }
}

/**
 * 指定した項目に入力されたら頭文字を入力する関数です。
 *
 * @param {string} label1 監視対象の項目名
 * @param {string} label2 入力対象の項目名
 * @param {number} size デフォルト4文字
 */
function inputAutoRyaku(label1, label2, size=4) {
    document.getElementById(commonGetId(label1)).addEventListener('input', function() {
        document.getElementById(commonGetId(label2)).value = this.value.slice(0, size);
    });
}