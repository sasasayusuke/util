#!/usr/bin/env python3
import zipfile
import xml.etree.ElementTree as ET
import re

def analyze_test_case_format(filename):
    """Extract detailed format from test case Excel file"""
    
    with zipfile.ZipFile(filename, 'r') as zip_file:
        # Get shared strings
        shared_strings = []
        if 'xl/sharedStrings.xml' in zip_file.namelist():
            shared_xml = zip_file.read('xl/sharedStrings.xml').decode('utf-8')
            shared_strings = re.findall(r'<t[^>]*>([^<]+)</t>', shared_xml)
        
        # Analyze specific sheets for format patterns
        sheets_to_analyze = ['【共通】ログイン', '【顧客ユーザー】ホーム']
        
        for sheet_idx, sheet_name in enumerate(['sheet4', 'sheet6'], start=1):
            if f'xl/worksheets/{sheet_name}.xml' in zip_file.namelist():
                sheet_xml = zip_file.read(f'xl/worksheets/{sheet_name}.xml').decode('utf-8')
                
                print(f"\n=== Sheet Analysis: {sheets_to_analyze[sheet_idx-1] if sheet_idx <= len(sheets_to_analyze) else sheet_name} ===")
                
                # Extract all cells
                cells = re.findall(r'<c r="([A-Z]+)(\d+)"[^>]*>.*?<v>([^<]+)</v>', sheet_xml, re.DOTALL)
                
                # Group by row
                rows = {}
                for col, row, value in cells:
                    row_num = int(row)
                    if row_num not in rows:
                        rows[row_num] = []
                    
                    # Convert string reference to actual string
                    try:
                        val_idx = int(value)
                        if val_idx < len(shared_strings):
                            value = shared_strings[val_idx]
                    except ValueError:
                        pass
                    
                    rows[row_num].append((col, value))
                
                # Display first 10 rows
                print("\nFirst 10 rows:")
                for row_num in sorted(rows.keys())[:10]:
                    print(f"\nRow {row_num}:")
                    for col, val in sorted(rows[row_num]):
                        print(f"  {col}{row_num}: {val[:50]}..." if len(val) > 50 else f"  {col}{row_num}: {val}")

# Analyze the test case file
print("=== Test Case Excel Format Analysis ===")
analyze_test_case_format('【Nuxt3】【FO】結合試験テストケース（v２.５）.xlsx')