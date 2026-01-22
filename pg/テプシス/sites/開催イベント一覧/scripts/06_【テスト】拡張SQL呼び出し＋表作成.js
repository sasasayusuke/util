(function ($) {
  'use strict';

  /*************************************************************************
   * upcoming events — 初心者向けに丁寧にコメントした実装
   *
   * 全体の流れ（上位関数: upcomingEventsFormAction）
   * 1) 入力（開始日・終了日）を取得
   * 2) 取得した日付を alert で表示（今回の代替動作）
   * 3) API の代わりにダミーデータを取得（fetchEventsDummy）
   * 4) 各イベントの開催期間 (EventPeriod) を解析し、
   *    期間内の日ごとに行を「穴埋め」する（expandEventsByPeriod）
   * 5) 生成した行を HTML として描画する（renderEvents）
   *
   * 注意:
   * - 外部公開は upcomingEventsFormAction のみ（onclick 用）
   * - 他の関数は IIFE 内に閉じます（読みやすさ優先）
   *************************************************************************/

  /**
   * getInputDates
   * - 画面にある #fn-startDatet と #fn-endDate の input 要素から値を取得します。
   * - 要素が無ければ空文字を返します（呼び出し側で扱いやすい形に統一するため）。
   *
   * 戻り値の構造:
   *   { start: 'YYYY-MM-DD など', end: 'YYYY-MM-DD など' }
   */
  function getInputDates() {
    // jQuery で要素を取得
    var $start = $('#fn-startDate');
    var $end = $('#fn-endDate');

    // val() が undefined なら '' に統一して返す
    return {
      start: ($start.length ? String($start.val() || '') : ''),
      end: ($end.length ? String($end.val() || '') : '')
    };
  }

  /**
   * showDatesAlert
   * - 取得した日付を画面に alert で表示します（API 実行の代替挙動）。
   * - ここはデバッグ／動作確認用なので、実運用では API 呼び出しに置き換えます。
   */
  function showDatesAlert(dates) {
    // 短く見やすいメッセージで表示
    alert('開始: ' + dates.start + '\n終了: ' + dates.end);
  }

  /**
   * fetchEventsDummy
   * - 本来はサーバー API を叩いてイベントを取得する場所。
   * - 今回はダミーの配列を返す（非同期のふりをするため Deferred を返す）。
   *
   * 引数:
   *   dates - 将来 API に渡す可能性のある日付オブジェクト（今回は使わない）
   *
   * 戻り値:
   *   jQuery Deferred.promise() を返す（resolve されると配列が返る）
   */
  async function fetchEventsDummy(dates) {

    // パラメーターなし
    const getEventScheduleList = sql(
        "getEventScheduleList",
        { DateFrom: dates.start, DateTo: dates.end }
    );

    const rows = await getEventScheduleList({});

    console.log("rows length:", rows.length);
    console.table(rows[0]);
    return rows;
  }

  /******************************************************************
   * ここから「期間展開（穴埋め）」に関する補助関数群（初心者向けに丁寧に）
   ******************************************************************/

  /**
   * parsePeriod
   * - EventPeriod の文字列（例: "2026/03/01 - 2026/03/03"）から
   *   開始日と終了日を Date オブジェクトにして返す関数
   *
   * - サポートする形式: yyyy/mm/dd または yyyy-mm-dd が本文中に 1 個以上あるもの
   * - 解析に失敗したら null を返す（呼び出し側でフォールバック処理を行う）
   *
   * 戻り値の例:
   *   { start: DateObject, end: DateObject } または null
   */
  function parsePeriod(periodStr) {
    if (!periodStr || typeof periodStr !== 'string') return null;

    // 正規表現で yyyy/mm/dd または yyyy-mm-dd をすべて探す
    var re = /(\d{4})[\/\-](\d{1,2})[\/\-](\d{1,2})/g;
    var matches = [];
    var m;
    while ((m = re.exec(periodStr)) !== null) {
      matches.push({ y: parseInt(m[1], 10), mo: parseInt(m[2], 10), d: parseInt(m[3], 10) });
    }

    // 日付が見つからなければ null
    if (matches.length === 0) return null;

    // 見つかった最初の 1 個を開始日、2 個目を終了日（2 個無ければ開始と同じ日を終了とする）
    var s = matches[0];
    var e = matches.length > 1 ? matches[1] : matches[0];

    // Date コンストラクタに渡す（注意: 月は 0 起点なので -1 する）
    var startDate = new Date(s.y, s.mo - 1, s.d);
    var endDate = new Date(e.y, e.mo - 1, e.d);

    // 日付の妥当性チェック
    if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) return null;

    // start が end より後なら入れ替える（安全措置）
    if (startDate.getTime() > endDate.getTime()) {
      var tmp = startDate;
      startDate = endDate;
      endDate = tmp;
    }

    return { start: startDate, end: endDate };
  }

  /**
   * getDatesInRange
   * - 開始日から終了日まで（両端 inclusive）の Date オブジェクト配列を返す。
   * - 例: start=2026-03-01, end=2026-03-03 => [2026-03-01, 2026-03-02, 2026-03-03]
   *
   * ※ Date はコピーして返します（元の Date を壊さないように）
   */
    function getDatesInRange(start, end, dates) {

        // JSTチェック用（"YYYY-MM-DD"）
        var checkStart = toCheckJSTDate(start);
        var checkEnd   = toCheckJSTDate(end);

        if (!dates || !dates.start || !dates.end) return [];

        // 文字列 → Date
        var rangeStart = new Date(dates.start);
        var rangeEnd   = new Date(dates.end);

        var periodStart = new Date(checkStart);
        var periodEnd   = new Date(checkEnd);

        if (
            isNaN(rangeStart) || isNaN(rangeEnd) ||
            isNaN(periodStart) || isNaN(periodEnd)
        ) return [];

        // ★ 重なっている開始日・終了日を求める
        var actualStart = periodStart > rangeStart ? periodStart : rangeStart;
        var actualEnd   = periodEnd   < rangeEnd   ? periodEnd   : rangeEnd;

        // 重なりがなければ空
        if (actualStart > actualEnd) return [];

        var newDates = [];

        // 年月日のみで回す
        var cur = new Date(
            actualStart.getFullYear(),
            actualStart.getMonth(),
            actualStart.getDate()
        );
        var last = new Date(
            actualEnd.getFullYear(),
            actualEnd.getMonth(),
            actualEnd.getDate()
        );

        while (cur.getTime() <= last.getTime()) {
            newDates.push(new Date(cur.getFullYear(), cur.getMonth(), cur.getDate()));
            cur.setDate(cur.getDate() + 1);
        }

        return newDates;
    }


  /**
   * formatToDisplayDate
   * - Date オブジェクトを「3月1日」のような表示用文字列に変換する小さなユーティリティ
   * - 表示を見やすくするために使います
   */
  function formatToDisplayDate(dateObj) {
    if (!(dateObj instanceof Date) || isNaN(dateObj.getTime())) return '';
    var month = dateObj.getMonth() + 1; // 月は 0 起点なので +1
    var day = dateObj.getDate();
    return month + '月' + day + '日';
  }

  /**
   * expandEventsByPeriod
   * - 元の eventsArray（各要素がイベントオブジェクト）を受け取り、
   *   EventPeriod を解析して期間分（1日ごと）に複製・展開した新しい配列を返す関数
   *
   * - 例:
   *   元: [{ EventPeriod: "2026/03/01 - 2026/03/03", ... }]
   *   結果: [{ Date: "3月1日", ... }, { Date: "3月2日", ... }, { Date: "3月3日", ... }]
   *
   * - 解析に失敗した要素は「元の要素を1行分だけ使う」ようにしています（最小限のフォールバック）
   */
    function expandEventsByPeriod(eventsArray,dates) {
    if (!Array.isArray(eventsArray)) return [];
    var out = [];

    eventsArray.forEach(function (item) {
        if (item["開始日"] && item["終了日"]) {

            // 期間（JST）
            var start = toJSTDate(item["開始日"]);
            var end   = toJSTDate(item["終了日"]);
            let test = toJSTDate(item["勤務日"])
            if (!start || !end) return;

            var days = getDatesInRange(start, end, dates);

            days.forEach(function (d) {
                var copy = Object.assign({}, item);

                copy.Date = formatToDisplayDate(d);

                // ★ copy に入れる（item は触らない）
                copy["開催期間"] =
                    toJSTDateOnlyString(item["開始日"]) + "～" +
                    toJSTDateOnlyString(item["終了日"]);

                copy["イベントURL"] =
                    `http://192.168.10.67/items/${item["イベント番号"]}`;

                
                var workDayKey = dateKeyFromJSTDate(test);

                if (copy.Date === workDayKey && copy["ID"]) {
                    copy["勤務日"] = toJSTDisplayString(item["勤務日"]);
                    copy["勤務開始時間"] = toJSTDisplayString(item["勤務開始時間"]);
                    copy["勤務終了時間"] = toJSTDisplayString(item["勤務終了時間"]);
                    copy["担当者"] = item["担当者"];
                    copy["ID"] = item["ID"];

                    // ★ ここは copy["ID"] を使う
                    copy["イベント当日URL"] =
                    `http://192.168.10.67/items/${copy["ID"]}`;

                } else {
                    copy["勤務日"] = null;
                    copy["勤務開始時間"] = null;
                    copy["勤務終了時間"] = null;
                    copy["担当者"] = null;
                    copy["ID"] = null;
                    // copy["新規イベント当日"] =`http://192.168.10.67/items/${eventDaySiteId}/new?FromSiteId=${periodSiteId}&LinkId=${item["イベント店舗ID"]}&FromTabIndex=0`;
                    copy["イベント当日URL"] =`http://192.168.10.67/items/${eventDaySiteId}/new?FromSiteId=${periodSiteId}&LinkId=254586&FromTabIndex=0`;
                }

                out.push(copy);
            });


        } else {
        out.push(Object.assign({}, item));
        }
    });
    

    return out;
    }

    function toCheckJSTDate(dateLike) {
        var d = new Date(dateLike);
        if (isNaN(d)) return null;

        // JST (+9h)
        d.setHours(d.getHours() + 9);

        // YYYY-MM-DD 形式で返す
        return (
            d.getFullYear() + '-' +
            String(d.getMonth() + 1).padStart(2, '0') + '-' +
            String(d.getDate()).padStart(2, '0')
        );
    }


    //9時間プラス
    function toJSTDate(dateLike) {
        var d = new Date(dateLike);
        if (isNaN(d)) return null;
        d.setHours(d.getHours() + 9);
        return d;
    }


    //表示用に変換
    function toJSTDisplayString(dateLike) {
        var d = toJSTDate(dateLike);
        if (!d) return null;

        var y = d.getFullYear();
        var m = String(d.getMonth() + 1).padStart(2, '0');
        var day = String(d.getDate()).padStart(2, '0');
        var h = String(d.getHours()).padStart(2, '0');
        var min = String(d.getMinutes()).padStart(2, '0');

        // ★ 時刻を持っているか判定
        var hasTime = d.getHours() !== 0 || d.getMinutes() !== 0 || d.getSeconds() !== 0;

        if (hasTime) {
            // 勤務開始時間・勤務終了時間 用
            return `${y}/${m}/${day} ${h}:${min}`;
        } else {
            // 日付のみ 用
            return `${y}年${m}月${day}日`;
        }
    }

    //開催期間表示用
    function toJSTDateOnlyString(dateLike) {
        var d = toJSTDate(dateLike);
        if (!d) return null;

        return (
            d.getFullYear() + '年' +
            String(d.getMonth() + 1).padStart(2, '0') + '月' +
            String(d.getDate()).padStart(2, '0') + '日'
        );
        }

        function dateKeyFromJSTDate(d) {
        if (!(d instanceof Date) || isNaN(d.getTime())) return '';
        return (d.getMonth() + 1) + '月' + d.getDate() + '日';
    }


  /******************************************************************
   * HTML 作成（各セル／各列を作る関数群）
   * - buildCellElement: 1 セル（ヘッダ + 本文）を作る
   * - buildColumnElement: 1 レコード（複数セル）を作る
   *
   * これらは DOM を直接作る補助で、読みやすさを優先して分割しています。
   ******************************************************************/

  /**
   * buildCellElement
   * - data-style 属性、ヘッダ表示テキスト、本文を受け取りセルの DOM を返す
   * - bodyContent が文字列ならテキストとして追加、jQuery 要素ならそのまま追加
   */
  function buildCellElement(dataStyle, headText, bodyContent) {
    // 親のセル要素 <div class="sdt-table-variable-cell" data-style="...">
    var $cell = $('<div>').addClass('sdt-table-variable-cell').attr('data-style', dataStyle);

    // ヘッダ部
    var $head = $('<div>').addClass('sdt-table-variable-cell-head');
    $head.append($('<p>').addClass('sdt-table-variable-cell-head__text').text(headText));
    $cell.append($head);

    // 本文部
    var $body = $('<div>').addClass('sdt-table-variable-cell-body');
    var $bodyTextWrap = $('<p>').addClass('sdt-table-variable-cell-body__text');

    if (bodyContent && bodyContent instanceof jQuery) {
      // もしリンクなど jQuery 要素が渡されたらそれをそのまま入れる
      $bodyTextWrap.append(bodyContent);
    } else {
      // 文字列ならエスケープ（text() を使うことで HTML 挿入を防ぐ）
      $bodyTextWrap.text(String(bodyContent || ''));
    }

    $body.append($bodyTextWrap);
    $cell.append($body);

    return $cell;
  }

  /**
   * buildColumnElement
   * - 1 レコード（イベント）から「column」DOM を作ります。
   * - buildCellElement を使ってセルを順に追加するだけなので読みやすく少ない処理量にしています。
   *
   * 入力 item の主な期待プロパティ:
   *   Date, Category, Area, EventName, EventUrl, BusinessStore, EventPeriod, StartTime, EndTime, PersonCharge
   */
  function buildColumnElement(item) {

    var $col = $('<div>').addClass('sdt-table-variable-column');

    // 日付セル
    $col.append(buildCellElement('Date', '日付', item.Date || ''));

    // カテゴリ
    $col.append(buildCellElement('Category', 'カテゴリー', item["カテゴリ"] || ''));

    // エリア
    $col.append(buildCellElement('Area', '開催エリア', item["開催エリア"]  || ''));

    // エリア詳細
    $col.append(buildCellElement('AreaDetail', '開催エリア詳細', item["開催エリア詳細"]  || ''));

    // イベント名（リンク）
    var $link = $('<a>').addClass('sdt-link').attr('href', item["イベントURL"] || '#').text(item["イベント名称・出店先"] || 'イベント名称未設定');
    $col.append(buildCellElement('EventName', 'イベント名称・出店先', $link));

    // 事業名・店舗名
    $col.append(buildCellElement('BusinessStore', '事業名・店舗名', item["店舗名"] || ''));

    // 開催期間（元の文字列を表示）
    $col.append(buildCellElement('EventPeriod', '開催期間', item["開催期間"] || ''));

    // 勤務開始時間 / 勤務終了時間
    $col.append(buildCellElement('StartTime', '勤務開始時間', item["勤務開始時間"] || ''));
    $col.append(buildCellElement('EndTime', '勤務終了時間', item["勤務終了時間"] || ''));

    // 担当者
    $col.append(buildCellElement('PersonCharge', '担当者', item["担当者"] || ''));

    // 入力ボタン（リンク） - ページ遷移させたくないので preventDefault している
    var $inputA = $('<a>')
      .addClass('sdt-inputButton')
      .attr('href', item["イベント当日URL"] )
      .text('入力')
      .on('click', function (e) {
        // e.preventDefault();
        // ここで必要なら別の処理（モーダル表示等）を呼び出す
      });

    $col.append(buildCellElement('Button', '入力ボタン', $inputA));

    return $col;
  }

  /******************************************************************
   * 描画処理
   * - renderEvents: 生成した配列を #sdt-upcomingEvents-table の中に挿入する
   ******************************************************************/

  /**
   * renderEvents
   * - eventsArray が配列かをチェックし、挿入先要素がある場合は中身をクリアして
   *   配列の順番どおりに column 要素を追加します。
   *
   * 挿入先セレクタ:
   *   #sdt-upcomingEvents-table .sdt-table-variable .sdt-table-variable-wrap
   */
  function renderEvents(eventsArray) {
  if (!Array.isArray(eventsArray)) return;

  var $wrap = $('#sdt-upcomingEvents-table .sdt-table-variable .sdt-table-variable-wrap');
  if ($wrap.length === 0) return;

  var sorted = eventsArray.slice().sort(function (a, b) {

    function toComparableDate(item) {
      if (!item.Date) return null;

      // 「1月12日」形式
      var m = item.Date.match(/(\d+)月(\d+)日/);
      if (!m) return null;

      var month = Number(m[1]);
      var day   = Number(m[2]);

      // 開始日の年・月
      if (!item["開始日"]) return null;
      var start = new Date(item["開始日"]);
      var year = start.getFullYear();
      var startMonth = start.getMonth() + 1;

      // ★ 年またぎ補正（開始日が12月 & 表示月が1月）
      if (startMonth === 12 && month === 1) {
        year += 1;
      }

      return new Date(year, month - 1, day);
    }

    var da = toComparableDate(a);
    var db = toComparableDate(b);

    if (!da && !db) return 0;
    if (!da) return 1;
    if (!db) return -1;

    return da.getTime() - db.getTime();
  });

  // 描画
  $wrap.empty();

  sorted.forEach(function (item) {
    if (item["開始日"] && item["終了日"]) {
      $wrap.append(buildColumnElement(item));
    }
  });
}





  /******************************************************************
   * オーケストレーション関数（ボタン onclick で呼ばれる）
   *
   * - 入力取得 → alert 表示 → ダミーデータ取得 → 期間展開 → 描画
   * - シンプルで追いやすい順序にしています（初心者でもたどりやすい）
   ******************************************************************/
    //画面表示時デフォルト値表示
    $(function () {

        // 今日
        var today = new Date();

        // 6日後
        var end = new Date();
        end.setDate(today.getDate() + 6);

        // YYYY-MM-DD に整形
        function toYMD(d) {
            return (
            d.getFullYear() + '-' +
            String(d.getMonth() + 1).padStart(2, '0') + '-' +
            String(d.getDate()).padStart(2, '0')
            );
        }

        // 入力欄にセット
        $('#fn-startDate').val(toYMD(today));
        $('#fn-endDate').val(toYMD(end));

        // 初期表示用 dates
        var dates = {
            start: toYMD(today),
            end: toYMD(end)
        };

        // ★ 画面表示直後に実行
        loadEventsByDates(dates);
    });

    //検索クリック時
    async function upcomingEventsFormAction(evt) {
        if (evt && typeof evt.preventDefault === 'function') {
            evt.preventDefault();
        }

        // 入力値を取得
        var dates = getInputDates();

        // 確認用（必要なら）
        showDatesAlert(dates);

        // 共通処理を呼ぶ
        await loadEventsByDates(dates);
    }


    async function loadEventsByDates(dates) {
        try {
            const eventsArray = await fetchEventsDummy(dates);
            const expanded = expandEventsByPeriod(eventsArray, dates);

            console.log("expanded");
            console.log(expanded);

            renderEvents(expanded);
        } catch (err) {
            console.error('fetchEventsDummy failed', err);
        }
    }


  // グローバルに公開（onclick 属性で呼び出せるように）
  window.upcomingEventsFormAction = upcomingEventsFormAction;

  /*************************************************************************
   * 補足（使用方法）
   * - ボタンに: <button onclick="upcomingEventsFormAction(event)">実行</button>
   * - フォームに: <input id="fn-startDatet"> と <input id="fn-endDate"> を用意
   * - 描画先は: #sdt-upcomingEvents-table .sdt-table-variable .sdt-table-variable-wrap
   *
   * 変更・拡張ポイント（将来の実装メモ）
   * - fetchEventsDummy を $.ajax に差し替えて実データを取得する
   * - 日付形式のバリエーションが増えたら parsePeriod を拡張する
   *************************************************************************/

})(jQuery);
