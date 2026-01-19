(function ($) {
    'use strict';

    // SectionFields のコンテナ ID
    const sectionFieldsContainers = {
        s3: 'SectionFields3Container',
        s4: 'SectionFields4Container',
        s5: 'SectionFields5Container',
        s6: 'SectionFields6Container'
    };

    // オーケストレーター
    async function indexProcess() {
        try {
            // check_period_Site は別で定義されている想定
            if (typeof check_period_Site === 'undefined' || check_period_Site === null) {
                return;
            }

            // data-id 属性で該当テーブルを取得
            var tableSelector = 'table[data-id="' + check_period_Site + '"]';
            var $table = $(tableSelector);

            // テーブルが無い、または「中身が空（innerHTMLが空）」、あるいはデータ行（tdを持つtr）が無い場合は非表示処理
            var tableMissingOrEmpty = false;

            if ($table.length === 0) {
                tableMissingOrEmpty = true;
            } else {
                // innerHTML が空かどうか（空白のみも含む）
                if ($.trim($table.html()) === '') {
                    tableMissingOrEmpty = true;
                } else {
                    // tbody があれば tbody 内の tr に td があるかをチェック
                    var $tbody = $table.find('tbody');
                    var $dataRows;
                    if ($tbody.length > 0) {
                        $dataRows = $tbody.find('tr').filter(function () {
                            return $(this).find('td').length > 0;
                        });
                    } else {
                        // tbody が無ければ table 内の tr をチェック（ヘッダ行のみの場合を除外）
                        $dataRows = $table.find('tr').filter(function () {
                            return $(this).find('td').length > 0;
                        });
                    }
                    if (!$dataRows || $dataRows.length === 0) {
                        tableMissingOrEmpty = true;
                    }
                }
            }

            if (tableMissingOrEmpty) {
                // 非表示対象セレクタを作る（参照変数が未定義でもエラーにならないようガード）
                var aId = (typeof check_foodTruck_site !== 'undefined' && check_foodTruck_site !== null) ? check_foodTruck_site : '';
                var bId = (typeof check_vipList_site !== 'undefined' && check_vipList_site !== null) ? check_vipList_site : '';

                if (aId !== '') $('#Results_Source' + aId + 'Field').hide();
                if (bId !== '') $('#Results_Source' + bId + 'Field').hide();

                // SectionFields コンテナ群を非表示（存在しなければ jQuery は無視）
                $('#' + sectionFieldsContainers.s6).hide();
                $('#' + sectionFieldsContainers.s3).hide();
                $('#' + sectionFieldsContainers.s4).hide();
                $('#' + sectionFieldsContainers.s5).hide();

                return;
            }

            // テーブルが存在し、かつ中身（データ行）がある場合は何もしない
            return;
        } catch (err) {
            if (typeof console !== 'undefined' && console.error) console.error('indexProcess内エラー:', err);
            return;
        }
    }

    // トリガー
    $(function() {
        indexProcess();
    });

    if (window.$p && $p.events) {
        var originalAfterSet = $p.events.after_set;
        $p.events.after_set = function () {
            if (typeof originalAfterSet === 'function') {
                originalAfterSet.apply(this, arguments);
            }
            indexProcess();
        };
    }
})(jQuery);
