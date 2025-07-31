// components/fields/RadioBar.jsx
import React from 'react';

/**
 * ラジオボタンのグループを表示するコンポーネント例
 *
 * @param {string} title グループのタイトル
 * @param {Array} items ラジオボタンの選択肢 [{ value: string, label: string }, ...]
 * @param {string} selectedValue 現在選択中の value
 * @param {Function} onChange (newValue) => void : 選択変更時に呼ばれるコールバック
 */
export default function RadioBar({
	title,
	items,
	selectedValue,
	onChange
}) {
	function handleChange(e) {
		const newValue = e.target.value;
		if (onChange) {
			onChange(newValue);
		}
	};

	return (
		<div className="inline-flex items-center gap-6">

			<div className="mb-3 text-lg font-semibold text-gray-700">{title}</div>
			{items.map(({ value, label }) => (
				<label
					key={value}
					className="flex-none flex items-center cursor-pointer"
				>
					<input
						type="radio"
						name={`radioGroup-${title}`}
						value={value}
						checked={selectedValue === value}
						onChange={handleChange}
						className="form-radio text-blue-600 h-4 w-4"
					/>
					<span className="ml-2 text-gray-800 whitespace-nowrap">{label}</span>
				</label>
			))}
		</div>
	);
}
