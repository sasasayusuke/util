// src/components/fields/InfoCard.jsx
import React from 'react';

/**
 * 単一のラベル & 値を表示するカードコンポーネント
 *
 * @param {string} label 左側に表示するラベル (必須)
 * @param {string} [value] 右側に表示する短い値 (条件必須)
 * @param {string} [value2] 右側に表示する長い値 (任意)
 * @param {string} [value3] 右側に表示する追加の値 (任意)
 * @param {ReactNode} [children] 右側に表示する任意のコンポーネント (条件必須)
 */
export default function InfoCard ({
	label,
	value,
	value2,
	value3,
	children,
	valueAlign = "left",
	value2Align = "left",
	value3Align = "left"
}) {
	// 必須項目 が falsy な場合は "未設定" と表示
	label = label || "未設定";
	if(commonIsNull(value))value = "未設定";

	// 内部で幅の設定を固定
	const LABEL_WIDTH = 'min-w-32';
	const LABEL_HEIGHT = 'min-h-4';
	const VALUE_WIDTH = 'flex-1';

	const getAlignClass = (align) =>
		({ left: "text-left", center: "text-center", right: "text-right" }[align] || "text-left");

	return (
		<div className="bg-white border border-gray-300 rounded-sm shadow-sm overflow-hidden">
			<div className="flex">
				<div className={`bg-gray-500 text-white text-center font-semibold px-2 py-1 whitespace-nowrap text-sm ${LABEL_WIDTH} ${LABEL_HEIGHT}`}>
					{label}
				</div>
				<div className={`bg-white text-gray-900 px-2 py-1 ${VALUE_WIDTH}`}>
					{children ?? (
						<div className="flex gap-2">
							{/* value: 他に値がある場合は固定幅、なければ flex-1 */}
							<div className={`bg-gray-200 border border-gray-300 rounded px-2 py-0.5 text-sm ${!commonIsNull(value2) || !commonIsNull(value3) ? 'w-32' : 'flex-1'} ${getAlignClass(valueAlign)}`}>
								{value}
							</div>
							{!commonIsNull(value2) && (
								<div className={`flex-1 bg-gray-200 border border-gray-300 rounded px-2 py-0.5 text-sm ${getAlignClass(value2Align)}`}>
									{value2}
								</div>
							)}
							{!commonIsNull(value3) && (
								<div className={`flex-1 bg-gray-200 border border-gray-300 rounded px-2 py-0.5 text-sm ${getAlignClass(value3Align)}`}>
									{value3}
								</div>
							)}
						</div>
					)}
				</div>
			</div>
		</div>
	);
};
