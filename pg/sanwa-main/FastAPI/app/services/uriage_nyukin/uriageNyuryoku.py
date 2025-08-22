import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse,JSONResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell, relative_reference_copy, ExcelPDFConverter
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.service_utils import ClsTokuisaki, ClsOutputRireki,ClsDates
from dateutil.relativedelta import relativedelta

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class uriageNyuryokuService(BaseService):
    def display(self,request,session):
        pass
    def execute(self,request,session):
        pass
    def render(self,request,session):
        
        logger.info('【start】売上入力レンダリング処理')
        stored_name = "usp_HD0701売上明細抽出"
        params = request.params
        stored_params = {k: v for k, v in params.items() if k.startswith('@')}
        stored_results = dict(SQLExecutor(session).execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        # 仕分名称取得
        query = """
            SELECT * FROM TD見積シート内訳名称 
            WHERE 見積番号 = {}
            ORDER BY 仕分番号
        """.format(params["@i見積番号"])
        result = dict(SQLExecutor(session).execute_query(query=query))

        # 売上日付
        if params.get('@i処理区分') == '0':
            cDates = ClsDates()
            if cDates.GetbyId(session,"売掛月次更新日") == False:
                sales_date = datetime.datetime.strftime(datetime.date.today(),'%Y-%m-%d')
            else:
                sales_date = datetime.datetime(cDates.更新日付.year, cDates.更新日付.month, 1) + relativedelta(months=1)
                sales_date = datetime.datetime.strftime(sales_date,'%Y-%m-%d')
        else:
            sales_date = params["売上日付"]

        headers = [
            {

                "label": "処理区分",
                "accessor": "process_cd",
                "value": params.get('@i処理区分', ''),
                "value2": params.get('処理区分名', ''),
                "position": "left",
            },
            {
                "label": "抽出区分",
                "accessor": "extraction_no",
                "value": params.get('@i抽出区分', ''),
                "value2": params.get('抽出区分名', ''),
                "position": "left",
            },
            {
                "label": "見積番号",
                "accessor": "estimate_no",
                "value": params.get('@i見積番号', ''),
                "value2": params.get('見積件名', ''),
                "position": "left",
            },
            {
                "label": "得意先",
                "accessor": "custmer_no",
                "value": params.get('得意先CD', ''),
                "value2": params.get('得意先名', ''),
                "position": "left",
            },
            {
                "label": "納入先",
                "accessor": "delivery_no",
                "value": params.get('納入得意先CD', ''),
                "value2": params.get('納入先CD', ''),
                "value3": params.get('納入先名', ''),
                "position": "left",
            },
            {
                "label": "売上日付",
                "accessor": "sales_date",
                "value": sales_date,
                "position": "left",
            },
            {
                "label": "外税対象額",
                "accessor": "tax_exclusive_amount",
                "position": "right",
            },
            {
                "label": "外税",
                "accessor": "tax_exclusive",
                "position": "right",
            },
            {
                "label": "非課税金額",
                "accessor": "non_taxable_amount",
                "position": "right",
            },
            {
                "label": "合計金額",
                "accessor": "sum_all",
                "position": "right",
            }
        ]
        
        columns=[
            {
                "label": "No",
                "accessor": "売上明細行番号",
                "type": "text",
                "width": "40px",
                "align":'center',
                "views": ["仕入情報", "製品情報", "エラー情報"], 
            },
            {
                "label": "CK",
                "accessor": "CHECK",
                "type": "checkbox",
                "width": "40px",
                're-render':True,
                "editable": True if params["更新フラグ"]=='true' else False,
                "description": "チェック：売上",
            },
            {
                "label": "伝区",
                "accessor": "伝票区分",
                "type": "text",
                "width": "60px",
                're-render':True,
                "editable": True if params["更新フラグ"]=='true' else False,
                "description": "伝票区分を入力して下さい。　1:仕入 2:返品 3:単価訂正 ACS:コメント",
                "values":["1","2","A","S","C"],
                "maxLength":1
            },
            {
                "label": "区分",
                "accessor": "見積明細区分",
                "type": "text",
                "width": "40px",
                "align":"center"
            },
            {
                "label": "仕入日付",
                "accessor": "仕入日付",
                "type": "text",
                "width": "90px",
                "align":"center"
            },
            {
                "label": "納品日付しまむら",
                "accessor": "他社納品日付",
                "type": "text",
                "width": "90px",
                "align":"center"
            },
            {
                "label": "伝票番号しまむら",
                "accessor": "他社伝票番号",
                "type": "text",
                "width": "90px",
                "align":"center"
            },
            # {
            #     "label": "特区",
            #     "accessor": "特別区分",
            #     "type": "text",
            #     "width": "40px",
            # },
            {
                "label": "SP",
                "accessor": "SP区分",
                "type": "text",
                "width": "40px",
                "align":"center"
            },
            {
                "label": "PC区分",
                "accessor": "PC区分",
                "type": "text",
                "width": "40px",
                "align":"center"
            },
            {
                "label": "製品No",
                "accessor": "製品NO",
                "type": "text",
                "width": "80px",
            },
            {
                "label": "仕様No",
                "accessor": "仕様NO",
                "type": "text",
                "width": "80px",
            },
            {
                "label": "ベース色",
                "accessor": "ベース色",
                "type": "text",
                "width": "80px",
                "views":["製品情報"]
            },
            {
                "label": "名称",
                "accessor": "漢字名称",
                "type": "text",
                "width": "300px",
            },
            {
                "label": "W",
                "accessor": "W",
                "type": "number",
                "width": "60px",
                # "views":['仕入情報',]
            },
            {
                "label": "D",
                "accessor": "D",
                "type": "number",
                "width": "60px",
            },
            {
                "label": "H",
                "accessor": "H",
                "type": "number",
                "width": "60px",
            },
            {
                "label": "D1",
                "accessor": "D1",
                "type": "number",
                "width": "60px",
                "views":['製品情報']
            },
            {
                "label": "D2",
                "accessor": "D2",
                "type": "number",
                "width": "60px",
                "views":['製品情報']
            },
            {
                "label": "H1",
                "accessor": "H1",
                "type": "number",
                "width": "60px",
                "views":['製品情報']
            },
            {
                "label": "H2",
                "accessor": "H2",
                "type": "number",
                "width": "60px",
                "views":['製品情報']
            },
            {
                "label": "エラー内容",
                "accessor": "エラー内容",
                "type": "text",
                "width": "250px",
                "views":['エラー情報']
            },
            {
                "label": "定価",
                "accessor": "定価",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "U",
                "accessor": "U区分",
                "type": "text",
                "width": "40px",
                "align":"center"
            },
            {
                "label": "原価",
                "accessor": "仕入単価",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "仕入％",
                "accessor": "仕入率",
                "type": "number",
                "width": "60px",
            },
            {
                "label": "売価",
                "accessor": "売上単価",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "売上％",
                "accessor": "売上率",
                "type": "number",
                "width": "60px",
            },
            {
                "label": "M",
                "accessor": "M区分",
                "type": "text",
                "width": "40px",
                "align":"center"
            },
            {
                "label": "数量",
                "accessor": "売上数量",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "単位",
                "accessor": "単位名",
                "type": "text",
                "width": "50px",
                "views":['製品情報']
            },
            {
                "label": "金額",
                "accessor": "売上金額",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "仕入金額",
                "accessor": "仕入金額",
                "type": "number",
                "width": "100px",
            },
            # {
            #     "label": "税区分",
            #     "accessor": "売上税区分",
            #     "type": "text",
            #     "width": "60px",
            # },
            # {
            #     "label": "消費税",
            #     "accessor": "消費税額",
            #     "type": "number",
            #     "width": "100px",
            # },
            {
                "label": "仕入業者",
                "accessor": "仕入業者CD",
                "type": "text",
                "width": "60px",
            },
            {
                "label": "仕入業者名",
                "accessor": "仕入業者名",
                "type": "text",
                "width": "200px",
            },
            {
                "label": "出荷元",
                "accessor": "仕入先CD",
                "type": "text",
                "width": "60px",
            },
            {
                "label": "出荷元名",
                "accessor": "仕入先名",
                "type": "text",
                "width": "200px",
            },
            {
                "label": "送り先",
                "accessor": "配送先CD",
                "type": "text",
                "width": "60px",
            },
            {
                "label": "送り先名",
                "accessor": "配送先名",
                "type": "text",
                "width": "200px",
            },
            {
                "label": "社内在庫",
                "accessor": "社内在庫数",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "客先在庫",
                "accessor": "客先在庫数",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "転用",
                "accessor": "転用数",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "発注調整",
                "accessor": "発注調整数",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "発注数",
                "accessor": "発注数",
                "type": "number",
                "width": "100px",
            },
            {
                "label": "Σ",
                "accessor": "総数量",
                "type": "number",
                "width": "100px",
            },
            # {
            #     "label": "製品区分",
            #     "accessor": "製品区分",
            #     "type": "text",
            #     "width": "60px",
            # },
            # {
            #     "label": "見積明細",
            #     "accessor": "見積明細連番",
            #     "type": "number",
            #     "width": "100px",
            # },
            
        ]

        # 仕分
        for i in range(1,31):
            if len(result["results"]) >= i:
                tmp = result["results"][i-1]["略称"]
                columns.append({
                    "label": tmp,
                    "accessor": "仕分数{}".format(i),
                    "type": "number",
                    "width": "60px",
                })
            else:
                # tmp = "{}".format(i)
                pass
            # columns.append({
            #     "label": tmp,
            #     "accessor": "仕分数{}".format(i),
            #     "type": "number",
            #     "width": "60px",
            # })
            
        columns.append(
            {
                "label": "仕入済数",
                "accessor": "仕入済数",
                "type": "number",
                "width": "80px",
            },
        )
        data = stored_results.get("results", [[]])[0]

        gUcnt = 0
        for row in data:
            # 初期表示はすべてのチェックがtrueの為追加
            row['CHECK'] = True

            # 日付のフォーマット
            date = row['仕入日付']
            if(date != None):
                row['仕入日付'] = date.strftime("%Y/%m/%d")

            date = row['他社納品日付']
            if(date != None):
                row['他社納品日付'] = date.strftime("%Y/%m/%d")


            if  row["U区分"] in ['U','B'] and row["売上数量"] != "0":
                gUcnt += 1

        return{
            "content": {
                "headers": headers,
                "columns": columns,
                "data": data,
                "gUcnt":gUcnt
            },
            "headers":{
                'Download-Skip': "yes"
            }
        }
    

    def upload(self,request,session):
        logger.info('【START】売上明細入力登録処理')

        all_data = request.params.get("入力データ",[])
        stored_params = request.params
        del stored_params["入力データ"]

        sql_executor = SQLExecutor(session)
        origin_table_name ="TD売上明細V2"
        temp_table_name ="#Tmp売上明細"

        # ==============================#チェックデータの有無==================================

        filtered_data = [item for item in all_data if item['CHECK'] == 1]
        if len(filtered_data) == 0:
            raise ServiceError("チェックデータがありません")

        # ==============================#TMPテーブルの作成処理==================================

        # 元になるテーブル の構造をもとに空の一時テーブルを作成
        create_temp_table_sql = f"""
            SELECT *
            INTO {temp_table_name}
            FROM {origin_table_name}
            WHERE 0<>0
        """

        sql_executor.execute_query(create_temp_table_sql)
        logger.info(f"一時テーブル「{temp_table_name}」を「{origin_table_name}」をもとに作成")

        # ==============================#TMPテーブルへデータ登録処理==================================

        # 各レコードから  不要なをカラムを排除する（一時テーブルにカラムがないため）
        columns = sql_executor.get_columns(origin_table_name)
        insert_data = [{key: val for key, val in record.items() if key in columns} for record in filtered_data]

        sql_executor.execute_insert(temp_table_name, insert_data)
        logger.info(f"一時テーブル「{temp_table_name}」へ入力データの登録")

        # ==============================ストアドプロシージャで登録処理==================================
        stored_name = "usp_HD0703UPD売上"

        stored_results = dict(sql_executor.execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetNo":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))


        # 結果が負の場合、エラー
        if stored_results["output_values"]["@RetST"] == -1:
            raise ServiceError(stored_results["output_values"]["@RetMsg"])

        logger.info("登録されたデータを使ってストアドプロシージャで登録処理")

        # ==============================#TMPテーブルの削除処理==================================
        drop_temp_table_sql = f"DROP TABLE IF EXISTS {temp_table_name};"
        sql_executor.execute_query(drop_temp_table_sql)

        logger.info(f"一時テーブル「{temp_table_name}」の削除(一時テーブルのためセッション内でしか生きていないが、念のため削除)")

        return JSONResponse(
            content= {},
            headers={
                'Download-Skip': "yes"
            }
        )

    def purge_check(self,request,session):
        query = """
            SELECT  USH.請求日付,USH.請求書発行日付
            FROM TD売上請求H AS USH
            WHERE USH.見積番号 = {}
        """.format(request.params["@i見積番号"])

        result = dict(SQLExecutor(session).execute_query(query=query))

        if len(result["results"]) == 0:
            res = False
        else:
            res =True

        return JSONResponse(
            content= {
                "flg":res
            },
            headers={
                'Download-Skip': "yes"
            }
        )
    
    def purge(self,request,session):
        stored_name = "usp_HD0704DEL売上"
        stored_params = request.params
        stored_results = dict(SQLExecutor(session).execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))
        # return stored_results["output_values"]
        return JSONResponse(
            content= {
                "output":stored_results["output_values"]
            },
            headers={
                'Download-Skip': "yes"
            }
        )