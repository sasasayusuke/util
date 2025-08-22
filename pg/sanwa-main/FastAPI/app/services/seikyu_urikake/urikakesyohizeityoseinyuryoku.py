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

class urikakesyohizeityoseinyuryokuService(BaseService):
    """売掛消費税調整入力サービス"""
    
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass
    
    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
            """実行処理を行うメソッド"""
            
            logger.info("【START】売掛消費税調整入力登録処理")

            params = request.params

            query =  ""

            # 修正
            if params["modeF"] == 1 and params["adjustmentNo"]:
                 query = "UPDATE TD消費税調整 SET \
                 調整日付 = '{}', 得意先CD = '{}', 調整金額 = {}, 登録変更日 = {} \
                 WHERE 消費税調整番号 = {}".format(
                    params['adjustmentDate'],
                    params['customerCD'],
                    params['adjustmentMoney'],
                    datetime.date.today(),
                    params['adjustmentNo']
                 )

            # 登録
            elif params["modeF"] == 0 and params["adjustmentNo"] == "":
                 
                cls_Snw_cm = Snw_cm
                GetCounter_result = cls_Snw_cm.GetCounter(self,session=session,sItemName="消費税調整番号")
                Number = GetCounter_result['output_values']['@GetNO']

                # ヘッダー部作成
                query = "INSERT INTO TD消費税調整 \
                    (消費税調整番号, 調整日付, 得意先CD, 調整金額, 初期登録日, 登録変更日) \
                    VALUES ({}, '{}', '{}', {}, '{}', '{}')".format(
                        Number,
                        params["adjustmentDate"],
                        params["customerCD"],
                        params["adjustmentMoney"],
                        datetime.date.today(),
                        datetime.date.today()
                    )
                
            logger.debug(f'query:{query}')

            SQLExecutor(session).execute_query(query=query)

            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )
    def delete(self, request, session) -> Dict[str, Any]:
            """実行処理を行うメソッド"""
            
            logger.info("【START】売掛消費税調整入力削除処理")

            params = request.params

            query =  ""

            # 削除用query
            query = "DELETE FROM TD消費税調整 WHERE 消費税調整番号 = {}".format(params['adjustmentNo'])

            SQLExecutor(session).execute_query(query=query)

            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )