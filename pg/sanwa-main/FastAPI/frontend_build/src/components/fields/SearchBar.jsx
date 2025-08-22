// components/fields/SearchBar.jsx
import React from 'react';

export default function SearchBar({
	value,
	onChange,
	onKeyDown,
	onSearch,
	onFocus,
	onBlur,
	placeholder
}) {
	return (
		<div className="flex items-center space-x-2">
			<input
				type="text"
				className="border rounded px-2 py-1"
				placeholder={placeholder}
				value={value}
				onChange={onChange}
				onKeyDown={onKeyDown}
				onFocus={onFocus}
				onBlur={onBlur}
			/>
			<button
				onClick={onSearch}
				className="px-4 py-1 bg-gray-700 text-white rounded hover:bg-gray-800"
			>
				検索
			</button>
		</div>
	);
}
