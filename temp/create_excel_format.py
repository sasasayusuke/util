#!/usr/bin/env python3
import csv
import json

def create_excel_format():
    """Create Excel format template using basic Python"""
    
    # Create CSV files that can be opened as Excel
    
    # 1. Create Summary Sheet
    summary_data = [
        ["#", "システム", "機能", "シート名"],
        ["1", "Front Office_共通", "ログイン", "【共通】ログイン"],
        ["2", "Front Office_共通", "トップ", "【共通】トップ"],
        ["3", "Front Office_顧客ユーザー", "ヘッダー・フッター", "【顧客ユーザー】ヘッダー・フッター"],
        ["4", "Front Office_顧客ユーザー", "ホーム", "【顧客ユーザー】ホーム"],
        ["5", "Front Office_支援者", "ヘッダー・フッター", "【支援者】ヘッダー・フッター"],
        ["6", "Front Office_支援者", "ホーム", "【支援者】ホーム"],
        ["", "", "", ""],
        ["記入例：", "", "", ""],
        ["連番", "対象システム名", "対象機能名", "テストケースシート名"]
    ]
    
    with open('サマリー.csv', 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerows(summary_data)
    
    # 2. Create Test Case Template Sheet
    testcase_headers = [
        "#", "大項目", "中項目", "小項目", "パターン", "userロール",
        "前提条件", "手順", "期待値", "設計備考", "環境", "ステータス",
        "担当", "実施日", "バックログNO", "備考"
    ]
    
    testcase_data = [
        ["テストケース番号", "テストの大分類", "テストの中分類", "テストの小分類", 
         "テストパターンの詳細", "実行するユーザーの権限/役割",
         "テスト実行前の必要な条件", "テスト実行の具体的な手順", "期待される結果",
         "設計に関する補足情報", "テスト実行環境", "テストの実行状況",
         "テスト実行担当者", "テスト実施日", "関連するバックログ番号", "その他の補足情報"],
        testcase_headers,
        ["1", "ログイン機能", "正常系", "有効な認証情報", "正しいID・パスワード",
         "顧客ユーザー", "ユーザーが登録済み", "1.ログイン画面を開く\n2.ID・パスワードを入力\n3.ログインボタンクリック",
         "ホーム画面に遷移する", "", "開発環境", "未実施", "", "", "BL-001", ""],
        ["2", "ログイン機能", "異常系", "無効な認証情報", "間違ったパスワード",
         "顧客ユーザー", "ユーザーが登録済み", "1.ログイン画面を開く\n2.正しいID、間違ったパスワードを入力\n3.ログインボタンクリック",
         "エラーメッセージが表示される", "", "開発環境", "未実施", "", "", "BL-001", ""],
        ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""]
    ]
    
    with open('テストケーステンプレート.csv', 'w', newline='', encoding='utf-8') as f:
        writer = csv.writer(f)
        writer.writerows(testcase_data)
    
    # 3. Create specific test case sheets
    test_sheets = [
        "【共通】ログイン",
        "【共通】トップ", 
        "【顧客ユーザー】ヘッダー・フッター",
        "【顧客ユーザー】ホーム"
    ]
    
    for sheet_name in test_sheets:
        sheet_data = [
            ["説明: " + sheet_name + "のテストケース"],
            testcase_headers,
            ["1", "例：" + sheet_name.replace("【", "").replace("】", ""), 
             "正常系", "基本機能", "正常パターン", "該当ユーザー",
             "前提条件を記載", "手順を記載", "期待値を記載", 
             "備考", "開発環境", "未実施", "", "", "", ""],
            ["", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""]
        ]
        
        filename = sheet_name.replace("【", "").replace("】", "_") + ".csv"
        with open(filename, 'w', newline='', encoding='utf-8') as f:
            writer = csv.writer(f)
            writer.writerows(sheet_data)
    
    print("Excel format template files created:")
    print("- サマリー.csv")
    print("- テストケーステンプレート.csv")
    for sheet in test_sheets:
        filename = sheet.replace("【", "").replace("】", "_") + ".csv"
        print(f"- {filename}")
    
    print("\nThese CSV files can be opened in Excel and saved as .xlsx format")
    print("Or use LibreOffice Calc to convert to Excel format")

create_excel_format()