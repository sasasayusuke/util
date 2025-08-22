// src/components/tables/TableCell.jsx
import React, { useEffect, useRef, useState } from 'react';

const decimal_point = 100;
// 数値をカンマ区切りでフォーマットする関数
function formatNumber(value,max,min) {
  if (value === null || value === undefined || value == 0 || isNaN(value)) return '';
  if(max != "" && value >= max){
    alert('最大値を超えています。');
    return '0';
  }
  if(min != "" && value <= min){
    alert('最小値を超えています。');
    return '0';
  }
  value = Math.floor((value * decimal_point)) / decimal_point;
  return value.toLocaleString();
  // return new Intl.NumberFormat('ja-JP').format(value);
}

/**
 * align が指定されていればそのクラスを、なければ type に応じたデフォルトのクラスを返す
 */
function getAlignmentClass(type, align) {
  // align が指定されていればそれを使用
  if (align) {
    switch (align) {
      case 'center':
        return 'text-center';
      case 'right':
        return 'text-right';
      default:
        return 'text-left';
    }
  }

  // align が指定されていない場合は type に応じたデフォルトを使用
  switch (type) {
    case 'checkbox':
      return 'text-center';
    case 'number':
      return 'text-right';
    default:
      return 'text-left';
  }
}

function TableCell({
  col,              // セルに関する情報（accessor, type, align, editable, values など）
  row,              // 各行のデータオブジェクト
  rowIndex,         // 現在の行番号
  colIndex,         // 現在の列番号
  tableRefs,        // 親から渡された、セルの DOM 要素を保持する参照オブジェクト
  onChangeCell,     // セルの値が変更されたときに呼ぶコールバック
  onFocusMove,      // 矢印キー移動時に呼び出すコールバック
  onCellFocus,      // セルにフォーカスがあたったときに呼ぶコールバック（例：行のハイライト用）
}) {
  const { accessor, type, align, editable, values = [],length = "" ,max = "", min = ""} = col;
  // 現在のセルの値（デフォルトは空文字）
  const cellValue = row[accessor] ?? '';
  // 基本の <td> 用クラス
  const tdBaseClass = 'border border-gray-300 px-1';
  const alignmentClass = getAlignmentClass(type, align);

  // 入力要素への参照
  const inputRef = useRef(null);

  // マウント時に tableRefs に登録
  useEffect(() => {
    if (!tableRefs.current[rowIndex]) {
      tableRefs.current[rowIndex] = [];
    }
    tableRefs.current[rowIndex][colIndex] = inputRef.current;
  }, [rowIndex, colIndex, tableRefs]);


  // 矢印キー移動用のハンドラ
  function handleKeyDown(e) {
    switch (e.key) {
      case 'ArrowLeft':
        e.preventDefault();
        onFocusMove(rowIndex, colIndex, 'left');
        break;
      case 'ArrowRight':
        e.preventDefault();
        onFocusMove(rowIndex, colIndex, 'right');
        break;
      case 'ArrowUp':
        e.preventDefault();
        onFocusMove(rowIndex, colIndex, 'up');
        break;
      case 'ArrowDown':
        e.preventDefault();
        onFocusMove(rowIndex, colIndex, 'down');
        break;
      default:
        break;
    }
  }

  // 共通のフォーカスハンドラ
  function handleFocus(e) {
    if (onCellFocus) onCellFocus();
    // テキスト全体を選択する
    if (e.target && typeof e.target.select === 'function') {
      e.target.select();
    }
  }

  // 編集可能な場合の処理
  if (editable) {
    switch (type) {
      // (1) チェックボックスの場合
      case 'checkbox':
        return (
          <td className={`${tdBaseClass} ${alignmentClass}`}>
            <input
              ref={inputRef}
              type="checkbox"
              className="h-4 w-4"
              checked={!!cellValue}
              onChange={(e) => onChangeCell(rowIndex, accessor, e.target.checked)}
              onFocus={handleFocus}
              onKeyDown={handleKeyDown}
              tabIndex={0}
            />
          </td>
        );
      // (2) 数値の場合（フォーカス時はカンマ除去、ブラー時にフォーマット済み表示）
      case 'number': {
        const [displayValue, setDisplayValue] = useState(formatNumber(cellValue,max));

        useEffect(() => {
          const tmp = cellValue.toString().replace(/,/g,'');
          setDisplayValue(formatNumber(parseFloat(tmp),max));
        }, [cellValue]);

        function handleInputFocus(e) {
          handleFocus(e);
          // 表示中のカンマを除去して生の数字文字列を設定
          const rawValue = cellValue.toString().replace(/,/g, '');
          const value = parseFloat(rawValue);

          setDisplayValue(isNaN(value) || value == 0 ? "":value);
        }

        function handleInputBlur(e) {
          const rawStr = e.target.value.replace(/,/g, '');
          const numericVal = parseFloat(rawStr);
          console.log('numeric',numericVal)
          if (!isNaN(numericVal)) {
            onChangeCell(rowIndex, accessor, formatNumber(numericVal,max,min));
            setDisplayValue(formatNumber(numericVal,max,min));
          } else {
            onChangeCell(rowIndex, accessor, 0);
            setDisplayValue('');
          }
        }

        return (
          <td className={`${tdBaseClass} ${alignmentClass} p-1`}>
            <div className="w-full overflow-hidden whitespace-nowrap p-1">
              <input
                ref={inputRef}
                type="text"
                className={`
                  w-full px-2
                  border rounded-md
                  bg-transparent
                  focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
                  text-sm
                  ${alignmentClass}`
                }
                value={displayValue}
                onFocus={handleInputFocus}
                onBlur={handleInputBlur}
                onChange={(e) => setDisplayValue(e.target.value)}
                onKeyDown={handleKeyDown}
                tabIndex={0}
              />
            </div>
          </td>
        );
      }
      // (3) テキストの場合
      default: {
        const [textValue, setTextValue] = useState(cellValue);

        useEffect(() => {
          setTextValue(cellValue);
        }, [cellValue]);

        function handleTextChange(e) {
          setTextValue(e.target.value);
        }

        function handleBlur() {
          if (textValue && values.length > 0 && !values.includes(textValue)) {
            // 候補にない場合は元の値に戻す
            setTextValue(cellValue);
          } else {
            onChangeCell(rowIndex, accessor, textValue);
          }
        }

        return (
          <td className={`${tdBaseClass} ${alignmentClass} p-1`}>
            <div className="w-full overflow-hidden whitespace-nowrap p-1">
              <input
                ref={inputRef}
                type="text"
                className="
                  w-full px-2
                  border rounded-md
                  bg-transparent
                  focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
                  text-sm
                "
                value={textValue}
                onChange={handleTextChange}
                onBlur={handleBlur}
                onFocus={handleFocus}
                onKeyDown={handleKeyDown}
                tabIndex={0}
                maxLength={length}
              />
            </div>
          </td>
        );
      }
    }
  }

  switch (type) {
    case 'checkbox':
      return (
        <td className={`${tdBaseClass} ${alignmentClass}`}>
          <input
            type="checkbox"
            className="h-4 w-4"
            checked={!!cellValue}
            readOnly
          />
        </td>
      );
    case 'number':
      return (
        <td className={`${tdBaseClass} ${alignmentClass} overflow-hidden whitespace-nowrap  text-sm`}>
          {formatNumber(cellValue)}
        </td>
      );
    default:
      return (
        <td className={`${tdBaseClass} ${alignmentClass} overflow-hidden whitespace-nowrap  text-sm`}>
          {cellValue}
        </td>
      );
  }
}


export default React.memo(TableCell, (prev, next) => {
  const prevVal = prev.row[prev.col["accessor"]];
  const nextVal = next.row[next.col["accessor"]];

  
  // if (prev.colIndex !== next.colIndex) return false;
  if(!('re-render' in prev.col))return true;

  // CHECK列はここで検出
  return (
    prevVal === nextVal && 
    prev.onChangeCell === next.onChangeCell
  );
});