// KeyButton.js
import React from 'react';

export default function KeyButton({
  action,       // ボタンに紐付けるアクション（オブジェクトまたは配列）
  currentState, // 現在の状態
  buttonSize = "w-20",
  backColor = "bg-blue-500",
  hoverColor = "bg-blue-600",
  textSize = "text-xs",
  textColor = "text-white",
  disable = false, // 追加: 外部から受け取る「強制的に無効化したい」フラグ
}) {
  // ----------------------------
  // action.state が配列でなければ 1 要素の配列に変換
  // ----------------------------
  const normalizeState = (stateValue) =>
    Array.isArray(stateValue) ? stateValue : [stateValue];

  // ----------------------------
  // 現在の状態に適合しているか判定
  // ----------------------------
  const checkActionEnabled = (action) => {
    if (!action) return false;
    if (!action.state) return true;

    const stateArr = normalizeState(action.state);
    return stateArr.includes(currentState);
  };

  // ----------------------------
  // アクションのラベル取得
  // ----------------------------
  const getActionLabel = (action) => {
    if (!action) return '';
    if (Array.isArray(action)) {
      return action.map(a => a.label).join(' / ');
    } else {
      return action.label || '';
    }
  };

  // ----------------------------
  // アクションの onClick 実行
  // ----------------------------
  const handleActionClick = (action) => {
    if (!action) return;

    // 配列の場合はすべての要素に対して onClick を呼ぶ
    if (Array.isArray(action)) {
      action.forEach(a => {
        const stateArr = normalizeState(a.state);
        if (stateArr.includes(currentState) && typeof a.onClick === 'function') {
          a.onClick();
        }
      });
    } else {
      // 単一アクション
      const stateArr = normalizeState(action.state);
      if (stateArr.includes(currentState) && typeof action.onClick === 'function') {
        action.onClick();
      }
    }
  };

  // ----------------------------
  // 外部から disable={true} が渡されたら強制的に無効化
  // + アクションの state が現在の state と合致しなければ無効化
  // ----------------------------
  const actionEnabledByState = checkActionEnabled(action);
  const isEnabled = (actionEnabledByState && !disable);

  // 有効時のみラベル表示
  const label = isEnabled ? getActionLabel(action) : "";

  // ----------------------------
  // ボタンのスタイルクラス
  // ----------------------------
  const baseClasses = `flex items-center justify-center ${buttonSize} h-8 rounded-md transition-all duration-200`;
  const enabledClasses = `${backColor} hover:${hoverColor} ${textColor} active:scale-95`;
  const disabledClasses = "bg-gray-300 text-gray-500 cursor-not-allowed";
  const buttonClasses = `${baseClasses} ${isEnabled ? enabledClasses : disabledClasses}`;

  return (
    <button
      type="button"
      // isEnabled の場合のみクリックイベントを有効に
      onClick={isEnabled ? () => handleActionClick(action) : undefined}
      disabled={!isEnabled}
      className={buttonClasses}
    >
      {/* label を小さく下に寄せるために mb-1 */}
      <span className={`${textSize} mb-1`}>{label}</span>
    </button>
  );
}

