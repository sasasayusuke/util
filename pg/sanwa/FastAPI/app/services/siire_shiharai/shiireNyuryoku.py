import datetime
from typing import Dict, Any
import io
import logging
from copy import copy
from fastapi.responses import JSONResponse
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.service_utils import ClsDates
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.string_utils import null_to_zero


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class ShiireNyuryokuService(BaseService):
    """仕入入力サービス"""

    def render(self, request, session):
        # 支払予定表の印刷処理
        logger.info("【START】仕入明細入力レンダリング処理")

        sql_executor = SQLExecutor(session)
        stored_name = "usp_HS0101仕入明細抽出"
        params = request.params
        stored_params = {k: v for k, v in params.items() if k.startswith('@')}
        stored_results = dict(sql_executor.execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        # 仕入日付と支払日付
        if params.get('@i処理区分') == '0':
            cDates = ClsDates()
            if cDates.GetbyId(session,"買掛月次更新日") == False:
                sales_date = datetime.datetime.strftime(datetime.date.today(),'%Y-%m-%d')
                payment_date = sales_date
            else:
                query = "select * from TM仕入日付"
                result = dict(SQLExecutor(session).execute_query(query=query))
                sales_date = result["results"][0]["仕入日付"]
                sales_date = datetime.datetime.strftime(sales_date,'%Y-%m-%d')
                payment_date = sales_date
        else:
            sales_date = params["仕入日付"]
            payment_date = params["支払日付"]

        headers = [
            {
                "label": "処理区分",
                "accessor": "process_cd",
                "value": params.get('@i処理区分', ''),
                "value2": params.get('処理区分名', ''),
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
                "label": "仕入先",
                "accessor": "supplier_cd",
                "value": params.get('@i仕入先CD', ''),
                "value2": params.get('仕入先名', ''),
                "position": "left",
            },
            {
                "label": "配送先",
                "accessor": "delivery_cd",
                "value": params.get('@i配送先CD', ''),
                "value2": params.get('配送先名', ''),
                "position": "left",
            },
            {
                "label": "仕入日付",
                "accessor": "sale_date",
                "value": sales_date,
                "position": "left-down",
            },
            {
                "label": "支払日付",
                "accessor": "pay_date",
                "value": payment_date,
                "position": "left-down",
            },
            {
                "label": "外税対象額",
                "accessor": "tax_exclusive_amount",
                "value": params.get('外税対象額', '0'),
                "position": "right",
            },
            {
                "label": "外税",
                "accessor": "tax_exclusive",
                "value": params.get('外税額', '0'),
                "position": "right",
            },
            {
                "label": "内税対象額",
                "accessor": "tax_inclusive_amount",
                "position": "right",
            },
            {
                "label": "非課税金額",
                "accessor": "non_taxable_amount",
                "position": "right",
            },
            {
                "label": "合計金額",
                "accessor": "sum_amount",
                "position": "right",
            },
            {
                "label": "売上合計",
                "accessor": "sum_sales_amount",
                "position": "right",
            },
        ]
        columns = [
            {
                "label": "No",
                "accessor": "仕入明細行番号",
                "type": "text",
                "align": 'center',
                "width": "40px",
                "views": ["仕入情報", "製品情報", "エラー情報"], # １要素目は表示切替にだす項目を明示的に全部書く
            },
            {
                "label": "CK",
                "accessor": "CHECK",
                "type": "checkbox",
                "width": "40px",
                "editable": True,
                're-render':True,
                "description": "チェック：仕入",
            },
            {
                "label": "見積行",
                "accessor": "見積行番号",
                "type": "text",
                "align": 'right',
                "width": "50px",
            },
            {
                "label": "伝区",
                "accessor": "伝票区分",
                "type": "text",
                "align": 'center',
                "width": "50px",
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
            {
                "label": "SP区",
                "accessor": "SP区分",
                "type": "text",
                "align": 'center',
                "width": "50px",
            },
            {
                "label": "PC区",
                "accessor": "PC区分",
                "type": "text",
                "align": 'center',
                "width": "50px",
            },
            {
                "label": "製品No",
                "accessor": "製品NO",
                "type": "text",
                "width": "70px",
                "description": "製品Noを入力して下さい。",
            },
            {
                "label": "仕様No",
                "accessor": "仕様NO",
                "type": "text",
                "width": "70px",
                "description": "仕様Noを入力して下さい。",
            },
            {
                "label": "ベース色",
                "accessor": "ベース色",
                "type": "text",
                "width": "90px",
                "views": ["製品情報"],
            },
            {
                "label": "名称",
                "accessor": "漢字名称",
                "type": "text",
                "width": "300px",
                "description": "名称を入力して下さい。",
            },
            {
                "label": "W",
                "accessor": "W",
                "type": "number",
                "width": "60px",
                "description": "Ｗを入力して下さい。",
            },
            {
                "label": "D",
                "accessor": "D",
                "type": "number",
                "width": "60px",
                "description": "Dを入力して下さい。",
            },
            {
                "label": "H",
                "accessor": "H",
                "type": "number",
                "width": "60px",
                "description": "Hを入力して下さい。",
            },
            {
                "label": "D1",
                "accessor": "D1",
                "type": "number",
                "width": "60px",
                "views": ["製品情報"],
                "description": "D1を入力して下さい。",
            },
            {
                "label": "D2",
                "accessor": "D2",
                "type": "number",
                "width": "60px",
                "views": ["製品情報"],
                "description": "D2を入力して下さい。",
            },
            {
                "label": "H1",
                "accessor": "H1",
                "type": "number",
                "width": "60px",
                "views": ["製品情報"],
                "description": "H1を入力して下さい。",
            },
            {
                "label": "H2",
                "accessor": "H2",
                "type": "number",
                "width": "60px",
                "views": ["製品情報"],
                "description": "H2を入力して下さい。",
            },
            {
                "label": "エラー内容",
                "accessor": "エラー内容",
                "type": "text",
                "width": "400px",
                "views": ["エラー情報"],
            },
            {
                "label": "数量",
                "accessor": "仕入数量",
                "type": "number",
                "width": "80px",
            },
            {
                "label": "単位",
                "accessor": "単位名",
                "type": "text",
                "views": ["製品情報"],
                "width": "50px",
            },
            {
                "label": "U",
                "accessor": "U区分",
                "type": "text",
                "values": ["U", "B", "R"],
                "align": 'center',
                "width": "50px",
                # "editable": True,
                "length":1,
                "description": "原価未確定:U 売価未確定:B",
            },
            {
                "label": "仕入単価",
                "accessor": "仕入単価",
                "type": "number",
                "width": "120px",
                "editable": True,
                're-render':True,
                "max":100000000,
                "min":-100000000,
                "description": "仕入単価を入力して下さい。",
            },
            {
                "label": "仕入金額",
                "accessor": "仕入金額",
                "type": "number",
                "width": "120px",
                're-render':True,
            },
            {
                "label": "税区",
                "accessor": "仕入税区分",
                "type": "text",
                "align": 'center',
                "width": "50px",
            },
            {
                "label": "税区分名",
                "accessor": "仕入税区分名",
                "type": "text",
                "width": "70px",
            },
            {
                "label": "消費税額",
                "accessor": "消費税額",
                "type": "number",
                "width": "120px",
            },
            {
                "label": "売上単価",
                "accessor": "売上単価",
                "type": "number",
                "width": "120px",
            },
            {
                "label": "作業区分",
                "accessor": "作業区分CD",
                "type": "text",
                "align": 'center',
                "width": "50px",
            },
        ]

        # データの準備
        data = stored_results.get("results", [[]])[0]
        gUcnt = 0
        # 除外したいキー
        excludes = ['初期登録日', '登録変更日']
        for item in data:
            for key in excludes:
                if key in item:
                    del item[key]

            if item["仕入税区分"] == 0:
                tax_category_name = '外税'
            elif item["仕入税区分"] == 1:
                tax_category_name = '内税'
            elif item["仕入税区分"] == 2:
                tax_category_name = '非課税'

            item["仕入税区分名"] = tax_category_name

            # gucnt
            if item["U区分"] == 'U' and item["仕入数量"] != "0":
                gUcnt += 1

            # 他社納品日付の日付変換
            if item["他社納品日付"] != None:
                item["他社納品日付"] = datetime.datetime.strftime(item["他社納品日付"],'%Y-%m-%d')


        return JSONResponse(
            content= {
                "headers": headers,
                "columns": columns,
                "data": data,
                "gUcnt":gUcnt
            },
            headers={
                'Download-Skip': "yes"
            }
        )


    def check(self, request, session):

        logger.info("【START】仕入明細入力登録前チェック処理")

        sql_executor = SQLExecutor(session)
        origin_table_name ="Tmp仕入明細内訳"
        temp_table_name ="#Tmp仕入明細内訳"


        all_data = request.params.get("入力データ", [])
        stored_params = request.params
        del stored_params["入力データ"]

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
        insert_data = self.create_checkArray(columns,filtered_data)
        # insert_data = [{key: val for key, val in record.items() if key in columns} for record in filtered_data]

        sql_executor.execute_insert(temp_table_name, insert_data)
        logger.info(f"一時テーブル「{temp_table_name}」へ入力データの登録")

        tmp = sql_executor.execute_query(query="select * from #Tmp仕入明細内訳")

        # ==============================ストアドプロシージャでチェック処理==================================
        stored_name = "usp_HS0102仕入内訳UploadCheck"

        stored_results = dict(sql_executor.execute_stored_procedure(stored_name, stored_params))

        # 結果が負の場合、エラー
        if stored_results["output_values"]["@RetST"] == -1:
            raise ServiceError(stored_results["output_values"]["@RetMsg"])

        logger.info("登録されたデータを使ってストアドプロシージャでチェック処理")

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
    
    def create_checkArray(self,columns,data):
        res = []
        obj = {}
        for i,record in enumerate(data,1):
            obj = {}
            obj['仕入番号'] = record["仕入番号"]
            obj['仕入先CD'] = record["仕入先CD"]
            obj['配送先CD'] = record["配送先CD"]
            obj['仕入明細行番号'] = i
            obj['見積行番号'] = null_to_zero(record["見積行番号"],0)
            obj['見積明細連番'] = null_to_zero(record["見積明細連番"],0)
            obj['伝票区分'] = null_to_zero(record["伝票区分"],"")
            obj['SP区分']  = null_to_zero(record["SP区分"],"")
            obj['PC区分']  = null_to_zero(record["PC区分"],"")
            obj['製品NO']  = null_to_zero(record["製品NO"],"")
            obj['仕様NO']  = null_to_zero(record["仕様NO"],"")
            obj['ベース色'] =null_to_zero( record["ベース色"],"")
            obj['漢字名称'] =null_to_zero( record["漢字名称"],"")
            obj['W'] = record["W"]
            obj['D'] = record["D"]
            obj['H'] = record["H"]
            obj['D1'] = record["D1"]
            obj['D2'] = record["D2"]
            obj['H1'] = record["H1"]
            obj['H2'] = record["H2"]
            obj['エラー内容'] = null_to_zero(record["エラー内容"],"")
            obj['仕入数量'] = null_to_zero(record["仕入数量"],0)
            obj['発注数'] = null_to_zero(record["発注数"],0)
            obj['見積引当数'] = null_to_zero(record["見積引当数"],0)
            obj['単位名'] = null_to_zero(record["単位名"],"")
            obj['仕入単価'] = null_to_zero(record["仕入単価"],0)
            obj['仕入金額'] = null_to_zero(record["仕入金額"],0)
            obj['定価'] = null_to_zero(record["定価"],0)
            obj['仕入税区分'] = null_to_zero(record["仕入税区分"],0)
            obj['消費税額'] = null_to_zero(record["消費税額"],0)
            obj['製品区分'] = record["製品区分"]
            # obj['初期登録日'] = null_to_zero(record["初期登録日"],)
            # obj['登録変更日'] = null_to_zero(record["登録変更日"],)
            obj['CHECK'] = null_to_zero(record["CHECK"],0)
            obj['U区分'] = null_to_zero(record["U区分"],"")
            obj['作業区分CD'] = null_to_zero(record["作業区分CD"],0)

            res.append(obj)
        return res

    def upload(self, request, session):

        logger.info("【START】仕入明細入力登録処理")

        sql_executor = SQLExecutor(session)
        origin_table_name = "Tmp仕入明細内訳"
        temp_table_name   = "#Tmp仕入明細内訳"
        # temp_table_name = "##Tmp売上明細"


        all_data = request.params.get("入力データ", [])
        stored_params = request.params
        del stored_params["入力データ"]

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
        stored_name = "usp_HS0103UPD仕入"

        stored_results = dict(sql_executor.execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetNo":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))


        # 結果が負の場合、エラー
        if stored_results["output_values"]["@RetST"] == -1:
            raise ServiceError(stored_results["output_values"]["@RetMsg"])

        logger.info("登録されたデータを使ってストアドプロシージャで登録処理")

        # ==============================#TMPテーブルの削除処理==================================
        drop_temp_table_sql = f"DROP TABLE IF EXISTS {temp_table_name};"
        sql_executor.execute_query(drop_temp_table_sql)

        logger.info(f"一時テーブル「{temp_table_name}」の削除(一時テーブルのためセッション内でしか生きていないが、念のため削除)")

        # 仕入日付更新
        query = "update TM仕入日付 SET 仕入日付='{}', 登録変更日=SYSDATETIME() where id = 1".format(request.params["@i仕入日付"])
        sql_executor.execute_query(query)

        return JSONResponse(
            content= {},
            headers={
                'Download-Skip': "yes"
            }
        )

    def delete(self, request, session):

        logger.info("【START】仕入明細入力全削除処理")

        sql_executor = SQLExecutor(session)

        # ==============================ストアドプロシージャで削除処理==================================
        stored_name = "usp_HS0104DEL仕入"
        stored_params = request.params

        stored_results = dict(sql_executor.execute_stored_procedure(storedname=stored_name, params=stored_params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))


        # 結果が負の場合、エラー
        if stored_results["output_values"]["@RetST"] == -1:
            raise ServiceError(stored_results["output_values"]["@RetMsg"])

        logger.info("表示されている明細情報をストアドプロシージャですべて削除処理")


        return JSONResponse(
            content= {},
            headers={
                'Download-Skip': "yes"
            }
        )

    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass
    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass


