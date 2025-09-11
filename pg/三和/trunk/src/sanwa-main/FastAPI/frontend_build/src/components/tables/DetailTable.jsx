// src/components/tables/DetailTable.jsx
import React, { useEffect, useMemo, useState } from 'react';
import RadioBar from '../fields/RadioBar';
import SearchBar from '../fields/SearchBar';
import TableRow from './TableRow';

export default function DetailTable({
  columns = [],
  data = [],
  searchColumnLabel,
  viewRadioLabel,
  onDataChange,
  onCellFocus,
  onSearchFocus,
  onSearchBlur,
  focusedRow,
  setFocusedRow,
  tableRefs
}) {
  // 検索用テキスト
  const [searchText, setSearchText] = useState('');

  // columns から重複を除いたラジオボタンの選択肢を生成
  const generatedRadioItems = useMemo(() => {
    const viewSet = new Set();
    columns.forEach(col => {
      if (col.views && Array.isArray(col.views)) {
        col.views.forEach(view => viewSet.add(view));
      }
    });
    return Array.from(viewSet).map(value => ({
        value,
        label: value.toUpperCase()
      }));
  }, [columns]);

  // 現在表示すべき View の state（ラジオボタン用）
  const [selectedView, setSelectedView] = useState(
    generatedRadioItems[0]?.value ?? null
  );


  // 各列に hidden フラグを付与（selectedView に含まれない列は hidden:true）
  // const columnsWithHiddenFlag = useMemo(() => {
  //   return columns.map(col => {
  //     if (!col.views){
  //       tmp = col;
  //       tmp["hidden"] = false;
  //       return tmp;
  //     }
  //     const shouldShow = col.views.includes(selectedView);
  //     return { ...col, hidden: !shouldShow };
  //   });
  // }, [columns, selectedView]);


  // // 表示すべき列のみ抽出
  // const visibleColumns = useMemo(() => {
  //   const tmp = columnsWithHiddenFlag.filter(col => !col.hidden);
  //   return tmp;
  // }, [columnsWithHiddenFlag]);

  const visibleColumns = useMemo(() => {
    return view_list[selectedView];
  }, [selectedView]);


  // ラジオボタンの選択変更時のハンドラ
  function handleChangeRadio(newValue) {
    setSelectedView(newValue);
    // 上から渡した関数を動かす
    console.log('View selected:', newValue);
  }


  // セルの値変更時の処理（更新対象行のみ生成）
  function handleCellChange (rowIndex, accessor, newValue) {
    const updatedData = data.map((row, index) =>
      index === rowIndex ? { ...row, [accessor]: newValue } : row
    );
    if (onDataChange) {
      onDataChange(rowIndex, accessor, updatedData);
    }
  }

  /**
   * キーボードの矢印キーなどによるセル移動のハンドラ
   * @param {number} rowIndex 現在の行番号
   * @param {number} colIndex 現在の列番号（visibleColumns のインデックス）
   * @param {string} direction 'up' | 'down' | 'left' | 'right'
   */
  function handleFocusMove(rowIndex, colIndex, direction) {
    let newRowIndex = rowIndex;
    let newColIndex = colIndex;
    const maxRow = data.length;
    const maxCol = visibleColumns.length;

    // 最大 (row × col) 回まで試行
    for (let i = 0; i < maxRow * maxCol; i++) {
      switch (direction) {
        case 'left':
          newColIndex--;
          if (newColIndex < 0) return; // 左端
          break;
        case 'right':
          newColIndex++;
          if (newColIndex >= maxCol) return; // 右端
          break;
        case 'up':
          newRowIndex--;
          if (newRowIndex < 0) return; // 先頭
          break;
        case 'down':
          newRowIndex++;
          if (newRowIndex >= maxRow) return; // 最終
          break;
        default:
          return;
      }
      // editable なセルにフォーカスを移す
      if (visibleColumns[newColIndex]?.editable) {
        tableRefs.current[newRowIndex][newColIndex]?.focus();
        return;
      }
    }
  };

  // Enter キー押下で検索実行
  function handleSearchKeyDown(e) {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleSearch();
    }
  }

  // 検索のロジック
  function handleSearch() {
    if (!searchText) {
      alert('検索文字を入力してください。');
      return;
    }
    // visibleColumns 内から検索対象の列を特定
    const searchColIndex = visibleColumns.findIndex(
      col => col.label === searchColumnLabel
    );
    if (searchColIndex === -1) {
      alert('検索対象列が見つかりません。');
      return;
    }

    const startRow = focusedRow === null ? 0 : focusedRow + 1;
    // 現在位置以降を検索
    for (let i = startRow; i < data.length; i++) {
      const accessor = visibleColumns[searchColIndex].accessor;
      const cellValue = data[i][accessor];
      if (cellValue && cellValue.toString().includes(searchText)) {
        tableRefs.current[i][searchColIndex]?.focus();
        setFocusedRow(i);
        return;
      }
    }
    // 先頭から現在位置まで検索
    if (startRow > 0) {
      for (let i = 0; i < startRow; i++) {
        const accessor = visibleColumns[searchColIndex].accessor;
        const cellValue = data[i][accessor];
        if (cellValue && cellValue.toString().includes(searchText)) {
          tableRefs.current[i][searchColIndex]?.focus();
          setFocusedRow(i);
          return;
        }
      }
    }

    // 全く見つからなかった場合
    alert('[DB-004] 該当データは存在しません。');
  }


  return (
    <div className="border rounded-md p-2">
      {/* テーブル本体 */}
      <div className="w-full overflow-x-auto">
        {/* 高さを固定したい場合などは max-h-96 を使用 */}
        <div className="relative  overflow-y-auto   max-h-[calc(100vh-270px)]">
          {/* ここで key に selectedView を指定することで、view 変更時にテーブル全体が再マウントされます */}
          <table key={'test'} className="border-collapse table-fixed w-full" style={{ tableLayout: 'fixed' }}>
          {/* <table key={selectedView} className="border-collapse table-fixed w-full" style={{ tableLayout: 'fixed' }}> */}
            <colgroup>
              {visibleColumns.map((col, idx) => (
                <col key={idx} style={{ width: col.width || 'auto' }} />
              ))}
            </colgroup>

            <thead className="sticky top-0 bg-indigo-600 shadow-xl text-white z-10 font-medium">
              <tr>
                {visibleColumns.map((col, idx) => (
                  <th key={idx} className="p-2 border border-gray-300 text-xs font-medium">
                    {col.label}
                  </th>
                ))}
              </tr>
            </thead>

            <tbody>
              {data.map((row, rowIndex) => {
                return (
                  <TableRow
                    key={rowIndex}
                    row={row}
                    rowIndex={rowIndex}

                    // 表示すべき列のみを渡す
                    columns={visibleColumns}
                    tableRefs={tableRefs}
                    focusedRow={focusedRow}
                    onChangeCell={handleCellChange}
                    onFocusMove={handleFocusMove}
                    onCellFocus={onCellFocus}
                    setFocusedRow={setFocusedRow}
                    selectedView={selectedView}
                  />
                );
              })}
            </tbody>
          </table>
        </div>
      </div>

      {/* 検索バーとラジオボタン */}
      <div className="flex items-center justify-between mt-4">
      {searchColumnLabel && (
        <SearchBar
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
          onKeyDown={handleSearchKeyDown}
          onSearch={handleSearch}
          onFocus={onSearchFocus}
          onBlur={onSearchBlur}
          placeholder={`${searchColumnLabel}検索`}
        />
      )}
      {generatedRadioItems.length > 0 && (
        <RadioBar
          title={viewRadioLabel}
          items={generatedRadioItems}
          selectedValue={selectedView}
          onChange={handleChangeRadio}
        />
      )}
      </div>
    </div>
  );
}
