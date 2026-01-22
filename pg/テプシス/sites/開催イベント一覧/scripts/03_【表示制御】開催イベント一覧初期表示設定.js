(function ($) {
  'use strict';

  /************************************************************************
   * 画面読み込み時に一度だけ実行するレイアウト処理
   *
   * 目的：
   *  - ヘッダー（#Header）の高さを取得して、その高さ分だけ
   *    #sdt-upcomingEvents の top をずらします（重なり防止）。
   *  - さらに #sdt-upcomingEvents の高さを CSS の calc() を使って
   *    calc(100% - {headerHeight}px) に設定します。これにより、
   *    ビューポート全体の高さからヘッダー分を差し引いた高さにできます。
   *
   * 注意：
   *  - この処理は「読み込み時に一度だけ実行」されます。画面サイズ変更で追従
   *    させたい場合は下の「リサイズ追従（任意）」のコメントを参考に有効化してください。
   *  - #sdt-upcomingEvents に top を効かせるには、その要素に適切な position（relative/absolute/fixed/sticky）
   *    が CSS で設定されている必要があります。もし反映されない場合は CSS 側を確認してください。
   ************************************************************************/

  /**
   * getHeaderHeight
   * - #Header 要素の「見た目の高さ」および .both.cf 要素の高さをピクセル単位で合算し、
   *   さらに 10px を加えた合計を返します。
   * - 見た目の高さとは、実際に画面に描画された高さ（border・padding を含むサイズ）です。
   * - 要素が見つからなければ 0 を返します（呼び出し側で扱いやすくするため）。
   *
   * 基本の考え方（初心者向け）：
   *  - jQuery の .height() は「コンテンツ高さ」を返しますが、
   *    getBoundingClientRect().height は表示上の高さ（細かい描画情報）を返すので、
   *    見た目を合わせたい時はこちらを使うことが多いです。
   *
   * 追加仕様：
   *  - .both.cf の高さ（存在する場合）を加算します（最初に見つかった要素を使用）。
   *  - 上記合計に 10px を追加で上乗せします。
   *
   * @returns {number} headerHeightPx - 整数ピクセル（例: 84）
   */
  function getHeaderHeight() {
    // jQuery で要素を取得
    var $header = $('#Header');

    // 要素が無ければ 0 を返す（安全なフォールバック）
    var headerHeight = 0;
    if ($header.length !== 0) {
      // ネイティブ DOM の getBoundingClientRect() で「見た目の高さ」を取得
      var rect = $header[0].getBoundingClientRect();
      // 小数点が乗ることがあるため四捨五入して整数にする（CSS に入れやすくするため）
      headerHeight = Math.round(rect.height || 0);
      if (headerHeight < 0) {
        headerHeight = 0;
      }
    }

    // .both.cf 要素の高さを取得（最初に見つかった要素を対象）
    var bothHeight = 0;
    var $both = $('.both.cf');
    if ($both.length !== 0) {
      try {
        var bothRect = $both[0].getBoundingClientRect();
        bothHeight = Math.round(bothRect.height || 0);
        if (bothHeight < 0) {
          bothHeight = 0;
        }
      } catch (e) {
        // 何らかの理由で取得できなかった場合は 0 とする（安全対策）
        bothHeight = 0;
        window.force && console.error('.both.cf height read error', e);
      }
    }

    // 合計に 10px を上乗せする
    var total = headerHeight + bothHeight + 10;

    // 最終的に不正値にならないようにガードして返す
    return (total > 0) ? total : 0;
  }

  /**
   * buildCalcHeightCss
   * - ヘッダーのピクセル値を受け取り、CSS の calc() 文字列を返します。
   * - 例: px=50 -> "calc(100% - 50px)"
   *
   * @param {number} px - ヘッダー高さ（ピクセル）
   * @returns {string} CSS の値（例: "calc(100% - 50px)"）
   */
  function buildCalcHeightCss(px) {
    // 安全のため数値に変換してから文字列を生成
    var n = (typeof px === 'number' && !isNaN(px)) ? px : 0;
    return 'calc(100% - ' + String(n) + 'px)';
  }

  /**
   * applyUpcomingEventsLayout
   * - 実際に DOM に対して style を当てる関数（書き込みのみ）
   * - headerHeightPx が数値で与えられたら top と height を設定します。
   * - 要素が無ければ何もしません（無害で安全）。
   *
   * @param {number} headerHeightPx - ヘッダーの高さ（ピクセル）
   */
  function applyUpcomingEventsLayout(headerHeightPx) {
    // 対象要素を取得
    var $target = $('#fn-upcomingEvents');

    // 要素が無ければ早期リターン（処理を止めない）
    if ($target.length === 0) {
      return;
    }

    // top をピクセルで設定（例: "64px"）
    $target.css('top', String(headerHeightPx) + 'px');

    // height を calc(100% - {headerHeight}px) に設定
    $target.css('height', buildCalcHeightCss(headerHeightPx));
  }

  /**
   * initUpcomingEventsLayout
   * - 画面読み込み時に呼ばれるオーケストレーション関数
   * - 役割: ヘッダー高さを取得して、対象要素にレイアウトを適用する（1回だけ）
   */
  function initUpcomingEventsLayout() {
    // ヘッダーの高さを取得（例: 64）
    var headerH = getHeaderHeight();

    // 取得した高さを使って対象要素にスタイルを適用
    applyUpcomingEventsLayout(headerH);
  }

  // DOMContentLoaded のタイミングで一度だけ実行する
  document.addEventListener('DOMContentLoaded', function () {
    try {
      initUpcomingEventsLayout();
    } catch (e) {
      // エラーが出たときは開発用に console に出す（運用では消してもよい）
      // window.force を true にすると詳細ログが出ます（デバッグ用）
      window.force && console.error('initUpcomingEventsLayout error', e);
    }
  });

  /* --------------------------------------------------------------------
   * リサイズ時に高さを再計算して追従させたい場合（任意）
   *
   * - 画面サイズが変わるとヘッダーの高さが変化する UI がある場合は、
   *   下のコードを有効化してください。
   * - 最小限の実装を心がけるためデフォルトでは無効にしています。
   *
   * // window.addEventListener('resize', function () {
   * //   try {
   * //     initUpcomingEventsLayout();
   * //   } catch (e) {
   * //     window.force && console.error('resize initUpcomingEventsLayout error', e);
   * //   }
   * // });
   * ------------------------------------------------------------------ */

  /* --------------------------------------------------------------------
   * 補足（初心者向け Q&A）
   *
   * Q: なぜ top を設定するだけで表示がずれるの？
   * A: top は要素が position: absolute/fixed/sticky/relative の時に意味を持ちます。
   *    もし #sdt-upcomingEvents に position が無い（static）場合、top は効きません。
   *    その場合は CSS に position: relative; などを設定してください。
   *
   * Q: calc(100% - 50px) は何を意味するの？
   * A: 親要素の高さやビューポート高さを基準に 100% から 50px を引いた高さを意味します。
   *    ここでは「画面全体の高さ - ヘッダーの高さ」を意図しています。
   *
   * Q: この処理を手動で再実行する方法は？
   * A: このファイルは外部公開しない方針なので直接呼べる関数は置いていません。
   *    もし手動で呼びたい場合は initUpcomingEventsLayout を global に公開してください。
   * ------------------------------------------------------------------ */

})(jQuery);
