# プロジェクトのルートパス
rootPath = r"C:\Users\NT-210107\Downloads\移行用yamlファイル作成ツール"

# Pleasanterから取得したサイトパッケージのファイル名
filePath = [
    r"マスタ_2025_03_31 18_03_35.json",
    r"メニュー（開発環境向け）_2025_04_01 11_20_36.json"
]

# 反映先のサイトパッケージ（テーブルID取得用）
target_fileName = [
    "移行先_メニュー.json",
    "移行先_マスタ.json"
]

# 書き込み先フォルダパス（ルートフォルダ直下に作成されます）
siteData_folderPath =   r"サイトデータ"   # Pleasanterに登録されているデータを格納するフォルダ
yaml_folderPath =       r"yaml"          # yamlファイルを出力するフォルダ