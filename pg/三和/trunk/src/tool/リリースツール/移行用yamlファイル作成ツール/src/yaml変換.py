import json,os,sys,yaml
import config


# SDBとのカラム紐づけ情報を取得
himoduke_file = open(os.path.join(config.rootPath,'json','項目の紐づけ.json'),'r',encoding='utf-8_sig')
himoduke_file = json.load(himoduke_file)

# 移行先のjsonデータを取得（テーブルID用）
target_json = []
for target_fileName in config.target_fileName:
    with open(os.path.join(config.rootPath,'json',target_fileName),'r',encoding='utf-8_sig') as target:
        tmp_json = json.load(target)
        target_json.extend(tmp_json["HeaderInfo"]["Convertors"])

site_json_arr = []
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
        tmp_tmp = list(filter(lambda x : x["SiteTitle"] == site["Title"],target_json))
        if len(tmp_tmp) != 1:
            print('{}のタイトルがない'.format(site["Title"]))
        id = tmp_tmp[0]["SiteId"]
        site_json = {
            site["Title"]:{
                "ID":id
                # "ID":site["SiteId"]
            }
        }

        # styles
        if 'Styles' in site["SiteSettings"]:
            styles_json = []
            for style in site["SiteSettings"]["Styles"]:
                tmp_style_json = {
                    "Id":style["Id"],
                    "Title":style["Title"],
                }
                # パラメータを繰り返す
                for key in style:
                    if key in ['Disabled','Delete']:
                        tmp_style_json[key] = style[key]
                    elif key in ["Body","Id","Title"]:
                        continue
                    else:
                        tmp_style_json["Style{}".format(key)] = style[key]
                styles_json.append(tmp_style_json)
            site_json[site["Title"]]["Styles"] = styles_json


        # scripts
        if 'Scripts' in site["SiteSettings"]:
            scripts_json = []
            for script in site["SiteSettings"]["Scripts"]:
                tmp_script_json = {
                    "Title":script["Title"],
                    "Id":script["Id"],
                }
                # パラメータを繰り返す
                for key in script:
                    if key in ['Disabled','Delete']:
                        tmp_script_json[key] = script[key]
                    elif key in ['Id','Title','Body']:
                        continue
                    else:
                        tmp_script_json["Script{}".format(key)] = script[key]
                scripts_json.append(tmp_script_json)
            site_json[site["Title"]]["Scripts"] = scripts_json

        # # serverScripts
        if 'ServerScripts' in site["SiteSettings"]:
            serverScripts_json = []
            for ServerScripts in site["SiteSettings"]["ServerScripts"]:
                tmp_serverScripts_json = {
                    "Id":ServerScripts["Id"],
                    "Title":ServerScripts["Title"],
                    "Name":ServerScripts["Name"]
                }
                # パラメータを繰り返す
                for key in ServerScripts:
                    if key in ["Delete","Id","Title","Name"]:
                        tmp_serverScripts_json[key] = ServerScripts[key]
                    elif key == "Body":
                        continue
                    else:
                        tmp_serverScripts_json["ServerScript{}".format(key)] = ServerScripts[key]
                serverScripts_json.append(tmp_serverScripts_json)
            site_json[site["Title"]]["ServerScripts"] = serverScripts_json

        # # Htmls
        # encoding.js以外はdisabledを反転
        if 'Htmls' in site["SiteSettings"]:
            htmls_json = []
            for html in site["SiteSettings"]["Htmls"]:
                tmp_htmls_json = {
                    "Id":html["Id"],
                    "Title":html["Title"],
                }
                for key in html:
                    if key == "Disabled":
                        pass
                    if key in ["Delete","Id","Title"]:
                        tmp_htmls_json[key] = html[key]
                    elif key == "Body":
                        continue
                    elif key == 'PositionType':
                        if html[key] == 1000:
                            tmp_htmls_json["HtmlPositionType"] = 'HeadTop'
                        elif html[key] == 1010:
                            tmp_htmls_json["HtmlPositionType"] = 'HeadBottom'
                        elif html[key] == 9000:
                            tmp_htmls_json["HtmlPositionType"] = 'BodyScriptTop'
                        elif html[key] == 9010:
                            tmp_htmls_json["HtmlPositionType"] = 'BodyScriptBottom'
                    else:
                        tmp_htmls_json["Html{}".format(key)] = html[key]
                    
                if not 'Disabled' in html and html["Title"] != 'encodingjs読み込み':
                    tmp_htmls_json['Disabled'] = True


                htmls_json.append(tmp_htmls_json)
            site_json[site["Title"]]["Htmls"] = htmls_json


        # processes
        if 'Processes' in site["SiteSettings"]:
            process_json = []
            for Process in site["SiteSettings"]["Processes"]:
                tmp_Processes_json = {
                    "Name":Process["Name"],
                    "Id":Process["Id"],
                    "CurrentStatus":Process["CurrentStatus"],
                    "ChangedStatus":Process["ChangedStatus"],
                    "OnClick":Process["OnClick"]
                }
                process_json.append(tmp_Processes_json)

            site_json[site["Title"]]["Processes"] = process_json
        
        # 項目の紐づけ情報
        if site["Title"] in himoduke_file:
            for param in himoduke_file[site["Title"]]:
                site_json[site["Title"]][param] = himoduke_file[site["Title"]][param]
                pass


        site_json_arr.append(site_json)

# yamlに変換し書き込み
yml_folderPath = os.path.join(config.rootPath,config.yaml_folderPath)
if not os.path.exists(yml_folderPath):
        os.makedirs(yml_folderPath)
with open(os.path.join(yml_folderPath, "yaml.yaml"), 'w', encoding='utf-8_sig') as txt_file:
    for site in site_json_arr:
        ym = yaml.dump(site,Dumper=yaml.CDumper,allow_unicode=True)#ymlへ変換
        txt_file.write(ym)
        txt_file.write("\n")
