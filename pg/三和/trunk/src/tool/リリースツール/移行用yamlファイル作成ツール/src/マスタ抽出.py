import json,os,sys
import config

# 現在のディレクトリからjsonファイルを取得
filePaths = config.filePath
for filePath in filePaths:
    filePath = os.path.join(config.rootPath,'json',filePath)
    if not os.path.isfile(filePath):
        print('ファイルが存在しない')
        sys.exit()
    else:
        print("処理を開始します。")

    json_open = open(filePath,'r',encoding="utf-8_sig")
    json_load = json.load(json_open)

    # サイトごと繰り返し
    sites = json_load["Sites"]
    for site in sites:
        
        # categorys = ["Styles","Scripts","Htmls","ServerScripts"]
        categorys = ["Htmls"]
        for category in categorys:
            # jsonにカテゴリーが存在するか
            if not category in site["SiteSettings"]:
                continue
            # フォルダの存在確認
            tmp_path = os.path.join(config.rootPath,config.siteData_folderPath,site["Title"],category)
            if not os.path.exists(tmp_path):
                os.makedirs(tmp_path)
            for script in site["SiteSettings"][category]:
                try:
                    txt_file = open(os.path.join(tmp_path , script["Title"]+".txt"),'w',encoding='utf-8')
                except:
                    continue

                if "Body" in script:
                    txt_file.write(script["Body"])
                txt_file.close()
