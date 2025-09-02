from typing import Dict, Any
import logging
from app.services.base_service import BaseService
from app.core.config import settings
from fastapi import HTTPException
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class DeleteLockDataService():
    def get(self,request,session):
        logger.info('【start】ロックデータ取得')

        query = "select * from AppLockData where PCName = '{}'".format(request.user)
        results = SQLExecutor(session).execute_query(query=query)

        return results

    def delete(self,request,session):
        logger.info('【start】ロックデータ削除')

        where = "0<>0"
        for record in request.params["numbers"]:
            where = where + " or (DataName = '{}' and Number = {})".format(record["DataName"],record["Number"])

        query = "DELETE FROM AppLockData WHERE {}".format(where)
        SQLExecutor(session).execute_query(query=query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )