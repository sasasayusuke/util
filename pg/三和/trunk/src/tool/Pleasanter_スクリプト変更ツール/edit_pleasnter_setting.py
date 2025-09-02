import requests
import sys
import os

########開発環境定数##########
# pleasanter_url = 'http://192.168.10.54/'
pleasanter_url = 'http://192.168.0.9/'
api_version = 1.1
api_key = '93ff7c4c971c67a96325c11c3d2ed73cade6029767c14b57f5baed6274eb8ecca2e83a82cdc4c3005b78f7b4baf72f0f70933186aa371f1b7027254de40fc527'
# scripts_code_path = R"C:\Users\NT-240014\work\python\Pleasanter_Scripts"
scripts_code_path = R"C:\Users\NT-240014\work\python\Pleasanter_Scripts_notePC"
######################

### PleasanterのテーブルIDを定義
MASTER_TABLE ={
	'物件入力': 3064532,
	'ヤオコー製品変換マスタ': 1841975,
	'三研実績入力': 1841976,
	'仕入先マスタ': 1841977,
	'会社マスタ': 1841978,
	'担当者マスタ': 3064531,
	'科目マスタ': 1841980,
	'税率マスタ': 1841981,
	'請求金額マスタ': 1841982,
	'配送先マスタ': 1841983,
	'ウエルシア物件内容マスタ': 1841984,
	'売上仕入単価マスタ': 1841985,
	'売掛金額マスタ': 1841986,
	'得意先マスタ': 1841987,
	'支払金額マスタ': 1841988,
	'棚番マスタ': 1841989,
	'納入先マスタ': 1841990,
	'補助科目マスタ': 1841991,
	'製品マスタ': 1841992,
	'買掛金額マスタ': 1841993,
	'工事担当マスタ': 1841994,
	'三和サービス実績入力': 1841996,
	'ウエルシア物件区分マスタ': 1841997,
	'入金区分マスタ':2190740,
	'支払区分マスタ':2190749,
	'部署マスタ':3064556,
	'人数按分マスタ': 3066123,
}
###

### PleasanterのテーブルIDを定義
OTHER_TABLE_FOLDER ={
	'見積・発注': 2714936,
	'仕入・支払': 2714950,
	'売上・入金': 2714958,
	'請求・売掛': 2714968,
	'支払・買掛': 2714989,
	'統計': 2714995,
	'経費': 2715007,
	'保守': 2715004,
	'仕入明細入力': 3064555,
	'売上入力明細': 3065886,
	'権限設定': 3065964,
}
###

#Pleasanterの設定を更新
def Edit_Pleasanter():
	print(f"HTMLスクリプトの向け先を選択してください　　1:開発環境　2:本番環境")
	n = int(input())
	if n != 1 and n != 2:
		sys.exit()
	else:
		if n == 1:
			dev_bool = False
			pro_bool = True
		else:
			dev_bool = True
			pro_bool = False

	master_f1 = open(scripts_code_path + '\マスタ\Htmls\dev_assets_required.txt', 'r', encoding='UTF-8')
	master_f2 = open(scripts_code_path + '\マスタ\Htmls\pro_assets_required.txt', 'r', encoding='UTF-8')
	text_data1 = master_f1.read()
	text_data2 = master_f2.read()
	for key in MASTER_TABLE.keys():
		# マスタ下のHTMLを更新
		url = pleasanter_url + f"api/items/{str(MASTER_TABLE[key])}/updatesitesettings"
		headers = {'Content-Type':'application/json','charset':'utf-8'}
		data = {
			"ApiVersion" : api_version,
			"ApiKey" : api_key,
			"Htmls": [
				{
					"Id": 1,
					"Title": "dev_assets_required",
					"HtmlPositionType": "HeadTop",
					"Body": text_data1,
					"HtmlAll": "true",
					"Disabled": dev_bool
				},
				{
					"Id": 2,
					"Title": "pro_assets_required",
					"HtmlPositionType": "HeadTop",
					"Body": text_data2,
					"HtmlAll": "true",
					"Disabled": pro_bool
				}
			]       
		}

		response = requests.post(url,headers=headers, json=data)
		##########
		print(key +  ':' + response.content.decode())
		##########
		master_f1.close()
		master_f2.close()

	# 各フォルダのHTMLを更新
	for key in OTHER_TABLE_FOLDER.keys():
		url = pleasanter_url + f"api/items/{str(OTHER_TABLE_FOLDER[key])}/updatesitesettings"
		f1 = open(scripts_code_path + '\\' + key + '\Htmls\dev_assets_required.txt', 'r', encoding='UTF-8')
		f2 = open(scripts_code_path + '\\' + key + '\Htmls\pro_assets_required.txt', 'r', encoding='UTF-8')
		f3 = open(scripts_code_path + '\\' + key + '\Htmls\dev_assets.txt', 'r', encoding='UTF-8')
		f4 = open(scripts_code_path + '\\' + key + '\Htmls\pro_assets.txt', 'r', encoding='UTF-8')
		text_data1 = f1.read()
		text_data2 = f2.read()
		text_data3 = f3.read()
		text_data4 = f4.read()
		data = {
			"ApiVersion" : api_version,
			"ApiKey" : api_key,
			"Htmls": [
				{
					"Id": 1,
					"Title": "dev_assets_required",
					"HtmlPositionType": "HeadTop",
					"HtmlAll": "true",
					"Body": text_data1,
					"Disabled": dev_bool
				},
				{
					"Id": 2,
					"Title": "pro_assets_required",
					"HtmlPositionType": "HeadTop",
					"HtmlAll": "true",
					"Body": text_data2,
					"Disabled": pro_bool
				},
				{
					"Id": 3,
					"Title": "dev_assets",
					"HtmlPositionType": "BodyScriptBottom",
					"HtmlAll": "true",
					"Body": text_data3,
					"Disabled": dev_bool
				},
				{
					"Id": 4,
					"Title": "pro_assets",
					"HtmlPositionType": "BodyScriptBottom",
					"HtmlAll": "true",
					"Body": text_data4,
					"Disabled": pro_bool
				}
			]
		}
		response = requests.post(url,headers=headers, json=data)
		##########
		print(key +  ':' + response.content.decode())
		##########
		f1.close()
		f2.close()
		f3.close()
		f4.close()

if __name__ == '__main__':
	print('【Pleasanter設定更新処理開始】')
	Edit_Pleasanter()
	print('【Pleasanter設定更新処理終了】')
