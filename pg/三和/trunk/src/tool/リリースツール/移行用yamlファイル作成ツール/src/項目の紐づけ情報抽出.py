import os,yaml,json,config

yaml_path = os.path.join(config.rootPath,"json","pleasanter_setting.development.yaml")

yml_open = open(yaml_path,'r',encoding="utf-8_sig")
yml_load = yaml.safe_load(yml_open)


json_dic = {}

for table in yml_load:
    data = yml_load[table]
    
    tmp_json = {}
    if 'KEYS' in data:
        tmp_json["KEYS"] = data["KEYS"]
        for param_key in data:
            if param_key in ['ID','Styles','Htmls','Scripts','ServerScripts','KEYS']:
                continue
            
            tmp_json[param_key] = data[param_key]
        
        json_dic[table] = tmp_json

tmp_path = os.path.join(config.rootPath,config.yaml_folderPath)
if not os.path.exists(tmp_path):
        os.makedirs(tmp_path)
with open(os.path.join(config.rootPath,'json', "項目の紐づけ.json"), 'w', encoding='utf-8_sig') as txt_file:
        json.dump(json_dic,txt_file,indent=4,ensure_ascii=False)
