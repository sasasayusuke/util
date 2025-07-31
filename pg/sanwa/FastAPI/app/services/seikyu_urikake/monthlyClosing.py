import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.services.base_service import BaseService
from app.utils.excel_utils import (
    get_excel_buffer,
    create_excel_object,
    save_excel_to_buffer,
    ExcelPDFConverter
)
from app.utils.string_utils import encode_string, AnsiLeftB, SpcToNull, NullToZero, format_jp_date
from app.core.config import settings
from copy import copy
from app.utils.service_utils import Snw_cm, ClsKamoku
import zipfile
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor
from app.core.exceptions import ServiceError
from app.utils.service_utils import ClsDates, ClsMitumoriH, ClsTanto,ClsKaisya, ClsZeiritsu,ClsOutputRireki
from dateutil.relativedelta import relativedelta

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class MonthlyClosingService(BaseService):
    """月次締め処理サービス"""
    
    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

    def upload(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        # 経費データの取込処理
        logger.info("【START】月次締め処理")

        date_id = request.params['AggregateValue'] + "月次更新日"
        cDates = ClsDates()
        cDates.GetbyId(session,date_id)

        from_date = datetime.datetime.strptime(request.params['FromDt'], '%Y/%m/%d')
        to_date = datetime.datetime.strptime(request.params['ToDt'], '%Y/%m/%d')

        # 現在の更新日付を取得
        update_date = cDates.更新日付

        # リクエスト日付とDBの更新日付が一致しない場合はエラー
        if (from_date != update_date):
            raise ServiceError('データが更新されています。画面をリロードしてください。')
        
        # 更新日付の翌月の末日を取得
        tmp_date = update_date + relativedelta(months=2)
        tmp_date = tmp_date.replace(day=1)
        next_month_end = tmp_date - datetime.timedelta(days=1)
        
        now = datetime.datetime.now()
        select_query = f"SELECT * FROM TMDates WHERE DateID = '{date_id}'"
        select_result = SQLExecutor(session).execute_query(query=select_query)

        if select_result.count == 0:  # If no record exists, add new
            insert_query = (
                f"INSERT INTO TMDates (DateID, 初期登録日, 更新日付, UserID, 登録変更日) "
                f"VALUES ('{date_id}', '{now.strftime('%Y-%m-%d')}', '{next_month_end.strftime('%Y-%m-%d')}', '{request.params['LoginId']}', '{now.strftime('%Y-%m-%d')}')"
            )
            insert_result = SQLExecutor(session).execute_query(query=insert_query)
            
        else:  # Update existing record
            update_query = (
                f"UPDATE TMDates "
                f"SET 更新日付 = '{next_month_end.strftime('%Y-%m-%d')}', UserID = '{request.params['LoginId']}', 登録変更日 = '{now.strftime('%Y-%m-%d')}' "
                f"WHERE DateID = '{date_id}'"
            )
            update_result = SQLExecutor(session).execute_query(query=update_query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes",
            }
        )
    
    def uploadBack(self, request, session) -> Dict[str, Any]:
        """戻し処理を行うメソッド"""
        # 経費データの取込処理
        logger.info("【START】月次締め戻し処理")

        date_id = request.params['AggregateValue'] + "月次更新日"
        cDates = ClsDates()
        cDates.GetbyId(session,date_id)

        from_date = datetime.datetime.strptime(request.params['FromDt'], '%Y/%m/%d')
        to_date = datetime.datetime.strptime(request.params['ToDt'], '%Y/%m/%d')

        # 現在の更新日付を取得
        update_date = cDates.更新日付

        # リクエスト日付とDBの更新日付が一致しない場合はエラー
        if (from_date != update_date):
            raise ServiceError('データが更新されています。画面をリロードしてください。')
        
        # 更新日付の前月の末日を取得
        tmp_date = update_date.replace(day=1)
        before_month_end = tmp_date - datetime.timedelta(days=1)
        
        now = datetime.datetime.now()
        select_query = f"SELECT * FROM TMDates WHERE DateID = '{date_id}'"
        select_result = SQLExecutor(session).execute_query(query=select_query)

        if select_result.count == 0:  # If no record exists, add new
            insert_query = (
                f"INSERT INTO TMDates (DateID, 初期登録日, 更新日付, UserID, 登録変更日) "
                f"VALUES ('{date_id}', '{now.strftime('%Y-%m-%d')}', '{before_month_end.strftime('%Y-%m-%d')}', '{request.params['LoginId']}', '{now.strftime('%Y-%m-%d')}')"
            )
            insert_result = SQLExecutor(session).execute_query(query=insert_query)
            
        else:  # Update existing record
            update_query = (
                f"UPDATE TMDates "
                f"SET 更新日付 = '{before_month_end.strftime('%Y-%m-%d')}', UserID = '{request.params['LoginId']}', 登録変更日 = '{now.strftime('%Y-%m-%d')}' "
                f"WHERE DateID = '{date_id}'"
            )
            update_result = SQLExecutor(session).execute_query(query=update_query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes",
            }
        )