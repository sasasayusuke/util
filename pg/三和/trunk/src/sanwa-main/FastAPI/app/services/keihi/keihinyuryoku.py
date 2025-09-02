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

class keihinyuryokuService(BaseService):
    """経費入力サービス"""
    
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass
    
    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
            """実行処理を行うメソッド"""
            
            logger.info("【START】経費入力登録処理")

            params = request.params

            logger.debug('params:{}'.format(params))

            # 新規登録
            if params["modeF"] == 1 and params['DepositNo'] == "":
                  
                cls_Snw_cm = Snw_cm
                GetCounter_result = cls_Snw_cm.GetCounter(self,session=session,sItemName="経費番号")
                Number = GetCounter_result['output_values']['@GetNO']

                query1 = "INSERT INTO TD経費 \
                    (経費番号, 経費日付, 初期登録日, 登録変更日) \
                    VALUES ({}, '{}', '{}', '{}')".format(
                        Number,
                        params["DepositDate"],
                        datetime.date.today(),
                        datetime.date.today()
                    )

                logger.debug(f'query:{query1}')
                SQLExecutor(session).execute_query(query=query1)
                
                # 経費明細
                datacount = 0
                for tableData in params["input_tableData"]:
                    datacount += 1
                    query2 = "INSERT INTO TD経費明細 \
                        (経費番号, 枝番, 科目CD, 科目名, 金額, 担当者CD, 担当者名, 購入先名, 科目摘要CD, 科目摘要名, 消費税額, 補助CD) \
                        VALUES ({}, {}, {}, '{}', {}, {}, {}, {}, {}, {}, {}, {})".format(
                            Number,
                            datacount,
                            tableData["kamokuCD"],
                            tableData['kamokuName'],
                            tableData['kingaku'],
                            tableData['tantosyaCD'] if tableData['tantosyaCD'] != None else "null",
                            "'{}'".format(tableData['tantosyaName']) if tableData['tantosyaName'] != None else "null",
                            "'{}'".format(tableData['kounyusyaName']) if tableData['kounyusyaName'] != None else "null",
                            tableData['tekiyoCD'] if tableData['tekiyoCD'] != None else "null",
                            "'{}'".format(tableData['tekiyoName']) if tableData['tekiyoName'] != None else "null",
                            tableData['syohizei'],
                            tableData['hozyoCD']
                        )
                    
                    logger.debug(f'query:{query2}')

                    SQLExecutor(session).execute_query(query=query2)

            
            # 修正
            elif params['modeF'] == 2 and params['DepositNo']:
                 
                query1 = "DELETE FROM TD経費明細 WHERE 経費番号 = {}".format(params['DepositNo'])
                logger.debug(f'query:{query1}')
                SQLExecutor(session).execute_query(query=query1)
                 
                query2 = "DELETE FROM TD経費 WHERE 経費番号 = {}".format(params['DepositNo'])
                logger.debug(f'query:{query2}')
                SQLExecutor(session).execute_query(query=query2)

                query3 = "INSERT INTO TD経費 \
                    (経費番号, 経費日付, 初期登録日, 登録変更日) \
                    VALUES ({}, '{}', '{}', '{}')".format(
                        params['DepositNo'],
                        params["DepositDate"],
                        datetime.date.today(),
                        datetime.date.today()
                    )

                logger.debug(f'query:{query3}')
                SQLExecutor(session).execute_query(query=query3)

                # 経費明細
                datacount = 0
                for tableData in params["input_tableData"]:
                    datacount += 1
                    query4 = "INSERT INTO TD経費明細 \
                        (経費番号, 枝番, 科目CD, 科目名, 金額, 担当者CD, 担当者名, 購入先名, 科目摘要CD, 科目摘要名, 消費税額, 補助CD) \
                        VALUES ({}, {}, {}, '{}', {}, {}, {}, {}, {}, {}, {}, {})".format(
                            params['DepositNo'],
                            datacount,
                            tableData["kamokuCD"],
                            tableData['kamokuName'],
                            tableData['kingaku'],
                            tableData['tantosyaCD'] if tableData['tantosyaCD'] != None else "null",
                            "'{}'".format(tableData['tantosyaName']) if tableData['tantosyaName'] != None else "null",
                            "'{}'".format(tableData['kounyusyaName']) if tableData['kounyusyaName'] != None else "null",
                            tableData['tekiyoCD'] if tableData['tekiyoCD'] != None else "null",
                            "'{}'".format(tableData['tekiyoName']) if tableData['tekiyoName'] != None else "null",
                            tableData['syohizei'],
                            tableData['hozyoCD']
                        )
                    
                    logger.debug(f'query:{query4}')

                    SQLExecutor(session).execute_query(query=query4)


            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )

    def delete(self, request, session) -> Dict[str, Any]:
            """削除処理を行うメソッド"""
            
            logger.info("【START】経費入力削除処理")

            params = request.params

            query1 = "DELETE FROM TD経費明細 WHERE 経費番号 = {}".format(params['DepositNo'])
            logger.debug(f'query:{query1}')
            SQLExecutor(session).execute_query(query=query1)
                
            query2 = "DELETE FROM TD経費 WHERE 経費番号 = {}".format(params['DepositNo'])
            logger.debug(f'query:{query2}')
            SQLExecutor(session).execute_query(query=query2)

            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )