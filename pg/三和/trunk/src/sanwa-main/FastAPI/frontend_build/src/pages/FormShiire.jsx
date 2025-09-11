import React, { useEffect, useRef, useState } from 'react';
import DatePicker from '../components/fields/DatePicker';
import FunctionKeys from '../components/fields/FunctionKeys';
import InfoCard from '../components/fields/InfoCard';
import Layout from '../components/layout/ModalLayout';
import DetailTable from '../components/tables/DetailTable';

export default function FormShiire({ context }) {
  // データが0件の場合はエラーメッセージを表示して画面を閉じる
  if (!context.data || context.data.length === 0) {
    alert('[DB-004] 表示できる明細データがありません。');
    window.close();
    return null;
  }

  // 初期化時に、data の各行に "仕入明細行番号" プロパティを追加（行番号は index+1）
  const EXEC_PERMIT = 100;
  const initialData = assignLineNumbers(context.data, '仕入明細行番号');

  const [data, setData] = useState(initialData);
  const [message, setMessage] = useState('');
  const [tableActive, setTableActive] = useState(false);
  const [currentState, setCurrentState] = useState('default');
  const [uploadMode, setUploadMode] = useState(queryParams.get('gGetuDateFLG') === 'true' ? null : 'チェック');
  const [focusedRow, setFocusedRow] = useState(null);
  const [copyedRow,setCopyedRow] = useState(null);

  const [処理区分,set処理区分] = useState(context.headers.find(header => header.label === '処理区分')["value"]);
  
  // LocalStorageから仕入日付を取得（保存されていれば優先）
  const savedDate = localStorage.getItem('仕入明細_仕入日付');
  const initialDate = savedDate && savedDate !== 'null' ? savedDate : context.headers.find(header => header.label === '仕入日付')["value"];
  
  const [仕入日付,set仕入日付] = useState(initialDate);
  const [支払日付,set支払日付] = useState(initialDate);

  // 合計ヘッダー用state
  const [税抜金額,set税抜金額] = useState(0);
  const [外税対象額,set外税対象額] = useState(0);
  const [外税,set外税] = useState(0);
  const [内税対象額,set内税対象額] = useState(0);
  const [非課税金額,set非課税金額] = useState(0);
  const [合計金額,set合計金額] = useState(0);
  const [売上合計,set売上合計] = useState(0);
  const taxs = [外税対象額,外税,内税対象額,非課税金額,合計金額,売上合計];

  // 読み込み時に実行されるAPIの状態を管理する
  const [準備完了,set準備完了] = useState(false);

  // 画面を開く際にロック処理を実行
  useEffect(() => {
    const lockData = async () => {
      try {
        const estiNo = queryParams.get("@i見積番号");
        
        // 見積番号のロック
        if (estiNo) {
          if (!await LockData('見積番号', estiNo)) {
            window.close();
            return;
          }
        }
      } catch (error) {
        alert('[DB-006] ロック処理でエラーが発生しました: ' + error.message);
        window.close();
      }
    };
    
    lockData();
  }, []);



  // 仕入日付の変更ハンドラ
  const [DateFlg,setDateFlg] = useState(false);
  async function change_stock_date(e){
    try{
      if(DateFlg)return;
      setDateFlg(true);

      // 

      let new_date = e.target.value;
      if(new_date == ""){
        alert('仕入日付を入力して下さい。');
        new_date = new Date().toLocaleDateString('sv-SE');
      }
      if(new Date(new_date) <= gGetuDate){
        alert('更新済みの為、修正できません。');
      }
      set仕入日付(new_date);
      set支払日付(new_date);
      // LocalStorageに仕入日付を保存
      localStorage.setItem('仕入明細_仕入日付', new_date);
    }catch(e){
      // alert('予期せぬエラーが発生しました。');
      console.error('[GEN-034]', e);
      alert('[GEN-034] ' + e.message);
      return;
    }
    finally{
      setMessage('');
      await sleep(0);
      setDateFlg(false);
    }
  }
  // 支払日付の変更ハンドラ
  async function change_payment_date(e){
    try{
      if(DateFlg)return;
      setDateFlg(true);

      let new_date = e.target.value;
      if(new_date == ""){
        alert('支払日付を入力して下さい。');
        new_date = new Date().toLocaleDateString('sv-SE');
        return;
      }
      if(new Date(new_date) <= gGetuDate){
        alert('更新済みの為、修正できません。');
      }
      set支払日付(new_date);
    }catch(e){
      // alert('予期せぬエラーが発生しました。');
      console.error('[GEN-035]', e);
      alert('[GEN-035] ' + e.message);
      return;
    }
    finally{
      setMessage('')
      await sleep(0);
      setDateFlg(false);
    }
  }
  const date_tmp = [
    {value:仕入日付,func:change_stock_date},
    {value:支払日付,func:change_payment_date}
  ];
  // 支払日付が更新されたら税率を取得

  async function get_taxRate(){
    try {
      var res = await GetTax(new Date(支払日付));
      ZEIRITU = res.results[0].税率;
    } catch (e) {
      console.error("[API-007] Error fetching tax:", e);
    }
  }

  useEffect(() => {
    get_taxRate();
  }, [支払日付]);

  const [gGetuDate,setGgetuDate] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      const cls_dates = new clsDates("売掛月次更新日");
      await cls_dates.GetbyID();
      // updateDate が Promise の場合は await する
      const updatedDate = cls_dates.updateDate;
      setGgetuDate(updatedDate);
      await get_taxRate();
      set準備完了(true);
      calc_total(data,支払日付,set税抜金額,set外税対象額,set外税,set内税対象額,set非課税金額,set合計金額,set売上合計);
    };
    fetchData();
  }, []);

  // テーブルデータの状態管理
  // セルへの ref（キーボード移動・検索用）
  const tableRefs = useRef([]);

  useEffect(() =>{
    if(準備完了)calc_total(data,支払日付,set税抜金額,set外税対象額,set外税,set内税対象額,set非課税金額,set合計金額,set売上合計);
  },[data]);
    

  useEffect(() =>{
    if(uploadMode == null)return;
    setUploadMode('チェック');
  },[data,仕入日付,支払日付])

  async function handleDataChange(rowIndex, accessor, updatedData) {
    // updatedData の対象行を取得
    const updatedRow = updatedData[rowIndex];

    // 現在の data の対象行と比較（深い比較）
    if (JSON.stringify(updatedRow) === JSON.stringify(data[rowIndex])) {
      console.log("対象行に変更はありません。");
      return;
    }


    let newData = [];

    switch (accessor) {
      // 未使用のためコメントアウト
      // case '製品NO':
      // case '仕様NO': {
      //   const filter = {
      //     "ClassA": updatedRow["製品NO"],
      //     "ClassB": updatedRow["仕様NO"]
      //   };
      //   // 製品マスタから追加情報を取得
      //   const fetchedData = await getSeihin(filter);
      //   if (fetchedData.length === 1) {
      //     additionalData = fetchedData[0];
      //   }
      //   const updatedData = { ...updatedRow, ...additionalData };
      //   newData = assignLineNumbers(updatedData, '仕入明細行番号');
      //   break;
      // }
      case '仕入単価': {
        // 仕入金額の再計算
        updatedData[rowIndex]["仕入金額"] = ISHasuu_rtn(queryParams.get('仕入端数'),updatedData[rowIndex]["仕入数量"] * Number(updatedData[rowIndex]["仕入単価"].toString().replace(/,/g,'')),0);
        const tax = get_tax(data,rowIndex);
        updatedData[rowIndex]["消費税額"] = tax;
        newData = assignLineNumbers(updatedData, '仕入明細行番号');
        break;
      }
      default: {
        newData = assignLineNumbers(updatedData, '仕入明細行番号');
        break;
      }
    }
    console.log('Table updated:', newData);
    setData(newData);
  }


  // F11 のクリック時の共通ロジック
  function handleF11Click() {
    if (uploadMode === 'チェック') {
      handleCheckClick();
    } else if (uploadMode === '登録') {
      handleUploadClick();
    }
  }

  // 戻るボタンのハンドラ
  async function hundleBackClick(){
    if(!confirm('現在の処理を終了します。\nよろしいですか？'))return;

    // すべてのロックを解除
    const estiNo = queryParams.get("@i見積番号");
    if (estiNo) {
      await UnLockData('見積番号', estiNo);
    }
    
    window.close();
  }

  // チェックボタンのハンドラ
  async function handleCheckClick() {
    if(uploadMode == null){
      return;
    }
    // 締日確認
    if(new Date(仕入日付) <= gGetuDate || new Date(支払日付) <= gGetuDate){
        alert('更新済みの為、修正できません。');
        return;
    }

    // checkForm は外部実装
    let checkOk = await checkForm(data)
    if (checkOk) {
      setUploadMode("登録");
    }
  }

  // 登録ボタンのハンドラ
  async function handleUploadClick() {
    if(uploadMode == null){
      return;
    }
    // uploadForm は外部実装
    let confirmOk = confirm("保存しますか？");
    if (confirmOk) {
      try {
        await uploadForm(data,仕入日付,支払日付);
        
        // 登録成功時に仕入日付をLocalStorageに保存
        localStorage.setItem('仕入明細_仕入日付', 仕入日付);
        
        // 登録成功後、見積番号のロック解除（外部関数でやっていない分を補完）
        const estiNo = queryParams.get("@i見積番号");
        if (estiNo) {
          await UnLockData('見積番号', estiNo);
        }
      } catch (error) {
        // エラー時もロック解除
        const estiNo = queryParams.get("@i見積番号");
        if (estiNo) {
          await UnLockData('見積番号', estiNo);
        }
        throw error;
      }
    }
  }

  // 全削除ボタンのハンドラ
  async function handleDeleteClick() {
    if(uploadMode == null){
      return;
    }
    // deleteForm は外部実装
    let deleteOk = confirm("表示されている明細情報をすべて削除しますか？");
    if (deleteOk) {
      try {
        await deleteForm();
        
        // 削除成功後、見積番号のロック解除（外部関数でやっていない分を補完）
        const estiNo = queryParams.get("@i見積番号");
        if (estiNo) {
          await UnLockData('見積番号', estiNo);
        }
      } catch (error) {
        // エラー時もロック解除
        const estiNo = queryParams.get("@i見積番号");
        if (estiNo) {
          await UnLockData('見積番号', estiNo);
        }
        throw error;
      }
    }
  }

  // フォーカスが当たったときにメッセージを更新ハンドラ
  function handleTargetFocus(col) {
    console.log('Target focussed:', col);
    setMessage(col.description);
    setCurrentState(col.label === "CK" || col.label === "製品No" ? col.label : "default");
  }

  function assignLineNumbers(data, fieldName) {
    return data.map((row, index) => ({ ...row, [fieldName]: index + 1 }));
  }

  // キー設定オブジェクト（uploadMode に応じた設定）
  const keyConfig = {
    default: {
      F9: { label: "全削除", onClick: handleDeleteClick, disable: 処理区分 == "0" || context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F11: { label: uploadMode, onClick: handleF11Click, disable: context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F12: { label: '戻る', onClick: hundleBackClick },
    },
    CK: {
      F1: { label: 'ALL選択', onClick: () => checkAll(true) },
      F2: { label: 'ALL解除', onClick: () => checkAll(false) },
      F9: { label: "全削除", onClick: handleDeleteClick, disable: 処理区分 == "0" || context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F11: { label: uploadMode, onClick: handleF11Click, disable: context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F12: { label: '戻る', onClick: hundleBackClick},
    },
    製品No: {
      F1: { label: '行挿入', onClick: () => addRow() },
      F2: { label: '行削除', onClick: () => deleteRow() },
      F3: { label: '行コピー', onClick: () => copyRow() },
      F4: { label: '行複写', onClick: () => pasteRow() },
      F5: { label: '上へ', onClick: () =>  toUp()},
      F6: { label: '下へ', onClick: () => toDown() },
      F9: { label: "全削除", onClick: handleDeleteClick, disable: 処理区分 == "0" || context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F11: { label: uploadMode, onClick: handleF11Click, disable: context["gGetuDateFLG"] || +context["permittion"] < EXEC_PERMIT },
      F12: { label: '戻る', onClick:hundleBackClick },
    },
  };

    // 全選択ボタン
    function checkAll(flg){
      const tmp = data.map(row => ({
        ...row,           // 各行のコピーを作成
        CHECK: flg        // CHECKを変更
      }));
      setData(tmp);       // 新しい配列をセット
    }


  function toUp(){
    if(focusedRow == 0)return;
    let tmp = [...data];
    [tmp[focusedRow],tmp[focusedRow-1]] = [tmp[focusedRow-1],tmp[focusedRow]];
    tableRefs.current[focusedRow-1][6].focus();
    setData(tmp);
    setFocusedRow(focusedRow-1);
  }

  function toDown(){
    if(data.length == (focusedRow+1))return;
    let tmp = [...data];
    [tmp[focusedRow],tmp[focusedRow+1]] = [tmp[focusedRow+1],tmp[focusedRow]];
    tableRefs.current[focusedRow+1][6].focus();
    setData(tmp);
    setFocusedRow(focusedRow+1);
  }


  // 行挿入
  function addRow(){
    let tmp = [...data];
    const emptyRow = Object.fromEntries(Object.keys(tmp[0]).map(key => [key, ""]));
    tmp.splice(focusedRow,0,emptyRow);
    setData(tmp);
  }

  // 行削除
  function deleteRow(){
    let tmp = [...data];
    tmp.splice(focusedRow,1);
    setData(tmp);
  }

  // 行コピー
  function copyRow(){
    let tmp = [...data];
    setCopyedRow(tmp[focusedRow]);
  }

  // 行複写
  function pasteRow(){
    let tmp = [...data];
    tmp.splice(focusedRow,1,copyedRow);
    setData(tmp);
  }

  return (
    <Layout
      message={message}
      leftHeaderContent={
        <>
          {context.headers
            .filter(header => header.position === 'left')
            .map(header => (
              <InfoCard
                key={header.accessor}
                label={header.label}
                value={header.value}
                value2={header.value2}
              />
            ))
          }
          <div className="flex space-x-2">
            {context.headers
              .filter(header => header.position === 'left-down')
              .map((header,i) => (
                <div key={header.accessor} className="flex-1 min-w-[180px]">
                  <InfoCard label={header.label}>
                    <DatePicker
                      name={header.label}
                      value={date_tmp[i]["value"]}
                      onFocus={() => setMessage(`${header.label}を入力してください。`)}
                      onBlur={date_tmp[i]["func"]}
                    />
                  </InfoCard>
                </div>
              ))
            }
          </div>
        </>
      }
      rightHeaderContent={
        <>
          {
            context.headers
              .filter(header => header.position == 'right')
              .map((header,i) => (
                <InfoCard
                  key={header.accessor}
                  label={header.label}
                  value={taxs[i]}
                  valueAlign = "right"
                />
              ))
          }
        </>

      }
      tableContent={
        <DetailTable
          columns={context.columns}
          data={data}
          searchColumnLabel="製品No"
          viewRadioLabel="表示切替"
          onDataChange={handleDataChange}
          onCellFocus={handleTargetFocus}
          onSearchFocus={() => setMessage('製品Noを検索してください。')}
          onSearchBlur={() => setMessage('')}
          focusedRow = {focusedRow}
          setFocusedRow = {setFocusedRow}
          tableRefs = {tableRefs}
        />
      }
      keyButtonContent={
        <FunctionKeys keyConfig={keyConfig} currentState={currentState} />
      }
    />
  );
}
