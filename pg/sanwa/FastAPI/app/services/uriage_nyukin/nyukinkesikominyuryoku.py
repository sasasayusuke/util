import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.core.config import settings
from app.services.base_service import BaseService
from app.utils.string_utils import encode_string, AnsiLeftB, SpcToNull, NullToZero, format_jp_date
from app.utils.service_utils import Snw_cm, ClsTokuisaki
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class nyukinkesikominyuryokuService(BaseService):
    """入金消込入力サービス"""
    
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass
    
    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
            """実行処理を行うメソッド"""
            
            logger.info("【START】入金消込入力登録処理")

            params = request.params

            if params["processCategory"] == '1':
                query = "DELETE FROM TD入金 WHERE 入金番号 = {}".format(params["paymentNo"])
                SQLExecutor(session).execute_query(query=query)
            
            paymentNo = None
            if params["processCategory"] == '0':
                # 新No獲得
                cls_Snw_cm = Snw_cm()
                GetCounter_result = cls_Snw_cm.GetCounter(session=session,sItemName="入金番号")
                paymentNo = GetCounter_result['output_values']['@GetNO']
            else:
                paymentNo = params["paymentNo"]

            def change_paymentType(type):
                if type == "入金":
                    return 1
                elif type == "手形入金":
                    return 2
                elif type == "調整・値引":
                    return 3
                else:
                    return 'null'
            
            # 入金データセット
            cls_custmer = ClsTokuisaki()
            custmer_datas = ClsTokuisaki.GetbyID(session=session, tokuisakicd=params["custmerCD"])[0]
            for i in range(0,len(params["input_tableData"])):
                query = "INSERT INTO TD入金 \
                    (入金番号,枝番,入金日付,得意先CD,得意先名1,得意先名2,入金区分CD,入金区分名,入金種別,入金金額,手形期日,手形番号,摘要名,合計金額,初期登録日,登録変更日) \
                    VALUES ({},{},{},{},{},{},{},{},{},{},{},{},{},{},'{}','{}')".format(
                        paymentNo,
                        i+1,
                        "'{}'".format(params["paymentDate"]),
                        custmer_datas["得意先CD"] if custmer_datas["得意先CD"]!="" else 'null',
                        "'{}'".format(custmer_datas["得意先名1"]) if custmer_datas["得意先名1"]!="" else 'null',
                        "'{}'".format(custmer_datas["得意先名2"]) if custmer_datas["得意先名2"]!="" else 'null',
                        params["input_tableData"][i]["paymentCategory"],
                        "'{}'".format(params["input_tableData"][i]["paymentCategoryName"]),
                        change_paymentType(params["input_tableData"][i]["paymentType"]),
                        params["input_tableData"][i]["paymentMoney"],
                        "'{}'".format(params["input_tableData"][i]["tegataDate"]) if params["input_tableData"][i]["tegataDate"] != None else 'null',
                        "'{}'".format(params["input_tableData"][i]["tegataNo"]) if params["input_tableData"][i]["tegataNo"] != None else 'null',
                        "'{}'".format(params["input_tableData"][i]["remarks"]) if params["input_tableData"][i]["remarks"] != None else 'null',
                        params["sumAll"],
                        datetime.datetime.strptime(params["HAddDate"],'%Y-%m-%d'),
                        datetime.date.today()
                    )
                SQLExecutor(session).execute_query(query=query)

            if params["processCategory"] == '1':
                query = "DELETE FROM TD入金消込 WHERE 入金番号 = {}".format(params["paymentNo"])
                SQLExecutor(session).execute_query(query=query)

            w_kesikomiMoney = params["sumAll"]

            for i,row in enumerate(params["check_tableData"]):
                if SpcToNull(row["estimateNo"],0) != 0 and row["kesikomiMoney"] != 0:
                    query = "INSERT INTO TD入金消込 \
                        (売上番号,入金番号,入金金額,初期登録日,登録変更日) \
                        VALUES({},{},{},'{}','{}')".format(
                            row["estimateNo"],
                            paymentNo,
                            w_kesikomiMoney if i == len(params["check_tableData"])-1 else row["kesikomiMoney"],
                            datetime.date.today(),
                            datetime.date.today()
                        )
                    SQLExecutor(session).execute_query(query=query)
                    w_kesikomiMoney = w_kesikomiMoney - row["kesikomiMoney"]
            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )
    
    def purge(self,request,session) -> Dict[str, Any]:
        
        logger.info('【start】入金消込削除処理')

        query = "DELETE FROM TD入金 WHERE 入金番号 = {}".format(request.params["paymentNo"])
        SQLExecutor(session).execute_query(query=query)

        query = "DELETE FROM TD入金消込 WHERE 入金番号 = {}".format(request.params["paymentNo"])
        SQLExecutor(session).execute_query(query=query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )