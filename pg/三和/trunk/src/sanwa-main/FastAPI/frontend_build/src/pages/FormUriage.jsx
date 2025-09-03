import React, { useEffect, useRef, useState } from 'react';
import DatePicker from '../components/fields/DatePicker';
import FunctionKeys from '../components/fields/FunctionKeys';
import InfoCard from '../components/fields/InfoCard';
import Layout from '../components/layout/ModalLayout';
import DetailTable from '../components/tables/DetailTable';

export default function FormUriage({ context }) {
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

  // 画面を開く際にロック処理を実行
  useEffect(() => {
    const lockData = async () => {
      try {
        const 見積番号 = queryParams.get("@i見積番号");
        const 売上番号 = queryParams.get("@i売上番号");
        
        // 見積番号のロック
        if (見積番号) {
          await LockData('見積番号', 見積番号);
        }
        
        // 売上番号のロック
        if (売上番号) {
          await LockData('売上番号', 売上番号);
        }
      } catch (error) {
        alert('ロック処理でエラーが発生しました: ' + error.message);
        window.close();
      }
    };
    
    lockData();
  }, []);



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
  async function hundleF12Click(){
    if(!confirm('現在の処理を終了します。\nよろしいですか？'))return;

    // すべてのロックを解除
    const 見積番号 = queryParams.get("@i見積番号");
    const 売上番号 = queryParams.get("@i売上番号");
    
    if (見積番号) {
      await UnLockData('見積番号', 見積番号);
    }
    
    if (売上番号) {
      await UnLockData('売上番号', 売上番号);
    }
    
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
        onClick:hundleF12Click,
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
        onClick: hundleF12Click,
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
        onClick: hundleF12Click,
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
        onClick: hundleF12Click,
      },
    },
    updated: {
      // F7:{
      //   label:'表示項',
      //   onclick:()=> alert('表示項目ダイアログを表示する（未実装）')
      // },
      F12: {
        label: '戻る',
        onClick: hundleF12Click,
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
    <Layout
      message={message}
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
        />
      }
      keyButtonContent={
        <FunctionKeys keyConfig={keyConfig} currentState={currentState} />
      }
    />
  );
}
