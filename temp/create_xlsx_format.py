#!/usr/bin/env python3
import zipfile
import os
from datetime import datetime

def create_xlsx_file():
    """Create a proper XLSX file using ZIP and XML structure"""
    
    # Create directory structure
    os.makedirs('xl/worksheets', exist_ok=True)
    os.makedirs('_rels', exist_ok=True)
    os.makedirs('xl/_rels', exist_ok=True)
    os.makedirs('xl/theme', exist_ok=True)
    
    # [Content_Types].xml
    content_types = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
    <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
    <Default Extension="xml" ContentType="application/xml"/>
    <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
    <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
    <Override PartName="/xl/worksheets/sheet2.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
    <Override PartName="/xl/sharedStrings.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml"/>
    <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
    <Override PartName="/xl/theme/theme1.xml" ContentType="application/vnd.openxmlformats-officedocument.theme+xml"/>
</Types>'''
    
    # _rels/.rels
    rels = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
    <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
</Relationships>'''
    
    # xl/_rels/workbook.xml.rels
    workbook_rels = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
    <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
    <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet2.xml"/>
    <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings" Target="sharedStrings.xml"/>
    <Relationship Id="rId4" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
    <Relationship Id="rId5" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme" Target="theme/theme1.xml"/>
</Relationships>'''
    
    # xl/workbook.xml
    workbook = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
    <sheets>
        <sheet name="サマリー" sheetId="1" r:id="rId1"/>
        <sheet name="テストケーステンプレート" sheetId="2" r:id="rId2"/>
    </sheets>
</workbook>'''
    
    # Shared strings
    strings = [
        "#", "システム", "機能", "シート名",
        "1", "Front Office_共通", "ログイン", "【共通】ログイン",
        "2", "トップ", "【共通】トップ",
        "3", "Front Office_顧客ユーザー", "ヘッダー・フッター", "【顧客ユーザー】ヘッダー・フッター",
        "4", "ホーム", "【顧客ユーザー】ホーム",
        "記入例：", "連番", "対象システム名", "対象機能名", "テストケースシート名",
        
        # Test case headers
        "大項目", "中項目", "小項目", "パターン", "userロール",
        "前提条件", "手順", "期待値", "設計備考", "環境", "ステータス",
        "担当", "実施日", "バックログNO", "備考",
        
        # Sample data
        "ログイン機能", "正常系", "有効な認証情報", "正しいID・パスワード",
        "顧客ユーザー", "ユーザーが登録済み", 
        "1.ログイン画面を開く\\n2.ID・パスワードを入力\\n3.ログインボタンクリック",
        "ホーム画面に遷移する", "開発環境", "未実施", "BL-001",
        "異常系", "無効な認証情報", "間違ったパスワード",
        "1.ログイン画面を開く\\n2.正しいID、間違ったパスワードを入力\\n3.ログインボタンクリック",
        "エラーメッセージが表示される"
    ]
    
    shared_strings = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<sst xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" count="{}" uniqueCount="{}">'''.format(len(strings), len(strings))
    
    for s in strings:
        shared_strings += f'''
    <si><t>{s}</t></si>'''
    
    shared_strings += '''
</sst>'''
    
    # Summary worksheet
    summary_sheet = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
    <sheetData>
        <row r="1">
            <c r="A1" t="s"><v>0</v></c>
            <c r="B1" t="s"><v>1</v></c>
            <c r="C1" t="s"><v>2</v></c>
            <c r="D1" t="s"><v>3</v></c>
        </row>
        <row r="2">
            <c r="A2" t="s"><v>4</v></c>
            <c r="B2" t="s"><v>5</v></c>
            <c r="C2" t="s"><v>6</v></c>
            <c r="D2" t="s"><v>7</v></c>
        </row>
        <row r="3">
            <c r="A3" t="s"><v>8</v></c>
            <c r="B3" t="s"><v>5</v></c>
            <c r="C3" t="s"><v>9</v></c>
            <c r="D3" t="s"><v>10</v></c>
        </row>
        <row r="4">
            <c r="A4" t="s"><v>11</v></c>
            <c r="B4" t="s"><v>12</v></c>
            <c r="C4" t="s"><v>13</v></c>
            <c r="D4" t="s"><v>14</v></c>
        </row>
        <row r="5">
            <c r="A5" t="s"><v>15</v></c>
            <c r="B5" t="s"><v>12</v></c>
            <c r="C5" t="s"><v>16</v></c>
            <c r="D5" t="s"><v>17</v></c>
        </row>
        <row r="7">
            <c r="A7" t="s"><v>18</v></c>
        </row>
        <row r="8">
            <c r="A8" t="s"><v>19</v></c>
            <c r="B8" t="s"><v>20</v></c>
            <c r="C8" t="s"><v>21</v></c>
            <c r="D8" t="s"><v>22</v></c>
        </row>
    </sheetData>
</worksheet>'''
    
    # Test case template worksheet  
    testcase_sheet = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
    <sheetData>
        <row r="1">
            <c r="A1" t="s"><v>0</v></c>
            <c r="B1" t="s"><v>23</v></c>
            <c r="C1" t="s"><v>24</v></c>
            <c r="D1" t="s"><v>25</v></c>
            <c r="E1" t="s"><v>26</v></c>
            <c r="F1" t="s"><v>27</v></c>
            <c r="G1" t="s"><v>28</v></c>
            <c r="H1" t="s"><v>29</v></c>
            <c r="I1" t="s"><v>30</v></c>
            <c r="J1" t="s"><v>31</v></c>
            <c r="K1" t="s"><v>32</v></c>
            <c r="L1" t="s"><v>33</v></c>
            <c r="M1" t="s"><v>34</v></c>
            <c r="N1" t="s"><v>35</v></c>
            <c r="O1" t="s"><v>36</v></c>
            <c r="P1" t="s"><v>37</v></c>
        </row>
        <row r="2">
            <c r="A2" t="s"><v>4</v></c>
            <c r="B2" t="s"><v>38</v></c>
            <c r="C2" t="s"><v>39</v></c>
            <c r="D2" t="s"><v>40</v></c>
            <c r="E2" t="s"><v>41</v></c>
            <c r="F2" t="s"><v>42</v></c>
            <c r="G2" t="s"><v>43</v></c>
            <c r="H2" t="s"><v>44</v></c>
            <c r="I2" t="s"><v>45</v></c>
            <c r="K2" t="s"><v>46</v></c>
            <c r="L2" t="s"><v>47</v></c>
            <c r="O2" t="s"><v>48</v></c>
        </row>
        <row r="3">
            <c r="A3" t="s"><v>8</v></c>
            <c r="B3" t="s"><v>38</v></c>
            <c r="C3" t="s"><v>49</v></c>
            <c r="D3" t="s"><v>50</v></c>
            <c r="E3" t="s"><v>51</v></c>
            <c r="F3" t="s"><v>42</v></c>
            <c r="G3" t="s"><v>43</v></c>
            <c r="H3" t="s"><v>52</v></c>
            <c r="I3" t="s"><v>53</v></c>
            <c r="K3" t="s"><v>46</v></c>
            <c r="L3" t="s"><v>47</v></c>
            <c r="O3" t="s"><v>48</v></c>
        </row>
    </sheetData>
</worksheet>'''
    
    # Basic styles
    styles = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
    <fonts count="1">
        <font>
            <sz val="11"/>
            <name val="Calibri"/>
        </font>
    </fonts>
    <fills count="2">
        <fill>
            <patternFill patternType="none"/>
        </fill>
        <fill>
            <patternFill patternType="gray125"/>
        </fill>
    </fills>
    <borders count="1">
        <border>
            <left/>
            <right/>
            <top/>
            <bottom/>
            <diagonal/>
        </border>
    </borders>
    <cellStyleXfs count="1">
        <xf numFmtId="0" fontId="0" fillId="0" borderId="0"/>
    </cellStyleXfs>
    <cellXfs count="1">
        <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>
    </cellXfs>
</styleSheet>'''
    
    # Basic theme
    theme = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<a:theme xmlns:a="http://schemas.openxmlformats.org/drawingml/2006/main" name="Office Theme">
    <a:themeElements>
        <a:clrScheme name="Office">
            <a:dk1>
                <a:sysClr val="windowText" lastClr="000000"/>
            </a:dk1>
            <a:lt1>
                <a:sysClr val="window" lastClr="FFFFFF"/>
            </a:lt1>
        </a:clrScheme>
        <a:fontScheme name="Office">
            <a:majorFont>
                <a:latin typeface="Calibri Light" panose="020F0302020204030204"/>
            </a:majorFont>
            <a:minorFont>
                <a:latin typeface="Calibri" panose="020F0502020204030204"/>
            </a:minorFont>
        </a:fontScheme>
        <a:fmtScheme name="Office">
            <a:fillStyleLst/>
            <a:lnStyleLst/>
            <a:effectStyleLst/>
            <a:bgFillStyleLst/>
        </a:fmtScheme>
    </a:themeElements>
</a:theme>'''
    
    # Write all files
    with open('[Content_Types].xml', 'w', encoding='utf-8') as f:
        f.write(content_types)
    
    with open('_rels/.rels', 'w', encoding='utf-8') as f:
        f.write(rels)
    
    with open('xl/_rels/workbook.xml.rels', 'w', encoding='utf-8') as f:
        f.write(workbook_rels)
    
    with open('xl/workbook.xml', 'w', encoding='utf-8') as f:
        f.write(workbook)
    
    with open('xl/sharedStrings.xml', 'w', encoding='utf-8') as f:
        f.write(shared_strings)
    
    with open('xl/worksheets/sheet1.xml', 'w', encoding='utf-8') as f:
        f.write(summary_sheet)
    
    with open('xl/worksheets/sheet2.xml', 'w', encoding='utf-8') as f:
        f.write(testcase_sheet)
    
    with open('xl/styles.xml', 'w', encoding='utf-8') as f:
        f.write(styles)
    
    with open('xl/theme/theme1.xml', 'w', encoding='utf-8') as f:
        f.write(theme)
    
    # Create the ZIP file (Excel format)
    with zipfile.ZipFile('ex.xlsx', 'w', zipfile.ZIP_DEFLATED) as zipf:
        zipf.write('[Content_Types].xml')
        zipf.write('_rels/.rels')
        zipf.write('xl/_rels/workbook.xml.rels')
        zipf.write('xl/workbook.xml')
        zipf.write('xl/sharedStrings.xml')
        zipf.write('xl/worksheets/sheet1.xml')
        zipf.write('xl/worksheets/sheet2.xml')
        zipf.write('xl/styles.xml')
        zipf.write('xl/theme/theme1.xml')
    
    # Clean up temporary files
    import shutil
    shutil.rmtree('xl')
    shutil.rmtree('_rels')
    os.remove('[Content_Types].xml')
    
    print("ex.xlsx created successfully!")
    print("Contains:")
    print("- サマリー sheet with format example")
    print("- テストケーステンプレート sheet with all standard columns")

create_xlsx_file()