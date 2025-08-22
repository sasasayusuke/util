// src/components/layout/Layout.jsx
import React from 'react';

export default function Layout({
    message,
    leftHeaderContent,
    rightHeaderContent,
    tableContent,
    keyButtonContent,
}) {
    return (
        <div className="min-h-screen bg-gray-50 flex flex-col">

            {/* メインコンテンツ */}
            <main className="flex-grow w-full px-4 py-4">
                {/* ヘッダーのセクション */}
                <div className="flex flex-col md:flex-row gap-6 mb-1">
                    <div className="bg-gray-100 p-2 rounded-lg shadow-md flex-[2_1_0%]">
                        {leftHeaderContent}
                    </div>
                    <div className="bg-gray-100 p-2 rounded-lg shadow-md flex-1">
                        {rightHeaderContent}
                    </div>
                    <div className="w-[40%]">

                    </div>
                </div>

                {/* テーブルセクション */}
                <div className="bg-white p-2 rounded-lg shadow-md overflow-x-auto pb-0 ">
                    {tableContent}
                    <div className="p-4 text-left text-xs text-black h-6 border-b">
                        {message}
                    </div>
                </div>

                {/* メッセージセクション */}

            </main>
            {/* フッターセクション */}
            <footer className="bg-gray-100 p-4 rounded-lg shadow-inner">
                {keyButtonContent}
            </footer>

        </div>
    );
}
