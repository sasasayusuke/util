// FunctionKeys.jsx
import React, { useEffect } from 'react';
import KeyButton from './KeyButton';

/**
 * FunctionKeys コンポーネント
 *
 * @param {Object} props
 * @param {Object} props.keyConfig - 状態ごとのキー設定オブジェクト
 * @param {string} props.currentState - 現在の状態
 */
export default function FunctionKeys({ keyConfig, currentState }) {
  // 現在の状態に応じたキー設定を取得する
  // keyConfig の中から currentState に対応するオブジェクトを抽出する
  const actions = keyConfig[currentState] || {};

  // ----------------------------
  // キーボードイベントの処理
  // ----------------------------
  // キーが押されたとき、対応するアクション（onClick）を実行する
  function handleKeyDown(e) {
    switch (e.key) {
      case 'F1':
        if (actions.F1 && actions.F1.onClick) {
          e.preventDefault();
          actions.F1.onClick();
        }
        break;
      case 'F2':
        if (actions.F2 && actions.F2.onClick) {
          e.preventDefault();
          actions.F2.onClick();
        }
        break;
      case 'F3':
        if (actions.F3 && actions.F3.onClick) {
          e.preventDefault();
          actions.F3.onClick();
        }
        break;
      case 'F4':
        if (actions.F4 && actions.F4.onClick) {
          e.preventDefault();
          actions.F4.onClick();
        }
        break;
      case 'F5':
        if (actions.F5 && actions.F5.onClick) {
          e.preventDefault();
          actions.F5.onClick();
        }
        break;
      case 'F6':
        if (actions.F6 && actions.F6.onClick) {
          e.preventDefault();
          actions.F6.onClick();
        }
        break;
      case 'F7':
        if (actions.F7 && actions.F7.onClick) {
          e.preventDefault();
          actions.F7.onClick();
        }
        break;
      case 'F8':
        if (actions.F8 && actions.F8.onClick) {
          e.preventDefault();
          actions.F8.onClick();
        }
        break;
      case 'F9':
        if (actions.F9 && actions.F9.onClick) {
          e.preventDefault();
          actions.F9.onClick();
        }
        break;
      case 'F10':
        if (actions.F10 && actions.F10.onClick) {
          e.preventDefault();
          actions.F10.onClick();
        }
        break;
      case 'F11':
        if (actions.F11 && actions.F11.onClick) {
          e.preventDefault();
          actions.F11.onClick();
        }
        break;
      case 'F12':
        if (actions.F12 && actions.F12.onClick) {
          e.preventDefault();
          actions.F12.onClick();
        }
        break;
      // 環境により Space キーの表記が異なるため、複数のパターンでチェック
      case ' ':
      case 'Spacebar':
      case 'Space':
        if (actions.Space && actions.Space.onClick) {
          e.preventDefault();
          actions.Space.onClick();
        }
        break;
      default:
        break;
    }
  }

  // コンポーネントのマウント時にキーボードイベントのリスナーを追加し、アンマウント時に解除する
  useEffect(() => {
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [currentState, keyConfig]);

  // ----------------------------
  // 各キーのボタンをレンダリングする補助関数
  // ----------------------------
  /**
   * renderButton 関数
   *
   * @param {string} keyLabel - キーの名称（例："F1", "Space" など）
   * @param {Object} action - キーに対応するアクションオブジェクト
   * @param {string} [buttonSize="w-20"] - ボタンサイズを指定するクラス名
   * @returns JSX 要素
   */
  function renderButton(keyLabel, action, buttonSize = "w-20") {
    return (
      <div key={keyLabel} className="flex flex-col items-center">
        {/* キーのラベル表示 */}
        <span className="text-xs mb-1">{keyLabel}</span>
        {/* KeyButton コンポーネントにアクションとボタンサイズを渡す */}
        <KeyButton action={action} buttonSize={buttonSize} disable={action?.disable} />
      </div>
    );
  }

  // ----------------------------
  // コンポーネントのレンダリング
  // ----------------------------
  return (
    <div className="flex space-x-2">
      {/* F1～F4 のボタン */}
      {renderButton("F1", actions.F1)}
      {renderButton("F2", actions.F2)}
      {renderButton("F3", actions.F3)}
      {renderButton("F4", actions.F4)}

      {/* F4 と F5 の間にスペース */}
      <div className="w-8" />

      {/* F5～F8 のボタン */}
      {renderButton("F5", actions.F5)}
      {renderButton("F6", actions.F6)}
      {renderButton("F7", actions.F7)}
      {renderButton("F8", actions.F8)}

      {/* F8 と F9 の間にスペース */}
      <div className="w-8" />

      {/* F9～F12 のボタン */}
      {renderButton("F9", actions.F9)}
      {renderButton("F10", actions.F10)}
      {renderButton("F11", actions.F11)}
      {renderButton("F12", actions.F12)}

      {/* F12 と Space の間にスペース */}
      <div className="w-12" />

      {/* Space キーのボタン（大きめのサイズ指定） */}
      {renderButton("Space", actions.Space, "w-48")}
    </div>
  );
}
