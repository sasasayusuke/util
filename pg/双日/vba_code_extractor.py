#!/usr/bin/env python3
"""
VBAコード詳細抽出ツール
マクロが含まれるExcelファイルの完全なVBAコードを抽出します。
"""

from oletools.olevba import VBA_Parser
from pathlib import Path
import json

def extract_full_vba_code(file_path):
    """指定ファイルの完全なVBAコードを抽出"""
    result = {
        'file_name': Path(file_path).name,
        'modules': {}
    }
    
    try:
        vba_parser = VBA_Parser(str(file_path))
        
        if vba_parser.detect_vba_macros():
            print(f"\n=== {Path(file_path).name} VBAコード詳細 ===")
            
            for (filename, stream_path, vba_filename, vba_code) in vba_parser.extract_macros():
                if vba_code and vba_code.strip():
                    result['modules'][vba_filename] = {
                        'code': vba_code,
                        'lines': len(vba_code.split('\n'))
                    }
                    
                    print(f"\n--- {vba_filename} ---")
                    print(f"行数: {len(vba_code.split('\n'))}")
                    print("=" * 50)
                    print(vba_code)
                    print("=" * 50)
        
        vba_parser.close()
        
    except Exception as e:
        print(f"エラー: {e}")
        
    return result

def main():
    # マクロが含まれるファイルのリスト
    macro_files = [
        '/home/sdt_op/projects/util/pg/soujitsu/01_受領資料/計算用管理変動費Master.xls',
        '/home/sdt_op/projects/util/pg/soujitsu/01_受領資料/MO報告光熱費.xls'
    ]
    
    all_results = {}
    
    for file_path in macro_files:
        if Path(file_path).exists():
            result = extract_full_vba_code(file_path)
            all_results[Path(file_path).name] = result
    
    # 結果をJSONファイルに保存
    output_file = '/home/sdt_op/projects/util/pg/soujitsu/vba_code_complete.json'
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(all_results, f, ensure_ascii=False, indent=2)
    
    print(f"\n💾 完全なVBAコードを保存しました: {output_file}")

if __name__ == "__main__":
    main()