import React, { useEffect, useRef, useState } from 'react';
import DatePicker from '../components/fields/DatePicker';
import FunctionKeys from '../components/fields/FunctionKeys';
import InfoCard from '../components/fields/InfoCard';
import ModalLayout from '../components/layout/ModalLayout';
import DetailTable from '../components/tables/DetailTable';

// sleep関数の定義
function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

// Shadow DOM内からグローバル関数にアクセスするためのプロキシ関数群
function getGlobalFunction(functionName) {
  try {
    // 親ウィンドウのグローバル関数にアクセスを試行
    return window.parent?.[functionName] || window.top?.[functionName] || window[functionName];
  } catch (e) {
    console.warn(`グローバル関数 ${functionName} にアクセスできません:`, e);
    return null;
  }
}

// グローバル関数のプロキシ
function clsDates(param) {
  const globalFunc = getGlobalFunction('clsDates');
  if (globalFunc) {
    return new globalFunc(param);
  }
  console.warn('clsDates関数が見つかりません');
  return { GetbyID: async () => {}, updateDate: new Date() };
}

function GetTax(date) {
  const globalFunc = getGlobalFunction('GetTax');
  if (globalFunc) {
    return globalFunc(date);
  }
  console.warn('GetTax関数が見つかりません');
  return Promise.resolve({ results: [{ 税率: 10 }] });
}

function checkForm(data) {
  const globalFunc = getGlobalFunction('checkForm');
  if (globalFunc) {
    return globalFunc(data);
  }
  console.warn('checkForm関数が見つかりません');
  return Promise.resolve(true);
}

function uploadForm(data, 仕入日付, 支払日付) {
  const globalFunc = getGlobalFunction('uploadForm');
  if (globalFunc) {
    return globalFunc(data, 仕入日付, 支払日付);
  }
  console.warn('uploadForm関数が見つかりません');
  alert('アップロード機能が利用できません');
}

function deleteForm() {
  const globalFunc = getGlobalFunction('deleteForm');
  if (globalFunc) {
    return globalFunc();
  }
  console.warn('deleteForm関数が見つかりません');
  alert('削除機能が利用できません');
}

function assignLineNumbers(data, fieldName) {
  try {
    const globalFunc = getGlobalFunction('assignLineNumbers');
    if (globalFunc) {
      return globalFunc(data, fieldName);
    }
  } catch (e) {
    console.warn('assignLineNumbers関数が見つかりません、ローカル実装を使用');
  }
  // フォールバック実装
  return data.map((row, index) => ({ ...row, [fieldName]: index + 1 }));
}

function ISHasuu_rtn(type, value, decimal) {
  const globalFunc = getGlobalFunction('ISHasuu_rtn');
  if (globalFunc) {
    return globalFunc(type, value, decimal);
  }
  console.warn('ISHasuu_rtn関数が見つかりません、基本実装を使用');
  return Math.round(value * Math.pow(10, decimal || 0)) / Math.pow(10, decimal || 0);
}

function get_tax(data, rowIndex) {
  const globalFunc = getGlobalFunction('get_tax');
  if (globalFunc) {
    return globalFunc(data, rowIndex);
  }
  console.warn('get_tax関数が見つかりません');
  return 0;
}

function UnLockData(type, id) {
  const globalFunc = getGlobalFunction('UnLockData');
  if (globalFunc) {
    return globalFunc(type, id);
  }
  console.warn('UnLockData関数が見つかりません');
  return Promise.resolve();
}

export default function FormShiire({ context }) {
  // contextから値を取得（デフォルト値付き）
  const update_flg = context.update_flg ?? true;
  const process_category = context.process_category ?? '0';
  
  // デバッグ用ログ
  console.log('FormShiire - 受信した値:', {
    update_flg,
    process_category,
    context_keys: Object.keys(context),
    context_data: context.data,
    data_length: context.data?.length
  });
  
  // 初期化時に、data の各行に "仕入明細行番号" プロパティを追加（行番号は index+1）
  const EXEC_PERMIT = 100;
  const initialData = assignLineNumbers(context.data, '仕入明細行番号');

  const [data, setData] = useState(initialData);
  const [message, setMessage] = useState('');
  const [tableActive, setTableActive] = useState(false);
  const [currentState, setCurrentState] = useState('default');
  const [uploadMode, setUploadMode] = useState(context.gGetuDateFLG === 'true' ? null : 'チェック');
  const [focusedRow, setFocusedRow] = useState(null);
  const [copyedRow,setCopyedRow] = useState(null);

  const [処理区分,set処理区分] = useState(context.headers.find(header => header.label === '処理区分')["value"]);
  const [仕入日付,set仕入日付] = useState(context.headers.find(header => header.label === '仕入日付')["value"]);
  const [支払日付,set支払日付] = useState(context.headers.find(header => header.label === '支払日付')["value"]);

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


  // 仕入日付の変更ハンドラ
  const [DateFlg,setDateFlg] = useState(false);
  async function change_stock_date(e){
    try{
      if(DateFlg)return;
      setDateFlg(true);

      let new_date = e.target.value;
      if(new_date == ""){
        alert('仕入日付を入力して下さい。');
        new_date = new Date().toLocaleDateString('sv-SE');
      }
      if(process_category == '1' && new Date(new_date) <= gGetuDate){
        alert('更新済みの為、修正できません。');
      }
      set仕入日付(new_date);
      set支払日付(new_date);
    }catch(e){
      alert(e.message || '予期せぬエラーが発生しました。');
      console.error('change_stock_date エラー:', e);
      return;
    }
    finally{
      try {
        setMessage('');
        await sleep(0);
      } catch (e) {
        console.error('setMessage エラー:', e);
      }
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
      if(process_category == '1' && new Date(new_date) <= gGetuDate){
        alert('更新済みの為、修正できません。');
      }
      set支払日付(new_date);
    }catch(e){
      alert(e.message || '予期せぬエラーが発生しました。');
      console.error('change_payment_date エラー:', e);
      return;
    }
    finally{
      try {
        setMessage('');
        await sleep(0);
      } catch (e) {
        console.error('setMessage エラー:', e);
      }
      setDateFlg(false);
    }
  }
  const date_tmp = [
    {value:仕入日付,func:change_stock_date},
    {value:支払日付,func:change_payment_date}
  ];
  // 支払日付が更新されたら税率を取得
  let ZEIRITU;

  async function get_taxRate(){
    try {
      var res = await GetTax(new Date(支払日付));
      ZEIRITU = res.results[0].税率;
    } catch (e) {
      console.error("Error fetching tax:", e);
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

  // ===================計算処理=====================
  async function calc_total(data,payment_date,set税抜金額,set外税対象額,set外税,set内税対象額,set非課税金額,set合計金額,set売上合計){
    try{
      await new Promise(resolve => setTimeout(resolve, 0));
      const checked_rows = data.filter(e => e.CHECK);
  
      let check = 0;
      let getdata = 0;
      let i = 0;
      let wDenKB  = 0;
      let wZeiKB  = 0;
      let wTotal  = 0;
      let wKingak = 0;
      let wZeikin = 0;
      let wSiSoto = 0;
      let wSiUchi = 0;
      let wSiUZei = 0;
      let wHeSoto = 0;
      let wHeUchi = 0;
      let wHeUZei = 0;
      let wTeSoto = 0;
      let wTeUchi = 0;
      let wTeUZei = 0;
      let wZeiTotal = 0;
      let wSiHika = 0;
      let wHeHika = 0;
      let sales_All = 0;
  
      for(const row of checked_rows){
        // 売上合計
        sales_All += row["仕入数量"] * row["売上単価"];
  
        // 伝票区分
        check = row["伝票区分"];
        getdata = check;
        wDenKB = check ?  getdata : "";
  
        // 合計金額
        check = row["仕入金額"];
        getdata = check;
        wKingak = check ? getdata : 0;
  
        // 仕入税区分
        check = row["仕入税区分"];
        getdata = check;
        wZeiKB = check ? getdata : "";
  
        wTotal += wKingak;
  
        if(wZeiKB == "1"){
          wSiSoto += wKingak;
          wSiUZei = wSiSoto * ZEIRITU / 100;
        } else if(wZeiKB == "2"){
          wSiUchi += wKingak;
        } else if(wZeiKB == "3"){
          wSiHika += wKingak;
        }
      }
  
      // 結果を設定
      set税抜金額(wTotal - wSiUZei);
      set外税対象額(wSiSoto);
      set外税(wSiUZei);
      set内税対象額(wSiUchi);
      set非課税金額(wSiHika);
      set合計金額(wTotal);
      set売上合計(sales_All);
  
    } catch(e){
      console.error('計算エラー:', e);
    }
  }

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
        updatedData[rowIndex]["仕入金額"] = ISHasuu_rtn(context.仕入端数,updatedData[rowIndex]["仕入数量"] * Number(updatedData[rowIndex]["仕入単価"].toString().replace(/,/g,'')),0);
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

    // 番号を確認
    await UnLockData('仕入番号',context.仕入番号);
    window.close();
  }

  // チェックボタンのハンドラ
  async function handleCheckClick() {
    if(uploadMode == null){
      return;
    }
    // 締日確認
    if(process_category == '1' && (new Date(仕入日付) <= gGetuDate || new Date(支払日付) <= gGetuDate)){
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
      uploadForm(data,仕入日付,支払日付);
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
      deleteForm();
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
    <ModalLayout
      title="仕入明細入力"
      message={message}
      onClose={hundleBackClick}
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
          viewList = {context.view_list}
        />
      }
      keyButtonContent={
        <FunctionKeys keyConfig={keyConfig} currentState={currentState} />
      }
    />
  );
}
