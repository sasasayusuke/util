#!/usr/bin/env python3
import zipfile
import xml.etree.ElementTree as ET
import re

def extract_excel_structure(filename):
    """Extract basic structure from Excel file without external libraries"""
    
    # Excel files are actually zip archives
    with zipfile.ZipFile(filename, 'r') as zip_file:
        # Get list of sheets
        workbook_xml = zip_file.read('xl/workbook.xml').decode('utf-8')
        sheet_names = re.findall(r'<sheet name="([^"]+)"', workbook_xml)
        
        print(f"=== Excel File Analysis: {filename} ===\n")
        print(f"Found {len(sheet_names)} sheets: {', '.join(sheet_names)}\n")
        
        # Analyze first sheet structure
        if 'xl/worksheets/sheet1.xml' in zip_file.namelist():
            sheet_xml = zip_file.read('xl/worksheets/sheet1.xml').decode('utf-8')
            
            # Extract cell values
            cell_values = re.findall(r'<c r="([A-Z]+\d+)"[^>]*>.*?<v>([^<]+)</v>', sheet_xml, re.DOTALL)
            
            # Get shared strings if they exist
            shared_strings = []
            if 'xl/sharedStrings.xml' in zip_file.namelist():
                shared_xml = zip_file.read('xl/sharedStrings.xml').decode('utf-8')
                shared_strings = re.findall(r'<t[^>]*>([^<]+)</t>', shared_xml)
            
            print(f"Sheet: {sheet_names[0] if sheet_names else 'Sheet1'}")
            print(f"Found {len(cell_values)} cells with values")
            
            # Display first few cells
            print("\nFirst few cells:")
            for i, (cell_ref, value) in enumerate(cell_values[:10]):
                # If value is numeric and less than length of shared strings, it's a string reference
                try:
                    val_idx = int(value)
                    if val_idx < len(shared_strings):
                        value = shared_strings[val_idx]
                except ValueError:
                    pass
                
                print(f"  {cell_ref}: {value}")
            
            # Try to identify headers (row 1)
            row1_cells = [(ref, val) for ref, val in cell_values if ref.endswith('1')]
            if row1_cells:
                print("\nPossible headers (Row 1):")
                for ref, val in row1_cells:
                    try:
                        val_idx = int(val)
                        if val_idx < len(shared_strings):
                            val = shared_strings[val_idx]
                    except ValueError:
                        pass
                    print(f"  {ref}: {val}")

# Analyze all Excel files
excel_files = ['sample.xlsx', 'sample2.xlsx', '【Nuxt3】【FO】結合試験テストケース（v２.５）.xlsx']

for excel_file in excel_files:
    try:
        extract_excel_structure(excel_file)
        print("\n" + "="*50 + "\n")
    except Exception as e:
        print(f"Error reading {excel_file}: {str(e)}\n")