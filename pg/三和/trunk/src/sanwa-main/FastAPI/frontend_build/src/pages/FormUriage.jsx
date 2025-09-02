import React, { useEffect, useRef, useState } from 'react';
import DatePicker from '../components/fields/DatePicker';
import FunctionKeys from '../components/fields/FunctionKeys';
import InfoCard from '../components/fields/InfoCard';
import ModalLayout from '../components/layout/ModalLayout';
import DetailTable from '../components/tables/DetailTable';

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

function upload(data, salesDate, gGetuDate, 税抜金額, 外税対象額, 外税, 非課税金額, 合計金額) {
  const globalFunc = getGlobalFunction('upload');
  if (globalFunc) {
    return globalFunc(data, salesDate, gGetuDate, 税抜金額, 外税対象額, 外税, 非課税金額, 合計金額);
  }
  console.warn('upload関数が見つかりません');
  alert('アップロード機能が利用できません');
}

function purge() {
  const globalFunc = getGlobalFunction('purge');
  if (globalFunc) {
    return globalFunc();
  }
  console.warn('purge関数が見つかりません');
  alert('削除機能が利用できません');
}

function UnLockData(type, id) {
  const globalFunc = getGlobalFunction('UnLockData');
  if (globalFunc) {
    return globalFunc(type, id);
  }
  console.warn('UnLockData関数が見つかりません');
  return Promise.resolve();
}

export default function FormUriage({ context }) {
  // contextから値を取得（デフォルト値付き）
  const update_flg = context.update_flg ?? true;
  const process_category = context.process_category ?? '0';
  
  // デバッグ用ログ
  console.log('FormUriage - 受信した値:', {
    update_flg,
    process_category,
    context_keys: Object.keys(context),
    context_content: context.content,
    data_length: context.content?.data?.length,
    view_list: context.view_list,
    has_view_list: !!context.view_list
  });
  
  // 初期化時に、data の各行に "仕入明細行番号" プロパティを追加（行番号は index+1）
  const initialData = context.content.data.map((row, index) => ({ ...row, 売上明細行番号: index + 1 }));
  const [data, setData] = useState(initialData);
  const [message, setMessage] = useState('');
  const [tableActive, setTableActive] = useState(false);
  const [focusedRow, setFocusedRow] = useState(null);
  const [currentState, setCurrentState] = useState(() =>{
    if(!update_flg){
      return 'updated';
    }
    else if(process_category == '0'){
      return 'new_default';
    }
    else if(process_category == '1'){
      return 'update_default';
    }
  });
  const [salesDate,setSalesDate] = useState(context.content.headers[5]["value"]);
  const [gGetuDate,setGgetuDate] = useState(null);

  // テーブルデータの状態管理
  // セルへの ref（キーボード移動・検索用）
  const tableRefs = useRef([]);

  // 合計系
  const [税抜金額, set税抜金額] = useState(0);
  const [外税対象額, set外税対象額] = useState(0);
  const [外税, set外税] = useState(0);
  const [非課税金額, set非課税金額] = useState(0);
  const [合計金額,set合計金額 ] = useState(0);
  const tmp = [外税対象額,外税,非課税金額,合計金額];



  // テーブルデータ更新専用ハンドラ  
  function handleDataChange(rowIndex, accessor, updatedData) {

    // updatedData の対象行を取得
    const updatedRow = updatedData[rowIndex];
    if(!update_flg) {
      alert('更新済みの為、修正できません。');
      return;
    }
    // 現在の data の対象行と比較（深い比較）
    if (JSON.stringify(updatedRow) === JSON.stringify(data[rowIndex])) {
      console.log("対象行に変更はありません。");
      return;
    }
    console.log('Table updated:', updatedData);
    setData(updatedData);
  }

  // ===================合計計算処理=====================
  async function calc_total(data,sales_date,set税抜金額,set外税対象額,set外税,set非課税金額,set合計金額){
    try{
      // チェックされている行のデータを取得
      const checked_rows = data.filter(e => e.CHECK);
      
      let check = null;
      let getdata = "";
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

      for (const row of checked_rows){
        // 伝票区分
        check = row["伝票区分"];
        getdata = check;
        wDenKB = check ?  getdata : "";

        // 合計金額
        check = row["売上金額"];
        getdata = check;
        wKingak = check ? getdata : 0;

        // 売上税区分
        check = row["売上税区分"];
        getdata = check;
        wZeiKB = check ? getdata : 0;

        // 消費税額
        check = row["消費税額"];
        getdata = check;
        wZeikin = check ? getdata : 0;

        switch(wDenKB){
          case '1': // 売上
            switch(wZeiKB){
              case 0: // 外税
                wSiSoto += wKingak;
                wZeiTotal += wZeikin;
                break;
              case 1: // 内税
                wSiUchi += wKingak;
                wSiUZei += wZeikin;
                break;
              default: // 非課税
                wSiHika += wKingak;
                break;
            }
            wTotal += wKingak;
            break;
          case '2': // 返品
            switch(wZeiKB){
              case 0: // 外税
                wHeSoto += wKingak;
                wZeiTotal += wZeikin;
                break;
              case 1: // 内税
                wHeUchi += wKingak;
                wHeUZei += wZeikin;
                break;
              default: // 非課税
                wHeHika += wKingak;
                break;
            }
            wTotal += wKingak;
            break;
          case '3': // 訂正
            if(wZeiKB == 0){
              wTeSoto += wKingak;
            }else{
              wTeUchi += wKingak;
            }
            wTeUZei += wZeikin;
            wTotal += wKingak;
            break;
        }
      }
      
      // 結果を設定
      set税抜金額(
        (wSiSoto + wSiUchi - wSiUZei) + 
        (wHeSoto + wHeUchi - wHeUZei) +
        (wSiHika + wHeHika) +
        (wTeSoto + wTeUchi - wTeUZei)
      );
      set外税対象額((wSiSoto + wHeSoto));
      set非課税金額(wSiHika + wHeHika);
      set外税(wZeiTotal);
      set合計金額(wTotal);

    } catch(e){
      console.error('計算エラー:', e);
    }
  }

  useEffect(() => {
    // 合計の計算
    calc_total(data,salesDate,set税抜金額,set外税対象額,set外税,set非課税金額,set合計金額);
  },[data]);

  useEffect(() => {
    const fetchData = async () => {
      const cls_dates = new clsDates("売掛月次更新日");
      await cls_dates.GetbyID();
      // updateDate が Promise の場合は await する
      const updatedDate = cls_dates.updateDate;
      setGgetuDate(updatedDate);
    };
    fetchData();
  }, []);

  // 税率取得関数
  async function get_taxRate(date) {
    try {
      var res = await GetTax(new Date(date));
      // ZEIRITU = res.results[0].税率; // グローバル変数として設定（必要に応じて）
    } catch (e) {
      console.error("Error fetching tax:", e);
    }
  }

  useEffect(()=>{
    get_taxRate(salesDate);
  },[salesDate]);

  // F11 のクリック時の共通ロジック
  function handleF11Click() {
    handleUploadClick();
  }

  // 登録ボタンのハンドラ
  async function handleUploadClick() {
    // uploadForm は外部実装
    upload(data,salesDate,gGetuDate,税抜金額,外税対象額,外税,非課税金額,合計金額);
  }

  // F9のクリック時の共通ロジック
  function hundleF9Click(){
    handleDeleteClick();
  }

  // 全削除ボタンのハンドラ
  async function handleDeleteClick(){
    purge();
  }

  // 戻るボタンのハンドラ
  async function hundleBackClick(){
    if(!confirm('現在の処理を終了します。\nよろしいですか？'))return;

    await UnLockData('売上番号',context.売上番号);
    window.close();
  }


  // フォーカスが当たったときにメッセージを更新ハンドラ
  function handleTargetFocus(col) {
    console.log('Target focussed:', col);
    setMessage(col.description);
    if(!update_flg){
      setCurrentState('updated');
    }
    else if ( process_category == '0') {
      setCurrentState('new_CK');
    }
    else if(process_category == '1'){
      setCurrentState('update_CK');
    }
  }

  // キー設定オブジェクト（checkedFlg に応じた設定）
  const keyConfig = {
    new_default: {
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F11: {
        label: '保存',
        onClick: update_flg ? handleF11Click:"",
      },
      F12: {
        label: '戻る',
        onClick: hundleBackClick,
      },
    },
    new_CK: {
      F1: {
        label: 'ALL選択',
        onClick:() => checkAll(true),
      },
      F2: {
        label: 'ALL解除',
        onClick:() => checkAll(false),
      },
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F11: {
        label: "保存",
        onClick: update_flg ? handleF11Click:"",
      },
      F12: {
        label: '戻る',
        onClick: hundleBackClick,
      },
    },
    update_default: {
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F9: {
        label: '全削除',
        onClick: update_flg ? hundleF9Click:"",
      },
      F11: {
        label: '保存',
        onClick: update_flg ? handleF11Click:"",
      },
      F12: {
        label: '戻る',
        onClick: hundleBackClick,
      },
    },
    update_CK: {

      F1: {
        label: 'ALL選択',
        onClick:() => checkAll(true),
      },
      F2: {
        label: 'ALL解除',
        onClick:() => checkAll(false),
      },
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F11: {
        label: '保存',
        onClick: update_flg ? handleF11Click:"",
      },
      F12: {
        label: '戻る',
        onClick: hundleBackClick,
      },
    },
    updated: {
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F12: {
        label: '戻る',
        onClick: hundleBackClick,
      },
    },

  };

  function change_salesDate(e){
    try{
      let new_date = e.target.value;
      if(new_date == ""){
        alert('売上日付を入力して下さい。');
        new_date = new Date().toLocaleDateString('sv-SE');
        // return;
      }
      if(new Date(new_date) <= gGetuDate){
        alert('更新済みの為、修正できません。');
        // setSalesDate(new Date().toLocaleDateString('sv-SE'));
        // return;
      }
      setSalesDate(new_date);
    }catch(e){
      alert('予期せぬエラーが発生しました。');
      console.error(e);
      return;
    }
    finally{
      setMessage('')
    }
  }

  // 全選択ボタン
  function checkAll(flg){
    const tmp = data.map(row => ({
      ...row,           // 各行のコピーを作成
      CHECK: flg        // CHECKを変更
    }));
    setData(tmp);       // 新しい配列をセット
  }
  

  return (
    <ModalLayout
      title="売上明細入力"
      message={message}
      onClose={hundleBackClick}
      leftHeaderContent={
        <>
          {
            context.content.headers
              .filter(header => header.position == 'left')
              .map(header => {
                switch(header.label){
                    case '売上日付':
                        return <React.Fragment key='sales_date'>
                            <div className='flex-1 space-x-2'>
                                  <div className="flex-1 min-w-[180px]">
                                      <InfoCard
                                      key={`${header.accessor}_info_card`}
                                          label="売上日付"
                                          children={
                                          <DatePicker
                                              name="売上日付"
                                              value={salesDate}
                                              onFocus={() => {
                                                setMessage('売上日付を入力してください。')
                                                if(!update_flg){
                                                  setCurrentState('updated');
                                                }
                                                else if(process_category == '0'){
                                                  setCurrentState('new_default')
                                                }
                                                else{
                                                  setCurrentState("update_default");
                                                }
                                              }}
                                              onBlur={change_salesDate}
                                          />
                                          }
                                      />
                                  </div>
                              </div>
                        </React.Fragment>
                        
                    case '納入先':
                        return (
                            <InfoCard
                                key={header.accessor}
                                label={header.label}
                                value={header.value}
                                value2={header.value2}
                                value3={header.value3}
                            />
                        )
                    default:
                        return (
                            <InfoCard
                                key={header.accessor}
                                label={header.label}
                                value={header.value}
                                value2={header.value2}

                            />
                        )
                }
            })
          }
        </>
      }
      rightHeaderContent={
        <>

          {
            context.content.headers
              .filter(header => header.position == 'right')
              .map((header,i) => (
                <InfoCard
                  key={header.accessor}
                  label={header.label}
                  value={tmp[i]}
                  valueAlign="right"
                />
              ))
          }
        </>

      }
      tableContent={
        <DetailTable
          columns={context.content.columns}
          data={data}
          searchColumnLabel="製品No"
          viewRadioLabel="表示切替"
          onDataChange={handleDataChange}
          onCellFocus={handleTargetFocus}
          onSearchFocus={() => setMessage('製品Noを検索してください。')}
          onSearchBlur={() => setMessage('')}
          focusedRow = {focusedRow}
          setFocusedRow = {setFocusedRow}
          tableRefs={tableRefs}
          viewList = {context.view_list}
        />
      }
      keyButtonContent={
        <FunctionKeys keyConfig={keyConfig} currentState={currentState} />
      }
    />
  );
}
