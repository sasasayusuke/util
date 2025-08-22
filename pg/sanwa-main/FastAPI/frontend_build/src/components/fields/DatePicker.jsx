// components/fields/DatePicker.jsx
import React, { useState,useEffect } from 'react';

export default function DatePicker({ name, value = '', onFocus, onBlur, minDate = "1899-12-30",onChange}) {
  const [selectedDate, setSelectedDate] = useState(value);

  // 入力値が変更された場合、ローカル state を更新
  const handleDateChange = (e) => {
    setSelectedDate(e.target.value);
    if(onChange){
      onChange(e.target.value);
    }
  };
  // const handleDateBlur = (e) => {
  //   if(onBlur){
  //     onBlur(e.target.value);
  //   }
  // };

  // 親要素と同期
  useEffect(() =>{
    setSelectedDate(value);
  },[value])


  // minDate が渡されていなければ、今日の日付を使用
  // const date = selectedDate || minDate || today; 

  return (
    <div className="relative inline-flex items-center">
      <input
        type="date"
        name={name}
        value={selectedDate}
        // value={date}
        onChange={handleDateChange}
        onFocus={onFocus}
        onBlur={onBlur}
        // min={minDate}  // ここで最小選択日を 1899-12-30 に設定
        className="
            px-2
            border border-black rounded-md
            bg-slate-50
            focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
            text-sm
          "
      />
    </div>
  );
}
