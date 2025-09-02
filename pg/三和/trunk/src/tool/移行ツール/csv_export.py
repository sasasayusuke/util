import pyodbc
import csv
import requests
import json
import os
import time
from decimal import Decimal
from natsort import natsorted
import logging

########開発環境定数##########
# driver='{SQL Server}'
# server = '192.168.10.54'
# database = 'SanwaSDB_241016'
# user_name = 'sa'
# password = '1qaz!QAZ'
# pleasanter_url = 'http://192.168.10.54/'
# api_key = '93ff7c4c971c67a96325c11c3d2ed73cade6029767c14b57f5baed6274eb8ecca2e83a82cdc4c3005b78f7b4baf72f0f70933186aa371f1b7027254de40fc527'
# csv_export_path = 'C:/Users/NT-240014/work/python'
######################

########本番環境定数##########
driver='{SQL Server}'
server = '192.168.0.7'
database = 'SanwaSDB'
user_name = 'sa'
password = 'yjy0354847577'
pleasanter_url = 'http://192.168.0.7/'
api_key = '93ff7c4c971c67a96325c11c3d2ed73cade6029767c14b57f5baed6274eb8ecca2e83a82cdc4c3005b78f7b4baf72f0f70933186aa371f1b7027254de40fc527'
csv_export_path = 'C:\SDT\移行\Pleasanter_データ移行ツール'
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
	'業種按分マスタ': 1841995,
	'三和サービス実績入力': 1841996,
	'ウエルシア物件区分マスタ': 1841997,
	'入金区分マスタ':2190740,
	'支払区分マスタ':2190749,
	'部署マスタ':3064556,
	'人数按分マスタ': 3066123,
}
###

##マスタ名,SQL,[CSVヘッダー],PleasanterのテーブルID
TM_list = [
	[
		'配送先マスタ'
		,'SELECT TM配送先.配送先CD,TM配送先.配送先CD,TM配送先.配送先名1,TM配送先.配送先名2,TM配送先.略称,TM配送先.フリガナ,TM配送先.郵便番号,TM配送先.住所1,TM配送先.住所2,TM配送先.電話番号,TM配送先.FAX番号,TM配送先.配送先担当者名,TM配送先.メモ,null,null,TM配送先.登録変更日 FROM TM配送先;'
		,["タイトル","配送先CD","配送先名1","配送先名2","略称","フリガナ","郵便番号","住所1","住所2","TEL","FAX","配送先担当者名","メモ","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['配送先マスタ'])
	]
	,[
		'会社マスタ'
		,"SELECT TM会社.会社ID,TM会社.社名1,TM会社.社名2,TM会社.郵便番号,TM会社.住所1,TM会社.住所2,TM会社.電話番号,TM会社.FAX番号,TM会社.代表者名,TM会社.期首月,TM会社.締日,TM会社.インボイス登録番号,'048-507-1130',TM会社.会社ID,TM会社.入力開始日,null,null,TM会社.登録変更日 FROM TM会社;"
		,["タイトル","社名1","社名2","郵便番号","住所1","住所2","電話番号","FAX","代表者名","期首月","締日","登録番号","コールセンター番号","会社ID","入力開始日","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['会社マスタ'])
	]
	,[
		'税率マスタ'
		,"SELECT '',TM税率.旧税率,TM税率.税率,TM税率.切替日付,null,null,TM税率.登録変更日 FROM TM税率;"
		,["タイトル","切替前税率","切替後税率","切替日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['税率マスタ'])
	]
	,[
		'科目マスタ'
		,'SELECT TM科目.科目CD,TM科目.科目CD,TM科目.科目名,TM科目.借方消費税区分,TM科目.貸方消費税区分,TM科目.集計CD,TM科目.按分区分,TM科目.順序,null,null,TM科目.登録変更日 FROM TM科目;'
		,["タイトル","科目CD","科目名","借方消費税区分","貸方消費税区分","集計CD","按分区分","順序","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['科目マスタ'])
	]
	,[
		'補助科目マスタ'
		,"SELECT TM補助科目.科目CD,TM補助科目.科目CD,TM科目.科目名,TM補助科目.補助CD,TM補助科目.補助科目名,TM補助科目.借方消費税区分,TM補助科目.貸方消費税区分,TM補助科目.按分区分,CONVERT(VARCHAR(10), TM補助科目.科目CD) + '__' + CONVERT(VARCHAR(10), TM補助科目.補助CD),null,null,TM補助科目.登録変更日 FROM TM補助科目 LEFT JOIN TM科目 ON TM補助科目.科目CD = TM科目.科目CD;"
		,["タイトル","科目CD","科目名","補助CD","補助科目名","借方消費税区分","貸方消費税区分","按分区分","主キー項目","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['補助科目マスタ'])
	]
	,[
		'ウエルシア物件区分マスタ'
		,'SELECT TMウエルシア物件区分.ウエルシア物件区分CD,TMウエルシア物件区分.ウエルシア物件区分CD,TMウエルシア物件区分.ウエルシア物件区分名,null,null,null FROM TMウエルシア物件区分;'
		,["タイトル","区分CD","ウエルシア物件区分名","作成者","更新者","更新日時"],
		str(MASTER_TABLE['ウエルシア物件区分マスタ'])
		]
	,[
		'ウエルシア物件内容マスタ'
		,'SELECT TMウエルシア物件内容.ウエルシア物件内容CD,TMウエルシア物件内容.ウエルシア物件内容CD,TMウエルシア物件内容.ウエルシア物件内容名,null,null,null FROM TMウエルシア物件内容;'
		,["タイトル","内容CD","ウエルシア物件内容名","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['ウエルシア物件内容マスタ'])
	]
	,[
		'工事担当マスタ'
		,'SELECT TM工事担当.工事担当CD,TM工事担当.工事担当CD,TM工事担当.工事担当名,TM工事担当.順序,null,null,null FROM TM工事担当;'
		,["タイトル","工事担当CD","工事担当名","順序","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['工事担当マスタ'])
	]
	,[
		'三研実績入力'
		,"SELECT '',TM三研情報.年月,TM三研情報.売上税抜金額_元,TM三研情報.原価税抜金額_元,TM三研情報.人件一般費_元,TM三研情報.営業外費_元,TM三研情報.元円比率,TM三研情報.売上税抜金額_円,TM三研情報.原価税抜金額_円,TM三研情報.人件一般費_円,TM三研情報.営業外費_円,null,null,null FROM TM三研情報;"
		,["タイトル","実績日付","売上税抜金額_元","原価税抜金額_元","人件一般費_元","営業外費_元","元円比率","売上税抜金額_円","原価税抜金額_円","人件一般費_円","営業外費_円","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['三研実績入力'])
	]
	,[
		'入金区分マスタ'
		,"SELECT TM入金区分.入金区分CD,TM入金区分.入金区分CD,TM入金区分.入金区分名,CASE TM入金区分.入金種別 WHEN 1 THEN '入金' WHEN 2 THEN '手形入金' WHEN 3 THEN '調整・値引' ELSE null END,TM入金区分.入金種別,null,null,null FROM TM入金区分;"
		,["タイトル","入金区分CD","入金区分名","入金種別","入金種別CD","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['入金区分マスタ'])
	]
	,[
		'支払区分マスタ'
		,"SELECT TM支払区分.支払区分CD,TM支払区分.支払区分CD,TM支払区分.支払区分名,CASE TM支払区分.支払種別 WHEN 1 THEN '支払' WHEN 2 THEN '手形支払' WHEN 3 THEN '調整・値引' ELSE null END,TM支払区分.支払種別,null,null,null FROM TM支払区分;"
		,["タイトル","支払区分CD","支払区分名","支払種別","支払種別CD","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['支払区分マスタ'])
	]
	,[
		'部署マスタ'
		,"SELECT TM部署.部署CD,TM部署.部署CD,TM部署.部署名,TM部署.順序,null,null,null FROM TM部署;"
		,["タイトル","部署CD","部署名","順序","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['部署マスタ'])
	]
	,[
		'人数按分マスタ'
		,"SELECT TM人数按分v2.部CD,TM人数按分v2.統計集計先CD,TM人数按分v2.集計先名, CONVERT(VARCHAR(10), TM人数按分v2.部CD) + '__' + CONVERT(VARCHAR(10), TM人数按分v2.統計集計先CD) + '__' + CONVERT(VARCHAR(10), TM人数按分v2.営業担当CD) + '__' + CONVERT(VARCHAR(10), TM人数按分v2.工事担当CD),TM人数按分v2.比率,TM人数按分v2.部CD,TM人数按分v2.営業担当CD,TM人数按分v2.工事担当CD,null,null,null FROM TM人数按分v2;"
		,["タイトル","統計集計先CD","集計先名","主キー項目","比率","部CD","営業担当CD","工事担当CD","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['人数按分マスタ'])
	]
	,[
		'担当者マスタ'
		,'SELECT TM担当者.担当者CD,TM担当者.担当者CD,TM担当者.担当者名,TM担当者.メールアドレス,TM担当者.部署CD,TM部署.部署名,TM担当者.問い合わせ先,TM担当者.順序,null,null,TM担当者.登録変更日 FROM TM担当者 LEFT JOIN TM部署 ON TM担当者.部署CD = TM部署.部署CD;'
		,["タイトル","担当者CD","担当者名","メールアドレス","部署CD","部署名","問い合わせ先","順序","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['担当者マスタ'])
	]
	,[
		'仕入先マスタ'
		,"SELECT TM仕入先.仕入先CD,TM仕入先.仕入先CD,TM仕入先.仕入先名1,TM仕入先.仕入先名2,TM仕入先.インボイス登録番号,TM仕入先.略称,TM仕入先.フリガナ,TM仕入先.郵便番号,TM仕入先.住所1,TM仕入先.住所2,TM仕入先.電話番号,TM仕入先.FAX番号,TM仕入先.仕入先担当者名,TM仕入先.メモ,TM仕入先.担当者CD,TM担当者.担当者名,TM仕入先.締日,TM仕入先.支払方法,'0:振込,1:集金,2:郵送,3:その他',TM仕入先.決算月,TM仕入先.支払様式,'0:当社請求書,1:検収請求書,2:EDP手書,3:手書明細式,4:手書一覧式',TM仕入先.支払サイクル,'0:当月',TM仕入先.支払区分CD,TM支払区分.支払区分名,CASE TM支払区分.支払種別 WHEN 1 THEN '支払' WHEN 2 THEN '手形支払' WHEN 3 THEN '調整・値引' ELSE null END,TM仕入先.税集計区分,'0:伝票単位　1:支払単位　2:明細単位　3:税対象外',TM仕入先.仕入端数,'0:四捨五入　1:切り上げ　2:切り捨て',TM仕入先.消費税端数,'0:四捨五入　1:切り上げ　2:切り捨て',TM仕入先.支払予定日,TM仕入先.導入支払金額,TM仕入先.導入買掛金額,TM仕入先.最終使用日,null,null,TM仕入先.登録変更日 FROM TM仕入先 LEFT JOIN TM担当者 ON TM仕入先.担当者CD = TM担当者.担当者CD LEFT JOIN TM支払区分 ON TM仕入先.支払区分CD = TM支払区分.支払区分CD;"
		,["タイトル","仕入先CD","仕入先名1","仕入先名2","登録番号","略称","フリガナ","郵便番号","住所１","住所２","TEL","FAX","仕入先担当者名","メモ","担当者CD","担当者名","締日","支払方法CD","支払方法","決算月","支払様式CD","支払様式","支払サイクルCD","支払サイクル","支払区分CD","支払区分1","支払区分2","税集計区分CD","税集計区分","仕入端数CD","仕入端数","消費税端数CD","消費税端数","支払予定日","導入支払金額","導入買掛金額","最終使用日","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['仕入先マスタ'])
	]
	,[
		'製品マスタ'
		,"SELECT TRIM(TM製品.製品NO),TRIM(TM製品.製品NO),TRIM(TM製品.仕様NO),TM製品.漢字名称,TM製品.カナ名称,TM製品.ベース色,TM製品.入出庫用CD,TM製品.単位名,TM製品.費用区分,TM製品.主仕入先CD,TM仕入先.仕入先名1,TM製品.売上税区分,'0:外税　1:内税　2:非課税',TM製品.仕入税区分,'0:外税　1:内税　2:非課税',TM製品.在庫区分,'0:在庫管理する　1:在庫管理しない',TM製品.サイズ変更区分,'0:変更不可　1:変更可',TM製品.取組区分,'0:一般製品　1:取組製品',TM製品.取組終了年月,'0:部材費　1:労務費',TRIM(TM製品.製品NO) + '__' + TRIM(TM製品.仕様NO),TM製品.W,TM製品.D1,TM製品.H1,TM製品.D,TM製品.D2,TM製品.H2,TM製品.H,TM製品.定価,TM製品_メモ.メモ,TM製品.廃盤FLG,null,null,null FROM TM製品 LEFT JOIN TM製品_メモ ON TM製品.製品NO = TM製品_メモ.製品NO AND TM製品.仕様NO = TM製品_メモ.仕様NO LEFT JOIN TM仕入先 ON TM製品.主仕入先CD = TM仕入先.仕入先CD;"
		,["タイトル","製品No","仕様No","漢字名称","カナ名称","ベース色","入出庫用コード","単位名","費用区分","主仕入先CD","主仕入先名","売上税区分CD","売上税区分","仕入税区分CD","仕入税区分","在庫区分CD","在庫区分","サイズ変更区分CD","サイズ変更区分","取組区分CD","取組区分","取組終了年月","費用区分名","主キー項目","W","D1","H1","D","D2","H2","H","定価","メモ","廃盤FLG","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['製品マスタ'])
		]
	,[
		'棚番マスタ'
		,'SELECT TM棚番.棚番名,TRIM(TM棚番.製品NO),TRIM(TM棚番.仕様NO),TM製品.漢字名称,TM棚番.棚番名,TM製品.W,TM製品.D,TM製品.H,TM製品.D1,TM製品.D2,TM製品.H1,TM製品.H2,null,null,null FROM TM棚番 LEFT JOIN TM製品 ON TM棚番.製品NO = TM製品.製品NO AND TM棚番.仕様NO = TM製品.仕様NO;'
		,["タイトル","製品No","仕様No","漢字名称","棚番名","W","D","H","D1","D2","H1","H2","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['棚番マスタ'])
		]
	,[
		'得意先マスタ'
		,"SELECT TM得意先.得意先CD,TM得意先.得意先CD,TM得意先.得意先名1,TM得意先.得意先名2,TM得意先.略称,TM得意先.フリガナ,TM得意先.郵便番号,TM得意先.住所1,TM得意先.住所2,TM得意先.電話番号,TM得意先.FAX番号,TM得意先.得意先担当者名,TM得意先.メモ,TM得意先.集計CD,tokui2.得意先名1,TM得意先.担当者CD,TM担当者.担当者名,TM得意先.締日,TM得意先.回収方法,'0:振込　1:集金　2:郵送　3:その他',TM得意先.決算月,TM得意先.請求様式,'0:当社請求書　1:検収請求書　2:EDP手書　3:手書明細式　4:手書一覧式',TM得意先.入金サイクル,'0:当月',TM得意先.入金区分CD,TM入金区分.入金区分名,CASE TM入金区分.入金種別 WHEN 1 THEN '入金' WHEN 2 THEN '手形入金' WHEN 3 THEN '調整・値引' ELSE null END,TM得意先.税集計区分,'0:伝票単位　1:請求単位　3:税対象外',TM得意先.売上端数,'0:四捨五入　1:切り上げ　2:切り捨て',TM得意先.消費税端数,'0:四捨五入　1:切り上げ　2:切り捨て',TM得意先.入金予定日,TM得意先.導入請求金額,TM得意先.導入売掛金額,TM得意先.最終使用日,null,null,TM得意先.登録変更日 FROM TM得意先 LEFT JOIN TM担当者 ON TM得意先.担当者CD = TM担当者.担当者CD LEFT JOIN TM入金区分 ON TM得意先.入金区分CD = TM入金区分.入金区分CD LEFT JOIN TM得意先 as tokui2 ON TM得意先.集計CD = tokui2.得意先CD;"
		,["タイトル","得意先CD","得意先名1","得意先名2","略称","フリガナ","郵便番号","住所1","住所2","電話番号","FAX番号","得意先担当者名","メモ","集計CD","集計","担当者CD","担当者名","締日","回収方法CD","回収方法","決算月","請求様式CD","請求様式","入金サイクルCD","入金サイクル","入金区分CD","入金区分１","入金区分２","税集計区分CD","税集計区分","売上端数CD","売上端数","消費税端数CD","消費税端数","入金予定日","導入請求金額","導入売掛金額","最終使用日","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['得意先マスタ'])
	]
	,[
		'納入先マスタ'
		,"SELECT TM納入先.納入先CD,TM納入先.得意先CD,TM得意先.得意先名1,TM得意先.得意先名2,TM納入先.納入先CD,TM納入先.納入先名1,TM納入先.納入先名2,TM納入先.略称,TM納入先.表示用店舗名,TM納入先.フリガナ,TM納入先.郵便番号,TM納入先.住所1,TM納入先.住所2,TM納入先.電話番号,TM納入先.FAX番号,TM納入先.納入先担当者名,TM納入先.メモ,TM得意先.担当者CD,TM担当者.担当者名,TM納入先.得意先CD + '__' + TM納入先.納入先CD,TM納入先.最終使用日,null,null,TM納入先.登録変更日 FROM TM納入先 LEFT JOIN TM得意先 ON TM納入先.得意先CD = TM得意先.得意先CD LEFT JOIN TM担当者 ON TM得意先.担当者CD = TM担当者.担当者CD;"
		,["タイトル","得意先CD","得意先名1","得意先名2","納入先CD","納入先名1","納入先名2","略称","表示用店舗名","フリガナ","郵便番号","住所1","住所2","電話番号","FAX番号","納入先担当者名","メモ","担当者CD","担当者名","主キー項目","最終使用日","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['納入先マスタ'])
		]
	,[
		'売上仕入単価マスタ'
		,"SELECT TM売上仕入単価.得意先CD,TM売上仕入単価.得意先CD,TM得意先.得意先名1,TM売上仕入単価.仕入先CD,TM仕入先.仕入先名1,TM売上仕入単価.PC区分,TRIM(TM売上仕入単価.仕様NO),TM製品.漢字名称,TRIM(TM売上仕入単価.製品NO),TM売上仕入単価.得意先CD + '__' + TM売上仕入単価.仕入先CD + '__' + TM売上仕入単価.PC区分 + '__' + TRIM(TM売上仕入単価.製品NO) + '__' + TRIM(TM売上仕入単価.仕様NO),TM売上仕入単価.大口売上単価,TM売上仕入単価.大口仕入単価,TM売上仕入単価.小口売上単価,TM売上仕入単価.小口仕入単価,TM製品.D2,TM製品.H1,TM製品.H2,TM製品.W,TM製品.D,TM製品.H,TM製品.D1,null,TM売上仕入単価.初期登録日,null,null,TM売上仕入単価.登録変更日 FROM TM売上仕入単価 LEFT JOIN TM製品 ON TM売上仕入単価.製品NO = TM製品.製品NO AND TM売上仕入単価.仕様NO = TM製品.仕様NO LEFT JOIN TM得意先 ON TM売上仕入単価.得意先CD = TM得意先.得意先CD LEFT JOIN TM仕入先 ON TM売上仕入単価.仕入先CD = TM仕入先.仕入先CD;"
		,["タイトル","得意先CD","得意先名","仕入先CD","仕入先名","PC区分","仕様No","名称","製品No","主キー項目","大口売上単価","大口仕入単価","小口売上単価","小口仕入単価","D2","H1","H2","W","D","H","D1","初期登録日","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['売上仕入単価マスタ'])
	]
	,[
		'ヤオコー製品変換マスタ'
		,"SELECT TM製品変換.得意先CD,TM製品変換.得意先CD,TM得意先.得意先名1,TRIM(TM製品変換.仕様NO),TM製品.漢字名称,TM製品変換.他社部門CD,TM製品変換.他社製品CD,TRIM(TM製品変換.製品NO),TM製品変換.得意先CD + '__' + TRIM(TM製品変換.製品NO) + '__' + TRIM(TM製品変換.仕様NO) + '__' + TM製品変換.他社部門CD,TM製品.W,TM製品.D,TM製品.H,TM製品.D1,TM製品.D2,TM製品.H1,TM製品.H2,null,null,TM製品変換.登録変更日 FROM TM製品変換 LEFT JOIN TM製品 ON TM製品変換.製品NO = TM製品.製品NO AND TM製品変換.仕様NO = TM製品.仕様NO LEFT JOIN TM得意先 ON TM製品変換.得意先CD = TM得意先.得意先CD;"
		,["タイトル","得意先CD","得意先名","仕様NO","名称","部門CD","他社製品CD","製品NO","主キー項目","W","D","H","D1","D2","H1","H2","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['ヤオコー製品変換マスタ'])
	]
	,[
		'物件入力'
		,"SELECT TD物件情報.物件番号,TD物件情報.物件番号,TD物件情報.物件名,TD物件情報.物件略称,TD物件情報.得意先CD,TD物件情報.得意先名1,TD物件情報.得意先名2,TD物件情報.得TEL,TD物件情報.得FAX,TD物件情報.得意先担当者,TD物件情報.集計CD,tokui2.略称,TD物件情報.納入得意先CD,TD物件情報.納入先CD,TD物件情報.納入先名1,TD物件情報.納入先名2,TD物件情報.郵便番号,TD物件情報.住所1,TD物件情報.住所2,TD物件情報.納TEL,TD物件情報.納FAX,TD物件情報.納入先担当者,TD物件情報.担当者CD,TM担当者.担当者名,TD物件情報.部署CD,TM部署.部署名,TD物件情報.工事担当CD,TM工事担当.工事担当名,TD物件情報.予定業種区分,TD物件情報.予定物件種別,mitumori.合計金額,mitumori.出精値引,mitumori.原価合計,mitumori.原価率,TD物件情報.物件登録日付,TD物件情報.予定納期S,TD物件情報.予定納期E,TD物件情報.予定オープン日,TD物件情報.予定受付日付,TD物件情報.予定完工日付,TD物件情報.予定請求予定日付,null,null,null FROM TD物件情報 LEFT JOIN TM担当者 ON TD物件情報.担当者CD = TM担当者.担当者CD LEFT JOIN TM部署 ON TD物件情報.部署CD = TM部署.部署CD LEFT JOIN TM工事担当 ON TD物件情報.工事担当CD = TM工事担当.工事担当CD LEFT JOIN TM得意先 as tokui2 ON TD物件情報.集計CD = tokui2.得意先CD LEFT JOIN (select 物件番号,sum(合計金額) as 合計金額,sum(出精値引) as 出精値引,sum(原価合計) as 原価合計,CASE WHEN sum(合計金額) = 0 THEN 0 ELSE sum(原価合計)/sum(合計金額)*100 END as 原価率 from TD見積 GROUP BY 物件番号) as mitumori ON TD物件情報.物件番号 = mitumori.物件番号 ORDER BY TD物件情報.物件番号;"
		,["タイトル","物件番号","物件名","物件略称","得意先CD","得意先名1","得意先名2","得TEL","得FAX","得意先担当者","集計CD","集計","納入得意先CD","納入先CD","納入先名1","納入先名2","郵便番号","住所1","住所2","納TEL","納FAX","納入先担当者","担当者CD","担当者名","部署CD","部署名","工事担当CD","工事担当者名","予定業種区分","予定物件種別","売上合計","出精値引き","原価合計","原価率","物件登録日付","予定納期S","予定納期E","予定オープン日","予定受付日付","予定完工日付","予定請求予定日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['物件入力'])
	]
	,[
		'請求金額マスタ'
		,"SELECT TM請求金額.得意先CD,TM請求金額.得意先CD,TM得意先.得意先名1,TM請求金額.得意先CD + '__' + CONVERT(NVARCHAR,TM請求金額.請求日付,111),TM請求金額.前月残高,TM請求金額.入金金額,TM請求金額.調整金額,TM請求金額.売上金額,TM請求金額.返品金額,TM請求金額.訂正金額,TM請求金額.消費税額,TM請求金額.当月残高,TM請求金額.伝票枚数,TM請求金額.請求日付,TM請求金額.請求開始日付,TM請求金額.請求終了日付,null,null,null FROM TM請求金額 LEFT JOIN TM得意先 ON TM請求金額.得意先CD = TM得意先.得意先CD;"
		,["タイトル","得意先CD","得意先名","主キー項目","前月残高","入金金額","調整金額","売上金額","返品金額","訂正金額","消費税額","当月残高","伝票枚数","請求日付","請求開始日付","請求終了日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['請求金額マスタ'])
	]
	,[
		'売掛金額マスタ'
		,"SELECT TM売掛金額.得意先CD,TM売掛金額.得意先CD,TM得意先.得意先名1,TM売掛金額.得意先CD + '__' + CONVERT(NVARCHAR,TM売掛金額.売掛日付,111),TM売掛金額.前月残高,TM売掛金額.入金金額,TM売掛金額.調整金額,TM売掛金額.売上金額,TM売掛金額.返品金額,TM売掛金額.訂正金額,TM売掛金額.消費税額,TM売掛金額.当月残高,TM売掛金額.伝票枚数,TM売掛金額.非課税金額,TM売掛金額.売掛開始日付,TM売掛金額.売掛終了日付,TM売掛金額.売掛日付,null,null,null FROM TM売掛金額 LEFT JOIN TM得意先 ON TM売掛金額.得意先CD = TM得意先.得意先CD;"
		,["タイトル","得意先CD","得意先名","主キー項目","前月残高","入金金額","調整金額","売上金額","返品金額","訂正金額","消費税額","当月残高","伝票枚数","非課税金額","売掛開始日付","売掛終了日付","売掛日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['売掛金額マスタ'])
	]
	,[
		'支払金額マスタ'
		,"SELECT TM支払金額.仕入先CD,TM支払金額.仕入先CD,TM仕入先.仕入先名1,TM支払金額.仕入先CD + '__' + CONVERT(NVARCHAR,TM支払金額.支払日付,111),TM支払金額.前月残高,TM支払金額.支払金額,TM支払金額.調整金額,TM支払金額.仕入金額,TM支払金額.返品金額,TM支払金額.訂正金額,TM支払金額.消費税額,TM支払金額.当月残高,TM支払金額.伝票枚数,TM支払金額.支払日付,TM支払金額.支払開始日付,TM支払金額.支払終了日付,null,null,null FROM TM支払金額 LEFT JOIN TM仕入先 ON TM支払金額.仕入先CD = TM仕入先.仕入先CD;"
		,["タイトル","仕入先CD","仕入先名","主キー項目","前月残高","支払金額","調整金額","仕入金額","返品金額","訂正金額","消費税額","当月残高","伝票枚数","支払日付","支払開始日付","支払終了日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['支払金額マスタ'])
	]
	,[
		'買掛金額マスタ'
		,"SELECT TM買掛金額.仕入先CD,TM買掛金額.仕入先CD,TM仕入先.仕入先名1,TM買掛金額.仕入先CD + '__' + CONVERT(NVARCHAR,TM買掛金額.買掛日付,111),TM買掛金額.前月残高,TM買掛金額.支払金額,TM買掛金額.調整金額,TM買掛金額.仕入金額,TM買掛金額.返品金額,TM買掛金額.訂正金額,TM買掛金額.消費税額,TM買掛金額.当月残高,TM買掛金額.非課税金額,TM買掛金額.伝票枚数,TM買掛金額.買掛開始日付,TM買掛金額.買掛終了日付,TM買掛金額.買掛日付,null,null,null FROM TM買掛金額 LEFT JOIN TM仕入先 ON TM買掛金額.仕入先CD = TM仕入先.仕入先CD;"
		,["タイトル","仕入先CD","仕入先名","主キー項目","前月残高","支払金額","調整金額","仕入金額","返品金額","訂正金額","消費税額","当月残高","非課税金額","伝票枚数","買掛開始日付","買掛終了日付","買掛日付","作成者","更新者","更新日時"]
		,str(MASTER_TABLE['買掛金額マスタ'])
	]
]
######################

#csvファイルをエクスポート
def Csv_Export():
	#CSV出力先のフォルダ作成
	os.mkdir(csv_export_path + '/インポートcsv')

	#DB接続
	connect= pyodbc.connect('DRIVER='+driver+';SERVER='+server+';PORT=1433;DATABASE='+database+';UID='+ user_name +';PWD='+ password)
	cursor = connect.cursor()

	#CSV作成
	for i in range(len(TM_list)):
		logger.debug('CSV出力開始：' + TM_list[i][0])
		cursor.execute( TM_list[i][1] )
		rows = cursor.fetchall()
		csv_cnt = 1
		outf = open(csv_export_path + '/インポートcsv/' + str(i) + '_' + TM_list[i][0] + '_' + str(csv_cnt) + '.csv', 'w', newline='')
		writer = csv.writer(outf,delimiter=',',quotechar='"',quoting=csv.QUOTE_ALL)
		writer.writerow(TM_list[i][2])
		for j_cnt,j in enumerate(rows):
			if j_cnt % 3000 == 0 and j_cnt !=0:
				outf.close()
				csv_cnt += 1
				outf = open(csv_export_path + '/インポートcsv/' + str(i) + '_'  + TM_list[i][0] + "_" + str(csv_cnt) + ".csv", 'w', newline='')
				writer = csv.writer(outf,delimiter=',',quotechar='"',quoting=csv.QUOTE_ALL)
				writer.writerow(TM_list[i][2])
			j_list = list(j)
			for k in range(len(j_list)):
				if isinstance(j_list[k], Decimal):
					j_list[k] = float(j_list[k])
				j_list[k] = (str(j_list[k])).replace('\r\n', '')
				if j_list[k] == 'None':
					j_list[k] = ''
			writer.writerow(list(j_list))
		outf.close()
		logger.debug('CSV出力終了：' + TM_list[i][0])

		

#Pleasanterに登録
def Api_Import():
	dir_path = csv_export_path + '/インポートcsv'
	files_file = [
		f for f in os.listdir(dir_path) if os.path.isfile(os.path.join(dir_path, f))
	]
	for file_name in natsorted(files_file):
		##########
		logger.debug(file_name + ':処理開始-------------------------------')
		start_time = time.time()
		##########
		idx = file_name.find('_')
		mt_num = int(file_name[:idx])
		site_num = TM_list[mt_num][3]
		url = pleasanter_url + f"api/items/{site_num}/import"
		filePath = csv_export_path + f"/インポートcsv/{file_name}"
		data = {
			"parameters": json.dumps({
				"ApiVersion" : 1.1,
				"ApiKey" : api_key,
				"Encoding" : "Shift-JIS",
				"UpdatableImport" : False,
				"Key" : "Id"
			})            
		}
		files = {
			"file":(file_name, open(filePath,"rb"), "text/csv")
		}
		response = requests.post(url, data, files=files)
		##########
		logger.debug(response.content.decode())
		end_time = time.time()
		time_diff = end_time - start_time
		logger.debug(file_name +  ':' + str(time_diff) + 's')
		logger.debug(file_name +  ':処理終了-------------------------------')
		##########

#Pleasanterのデータ削除
def Api_Bulk_Delete():
	for site in TM_list:
		##########
		logger.debug(site[0] + ':処理開始-------------------------------')
		start_time = time.time()
		##########
		site_num = site[3]
		site_name = site[0]
		url = pleasanter_url + f"api/items/{site_num}/bulkdelete"
		headers = {'Content-Type':'application/json','charset':'utf-8'}
		data = {
			"ApiVersion": 1.1,
			"ApiKey": "93ff7c4c971c67a96325c11c3d2ed73cade6029767c14b57f5baed6274eb8ecca2e83a82cdc4c3005b78f7b4baf72f0f70933186aa371f1b7027254de40fc527",
			"view": {
				"ColumnFilterHash": {
					"Status": "[]"
				}
			},
			"PhysicalDelete": "true"
		}
		response = requests.post(url, headers=headers, json=data)
		##########
		logger.debug(response.content.decode())
		end_time = time.time()
		time_diff = end_time - start_time
		logger.debug(site_name +  ':' + str(time_diff) + 's')
		logger.debug(site_name +  ':処理終了-------------------------------')
		##########

# Pleasanterのマスタの同期処理を停止する
def Stop_Sync():
	for key in MASTER_TABLE.keys():
		url = pleasanter_url + f"api/items/{str(MASTER_TABLE[key])}/updatesitesettings"
		headers = {'Content-Type':'application/json','charset':'utf-8'}
		data = {
			"ApiVersion" : 1.1,
			"ApiKey" : api_key,
			"ServerScripts": [
				{
					"Id": 1,
					"Title": "createSync.js",
					"Name": "作成同期",
					"ServerScriptAfterCreate": False
				},
				{
					"Id": 2,
					"Title": "updateSync.js",
					"Name": "更新同期",
					"ServerScriptBeforeUpdate": False
				},
				{
					"Id": 3,
					"Title": "deleteSync.js",
					"Name": "削除同期",
					"ServerScriptBeforeDelete": False
				}
			]       
		}

		response = requests.post(url,headers=headers, json=data)
		##########
		logger.debug(key +  ':' + response.content.decode())
		##########

# Pleasanterのマスタの同期処理を有効化する
def Start_Sync():
	for key in MASTER_TABLE.keys():
		url = pleasanter_url + f"api/items/{str(MASTER_TABLE[key])}/updatesitesettings"
		headers = {'Content-Type':'application/json','charset':'utf-8'}
		data = {
			"ApiVersion" : 1.1,
			"ApiKey" : api_key,
			"ServerScripts": [
				{
					"Id": 1,
					"Title": "createSync.js",
					"Name": "作成同期",
					"ServerScriptAfterCreate": True
				},
				{
					"Id": 2,
					"Title": "updateSync.js",
					"Name": "更新同期",
					"ServerScriptBeforeUpdate": True
				},
				{
					"Id": 3,
					"Title": "deleteSync.js",
					"Name": "削除同期",
					"ServerScriptBeforeDelete": True
				}
			]       
		}

		response = requests.post(url,headers=headers, json=data)
		##########
		logger.debug(key +  ':' + response.content.decode())
		##########

if __name__ == '__main__':
	# ロガーの作成
	logger = logging.getLogger('my_logger')
	logger.setLevel(logging.DEBUG)

	# ファイルハンドラの設定
	file_handler = logging.FileHandler(R'C:\SDT\移行\Pleasanter_データ移行ツール\pleasanter.log')
	file_handler.setLevel(logging.DEBUG)

	# ログフォーマッタの設定
	formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')
	file_handler.setFormatter(formatter)

	# ハンドラをロガーに追加
	logger.addHandler(file_handler)
	try:
		logger.debug('【CSV出力処理開始】')
		Csv_Export()
		logger.debug('【CSV出力処理終了】')
		logger.debug('【Pleasanter同期停止開始】')
		Stop_Sync()
		logger.debug('【Pleasanter同期停止終了】')
		logger.debug('【Pleasanter削除処理開始】')
		Api_Bulk_Delete()
		logger.debug('【Pleasanter削除処理終了】')
		logger.debug('【Pleasanterインポート処理開始】')
		Api_Import()
		logger.debug('【Pleasanterインポート処理終了】')
		logger.debug('【Pleasanter同期開始】')
		Start_Sync()
		logger.debug('【Pleasanter同期終了】')
	except Exception as e:
		logger.error('-----------下記のエラーが発生しました----------')
		logger.error(e)
		logger.error('----------------------------------------------')
