(function ($) {
  'use strict';

  /************************************************************************
   * 画面読み込み時に一度だけ実行するレイアウト処理
   *
   * 目的：
   *  - ヘッダー（#Header）の高さを取得して、その高さ分だけ
   *    #fn-kitchen-car の top をずらします（重なり防止）。
   *  - さらに #fn-kitchen-car の高さを CSS の calc() を使って
   *    calc(100% - {headerHeight}px) に設定します。
   *
   * 注意：
   *  - この処理は「読み込み時に一度だけ実行」されます。
   *  - #fn-kitchen-car に top を効かせるには position が必要です。
   ************************************************************************/

  /**
   * getHeaderHeight
   * - #Header 要素の「見た目の高さ」をピクセル単位で返します。
   * - 要素が見つからなければ 0 を返します。
   *
   * @returns {number} headerHeightPx - 整数ピクセル
   */
  function getHeaderHeight() {
    var $header = $('#Header');

    if ($header.length === 0) {
      return 0;
    }

    var rect = $header[0].getBoundingClientRect();
    var h = Math.round(rect.height || 0);

    return (h > 0) ? h : 0;
  }

  /**
   * buildCalcHeightCss
   * - ヘッダーのピクセル値を受け取り、CSS の calc() 文字列を返します。
   *
   * @param {number} px - ヘッダー高さ（ピクセル）
   * @returns {string} CSS の値
   */
  function buildCalcHeightCss(px) {
    var n = (typeof px === 'number' && !isNaN(px)) ? px : 0;
    return 'calc(100% - ' + String(n) + 'px)';
  }

  /**
   * applyKitchenCarLayout
   * - 実際に DOM に対して style を当てる関数
   *
   * @param {number} headerHeightPx - ヘッダーの高さ（ピクセル）
   */
  function applyKitchenCarLayout(headerHeightPx) {
    var $target = $('#fn-kitchen-car');

    if ($target.length === 0) {
      return;
    }

    $target.css('top', String(headerHeightPx) + 'px');
    $target.css('height', buildCalcHeightCss(headerHeightPx));
  }

  /**
   * initKitchenCarLayout
   * - 画面読み込み時に呼ばれるオーケストレーション関数
   */
  function initKitchenCarLayout() {
    var headerH = getHeaderHeight();
    applyKitchenCarLayout(headerH);
  }

  // DOMContentLoaded のタイミングで一度だけ実行する
  document.addEventListener('DOMContentLoaded', function () {
    try {
      initKitchenCarLayout();
    } catch (e) {
      window.force && console.error('initKitchenCarLayout error', e);
    }
  });

})(jQuery);
