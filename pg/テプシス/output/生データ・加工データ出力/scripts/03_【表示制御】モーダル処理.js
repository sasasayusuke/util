  (function($) {
    'use strict';

    /**
     * ModalManager
     * 単一ファイル内に収めたオーケストレーション実装。
     * - fn- 接頭辞のセレクタのみ参照
     * - 開閉・フォーカス管理・キーハンドリングを統括
     */
    const ModalManager = (function() {
      // ---- セレクタ定義（fn- プレフィックスのみを使う） ----
      const SELECTORS = {
        trigger: '.fn-open-modal',       // data-target を持つ要素（クリックで起動）
        modal: '.fn-modal',              // モーダルルート（data-modal 属性を読む）
        overlay: '.fn-modal-overlay',    // オーバーレイ（背景クリックで閉じる）
        closeBtn: '.fn-modal-close',      // 閉じるボタン
        startDate: '.fn-modal-start-date', // 開始日
        finishDate: '.fn-modal-finish-date' // 終了日
      };

      // フォーカス可能要素のセレクタ（フォーカストラップで利用）
      const FOCUSABLES = 'a[href], area[href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), iframe, [tabindex]:not([tabindex="-1"])';

      // 内部状態
      let $activeModal = null; // 現在開いているモーダルの jQuery オブジェクト
      let lastFocused = null;  // 開く前にフォーカスしていた要素（復帰に使う）

      // ----------------- ヘルパー関数 -----------------

      /**
       * findModalById
       * data-modal の値から該当モーダルを返す（無ければ空の jQuery）
       */
      function findModalById(id) {
        if (!id) return $();
        return $(`${SELECTORS.modal}[data-modal="${id}"]`);
      }

      /**
       * setModalOpenState
       * data-open 属性と aria-hidden を切り替える（スタイルは sdt- に任せる）
       */
      function setModalOpenState($modal, isOpen) {
        if (!$modal || !$modal.length) return;
        $modal.attr('data-open', isOpen ? 'true' : 'false');
        $modal.attr('aria-hidden', isOpen ? 'false' : 'true');
      }

      /**
       * getFocusableElements
       * モーダル内の表示されているフォーカス可能要素を返す
       */
      function getFocusableElements($modal) {
        return $modal.find(FOCUSABLES).filter(':visible');
      }

      /**
       * formatDateLocal
       * Date → yyyy-mm-dd（ローカル時間）
       */
      function formatDateLocal(date) {
        const y = date.getFullYear();
        const m = String(date.getMonth() + 1).padStart(2, '0');
        const d = String(date.getDate()).padStart(2, '0');
        return `${y}-${m}-${d}`;
      }

      // ----------------- キーハンドリング -----------------

      /**
       * handleKeydown
       * - ESC で閉じる
       * - TAB でフォーカストラップを実現
       */
      function handleKeydown(e) {
        if (!$activeModal) return;

        // ESC（閉じる）
        if (e.key === 'Escape' || e.keyCode === 27) {
          e.preventDefault();
          closeActiveModal();
          return;
        }

        // TAB（フォーカストラップ）
        if (e.key === 'Tab' || e.keyCode === 9) {
          const $focusables = getFocusableElements($activeModal);
          if (!$focusables.length) {
            e.preventDefault();
            return;
          }

          const first = $focusables.first()[0];
          const last = $focusables.last()[0];

          if (e.shiftKey) {
            // Shift + Tab
            if (document.activeElement === first) {
              e.preventDefault();
              last.focus();
            }
          } else {
            // Tab
            if (document.activeElement === last) {
              e.preventDefault();
              first.focus();
            }
          }
        }
      }


      // ----------------- 年月入力初期値 -----------------

      /**
       * setInitialDateValues
       * 開始日：当月初日
       * 終了日：翌月初日
       */
      function setInitialDateValues($modal) {
        const today = new Date();

        // 当月の初日
        const startDate = new Date(today.getFullYear(), today.getMonth(), 1);
        if (window.force) console.log(startDate);

        // 翌月の初日
        const finishDate = new Date(today.getFullYear(), today.getMonth() + 1, 1);
        if (window.force) console.log(finishDate);

        $modal.find(SELECTORS.startDate).val(formatDateLocal(startDate));
        $modal.find(SELECTORS.finishDate).val(formatDateLocal(finishDate));
      }

      // ----------------- 開閉ロジック -----------------

      /**
       * openModalById
       * 指定された id のモーダルを開く（data-modal と一致）
       * @param {String} id - data-modal の値
       * @param {jQuery|Element} $trigger - 開いたトリガー要素（フォーカス復帰用）
       */
      function openModalById(id, $trigger) {
        const $modal = findModalById(id);
        if (!$modal.length) return;

        // 現在アクティブなモーダルを上書き（シンプル実装では同時オープンは非対応）
        $activeModal = $modal;
        lastFocused = ($trigger && $trigger.length) ? $trigger : $(document.activeElement);

        // 状態を開くにセット
        setModalOpenState($modal, true);

        // 年月入力の初期値をセット
        setInitialDateValues($modal);

        // フォーカス移動：モーダル内の最初のフォーカス可能要素へ
        const $focusables = getFocusableElements($modal);
        if ($focusables.length) {
          $focusables.first().focus();
        } else {
          // フォーカス可能要素がなければモーダルルートに tabindex を付与してフォーカス
          $modal.attr('tabindex', '-1').focus();
        }

        // ドキュメントにキーイベントを登録（Esc / Tab）
        $(document).on('keydown.fnModal', handleKeydown);
      }

      /**
       * closeActiveModal
       * 現在開いているモーダルを閉じる（フォーカス復帰・イベント解除）
       */
      function closeActiveModal() {
        if (!$activeModal || !$activeModal.length) return;

        // 閉じる状態に切り替え
        setModalOpenState($activeModal, false);

        // フォーカスを元の要素に戻す（安全に）
        try {
          if (lastFocused && $(lastFocused).length) {
            $(lastFocused).focus();
          } else {
            document.body.focus();
          }
        } catch (err) {
          // フォーカス復帰で例外が出ても無視
        }

        // イベント解除と状態クリア
        $(document).off('keydown.fnModal');
        $activeModal = null;
        lastFocused = null;
      }

      // ----------------- イベントバインド（デリゲート） -----------------

      /**
       * bindGlobalEvents
       * - すべてのクリック監視をここで行う（動的に追加された要素にも対応）
       */
      function bindGlobalEvents() {
        // トリガークリック（data-target を読み、対応する data-modal を open）
        $(document).on('click.fnModal', SELECTORS.trigger, function(e) {
          e.preventDefault();
          const targetId = $(this).attr('data-target');
          if (!targetId) return;
          openModalById(targetId, $(this));
        });

        // overlay クリックで閉じる
        $(document).on('click.fnModal', SELECTORS.overlay, function(e) {
          // overlay 自体がクリックされたら閉じる（dialog 内のクリックは対象外）
          closeActiveModal();
        });

        // 閉じるボタン
        $(document).on('click.fnModal', SELECTORS.closeBtn, function(e) {
          e.preventDefault();
          closeActiveModal();
        });

        // ルートモーダルの余地クリック（ダイアログ外クリック）で閉じる
        $(document).on('click.fnModal', SELECTORS.modal, function(e) {
          const $root = $(this);
          const $dialog = $root.find('.sdt-modal__dialog').first();
          // クリック対象がダイアログの外側（＝背景領域）なら閉じる
          if ($dialog.length && !$dialog[0].contains(e.target)) {
            closeActiveModal();
          }
        });
      }

      // ----------------- 公開 API -----------------
      function init() {
        bindGlobalEvents();
      }

      return {
        init: init,
        open: openModalById,   // テスト用に外部から開ける（必要なければ使わない）
        close: closeActiveModal
      };
    })();

    // DOM Ready で初期化
    $(function() {
      ModalManager.init();
      // （オプション）開発中はここで自動的に開いて動作確認もできます:
      // ModalManager.open('modal-A');
    });
  })(jQuery);