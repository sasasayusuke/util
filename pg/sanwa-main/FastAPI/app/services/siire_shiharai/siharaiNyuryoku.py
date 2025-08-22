import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.core.config import settings
from app.services.base_service import BaseService
from app.utils.string_utils import encode_string, AnsiLeftB, SpcToNull, NullToZero, format_jp_date
from app.utils.service_utils import Snw_cm, ClsTokuisaki,ClsShiiresaki
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class siharaiNyuryokuService(BaseService):
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass
    
    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        
        logger.info("【START】支払入力")

        params = request.params

        
        if params["mode"] == 2:
            query = "DELETE FROM TD支払 WHERE 支払番号 = {}".format(params["paymentNo"])
            SQLExecutor(session).execute_query(query=query)
        
        if params["mode"] == 1:
            cls_Snw_cm = Snw_cm()
            GetCounter_result = cls_Snw_cm.GetCounter(session=session,sItemName="支払番号")
            paymentNo = GetCounter_result['output_values']['@GetNO']
            HADDDate = datetime.datetime.now()
            HADDDate = datetime.datetime.strftime(HADDDate,'%Y-%m-%d')
        else:
            paymentNo = params["paymentNo"]
            HADDDate = params["HADDDate"]

        siiresaki_result = ClsShiiresaki.GetbyID(session=session, shiiresakicd=params["SupplierCD"])

        for i in range(0,len(params["input_tableData"])):
            query = "INSERT INTO TD支払 \
                (支払番号,枝番,支払日付,仕入先CD,仕入先名1,仕入先名2,支払区分CD,支払区分名,支払種別,支払金額,手形期日,手形番号,摘要名,合計金額,初期登録日,登録変更日) \
                VALUES({},{},{},{},{},{},{},{},{},{},{},{},{},{},'{}','{}')".format(
                    paymentNo,
                    i+1,
                    "'{}'".format(params["paymentDate"]),
                    siiresaki_result[0]["仕入先CD"],
                    "'{}'".format(NullToZero(siiresaki_result[0]["仕入先名1"],"")),
                    "'{}'".format(siiresaki_result[0]["仕入先名2"]) if NullToZero(siiresaki_result[0]["仕入先名2"],None) != None else "null",
                    params["input_tableData"][i]["paymentCategory"],
                    "'{}'".format(params["input_tableData"][i]["paymentCategoryName"]),
                    params["input_tableData"][i]["paymentType"],
                    params["input_tableData"][i]["paymentMoney"],
                    "'{}'".format(params["input_tableData"][i]["tegataDate"]) if params["input_tableData"][i]["tegataDate"] != None else "null",
                    "'{}'".format(params["input_tableData"][i]["tegataNo"]) if params["input_tableData"][i]["tegataNo"] != None else "null",
                    "'{}'".format(params["input_tableData"][i]["remarks"]) if params["input_tableData"][i]["remarks"] != None else "null",
                    params["sumAll"],
                    datetime.datetime.strptime(HADDDate,'%Y-%m-%d'),
                    datetime.date.today()
                )
            SQLExecutor(session).execute_query(query=query)
            

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )
    
    def purge(self,request,session) -> Dict[str, Any]:
        logger.info("【START】支払入力削除")

        query = "DELETE FROM TD支払 WHERE 支払番号 = {}".format(request.params["paymentNo"])
        SQLExecutor(session).execute_query(query=query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )
