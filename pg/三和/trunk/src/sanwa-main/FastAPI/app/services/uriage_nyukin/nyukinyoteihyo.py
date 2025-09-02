from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
import datetime
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer
from app.utils.string_utils import format_jp_date
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class NyukinyoteihyoService(BaseService):
    """入金予定表サービス"""

    def display(self, request, session) -> StreamingResponse:
        """印刷処理を行うメソッド"""

        # 入金予定表の印刷処理
        logger.info("【START】入金予定表処理")

        storedname="usp_ND0900入金予定表出力"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            # Excelオブジェクトの作成
            wb = create_excel_object('Template_入金予定表.xlsx')
            ws = wb.active

            # 初期位置指定
            start_row = 4

            for row_num, row_data in enumerate(storedresults['results'][0], start=start_row):
                ws.cell(row=row_num, column=1, value=int(row_data['得意先CD']))._style = copy(ws.cell(row=start_row, column=1)._style)
                ws.cell(row=row_num, column=2, value=row_data['得意先名1'])._style = copy(ws.cell(row=start_row, column=2)._style)
                ws.cell(row=row_num, column=3, value=row_data['得意先名2'])._style = copy(ws.cell(row=start_row, column=3)._style)
                ws.cell(row=row_num, column=4, value=row_data['入金予定日付'])._style = copy(ws.cell(row=start_row, column=4)._style)
                ws.cell(row=row_num, column=5, value=row_data['入金予定金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                ws.cell(row=row_num, column=6, value=row_data['請求日付'])._style = copy(ws.cell(row=start_row, column=6)._style)
                ws.cell(row=row_num, column=7)._style = copy(ws.cell(row=start_row, column=7)._style)

                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

            # 期間の追加
            start_date = datetime.datetime.strptime(request.params['@iS入金予定日付'], '%Y/%m/%d').strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
            end_date = datetime.datetime.strptime(request.params['@iE入金予定日付'], '%Y/%m/%d').strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
            ws['B2'] = "{} ～ {}".format(start_date, end_date)

            # 作成日の追加
            ws['G2'] = f"作成日： {format_jp_date()}"

            # 改ページプレビューの設定
            ws.sheet_view.view = 'pageBreakPreview'
            ws.sheet_view.zoomScaleSheetLayoutView= 100

            # シート名の変更
            ws.title = '入金予定表'

            # Excelオブジェクトの保存
            save_excel_to_buffer(buffer, wb, 'sanwa55')

            return StreamingResponse(
                io.BytesIO(buffer.getvalue()),
                media_type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                headers={
                    'Content-Disposition': 'attachment; filename="sample.xlsx"'
                }
            )

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass
