// src/components/layout/ModalLayout.jsx
import React from 'react';

export default function ModalLayout({
    message,
    title,
    leftHeaderContent,
    rightHeaderContent,
    tableContent,
    keyButtonContent,
    onClose,
}) {
    return (
        <div className="modal-content bg-gray-50 flex flex-col relative" 
             style={{ 
                 height: '90vh', 
                 maxHeight: '90vh',
                 margin: '5vh auto',
                 width: '98%',
                 maxWidth: '1400px',
                 borderRadius: '12px',
                 boxShadow: '0 20px 60px rgba(0,0,0,0.2)'
             }}>
            {/* 閉じるボタン */}
            {onClose && (
                <button
                    onClick={onClose}
                    className="absolute top-3 right-3 z-20 text-gray-500 hover:text-gray-700 text-2xl font-bold cursor-pointer"
                    style={{ lineHeight: '1' }}
                >
                    ×
                </button>
            )}
            
            {/* タイトル */}
            {title && (
                <header className="bg-white border-b border-gray-200 px-4 py-3 shadow-sm rounded-t-lg">
                    <h1 className="text-lg font-bold text-gray-800">{title}</h1>
                </header>
            )}

            {/* メインコンテンツ */}
            <main className="flex-1 w-full px-2 py-1 overflow-auto" style={{ paddingBottom: '10px' }}>
                {/* ヘッダーのセクション */}
                <div className="flex flex-col md:flex-row gap-2 mb-1">
                    <div className="bg-gray-100 p-1 rounded-lg shadow-md flex-[2_1_0%]">
                        {leftHeaderContent}
                    </div>
                    <div className="bg-gray-100 p-1 rounded-lg shadow-md flex-1">
                        {rightHeaderContent}
                    </div>
                </div>

                {/* テーブルセクション */}
                <div className="bg-white p-1 rounded-lg shadow-md overflow-hidden flex-1 flex flex-col">
                    <div className="flex-1 overflow-hidden">
                        {tableContent}
                    </div>
                    <div className="px-2 py-1 text-left text-xs text-black border-t bg-gray-50">
                        {message}
                    </div>
                </div>

            </main>
            {/* フッターセクション */}
            <footer className="bg-gray-100 p-1 shadow-inner flex-shrink-0">
                {keyButtonContent}
            </footer>

        </div>
    );
}
