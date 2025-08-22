export default function HeaderRow ({label, value, rightAlign = false, bold = false}) {
	return (
		<div className="grid grid-cols-4 gap-0">
			<div className="p-2 bg-gray-800 text-white text-sm font-medium">
				{label}
			</div>
			<div
				className={`
					col-span-3
					p-2
					bg-gray-100
					text-gray-900
					${rightAlign ? 'text-right' : ''}
					${bold ? 'font-semibold' : ''}
					font-mono
				`}
			>
				{value}
			</div>
		</div>
	)
}