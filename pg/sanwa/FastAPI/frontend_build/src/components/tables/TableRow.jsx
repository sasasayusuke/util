import React, { useCallback, memo, useRef, useEffect, useState,useMemo } from 'react';
import TableCell from './TableCell';

function TableRow({
  row,
  rowIndex,
  columns,
  tableRefs,
  onChangeCell,
  onFocusMove,
  focusedRow,
  onCellFocus,
  setFocusedRow,
  selectedView
}) {
  const isFocusedRow = (rowIndex === focusedRow);

  let backgroundClass = '';
  let textClass = '';

  if (row['U区分'] === 'R') {
    backgroundClass = 'bg-red-200';
    textClass       = 'text-black';
  } else if (row['U区分'] === 'U') {
    backgroundClass = 'bg-yellow-200';
    textClass       = 'text-black';
  } else if (row['U区分'] === 'B') {
    backgroundClass = 'bg-blue-200';
    textClass       = 'text-black';
  } else if (row['U区分'] === 'H') {
    backgroundClass = 'bg-gray-200';
    textClass       = 'text-black';
  } else {
    backgroundClass = rowIndex % 2 === 0
      ? 'bg-slate-50 hover:bg-blue-100'
      : 'bg-zinc-100 hover:bg-blue-100';
    textClass = 'text-black';
  }

  if (row['CHECK']) {
    textClass = 'text-blue-500';
  }

const rowFocusClass = isFocusedRow
  ? 'border-4 border-blue-500'
  : 'border border-gray-300';

  return (
    <tr className={`
      ${backgroundClass}
      ${textClass}
      ${rowFocusClass}
      transition-colors duration-150
    `}>
      {columns.map((col, colIndex) => (
        <TableCell
          key={`${rowIndex}_${col["accessor"]}`}
          // key={`${rowIndex}_${colIndex}`}
          row={row}
          col={col}
          rowIndex={rowIndex}
          colIndex={colIndex}
          tableRefs={tableRefs}
          onChangeCell={onChangeCell}
          onFocusMove={onFocusMove}
          onCellFocus={() => {
            setFocusedRow(rowIndex);
            if (onCellFocus) {
              onCellFocus(col);
            }
          }}
        />
      ))}
    </tr>
  );
}

export default memo(TableRow , (prevProps, nextProps) => {
  // rowデータ自体に変化がない && フォーカス状態が変化していない
  const prevFocused = prevProps.focusedRow === prevProps.rowIndex;
  const nextFocused = nextProps.focusedRow === nextProps.rowIndex;

  return (
    prevProps.row === nextProps.row &&
    prevFocused === nextFocused &&
    prevProps.columns === nextProps.columns
  );
});
