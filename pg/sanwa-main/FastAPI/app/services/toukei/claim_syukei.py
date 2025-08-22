import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer
from app.utils.string_utils import format_jp_date
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class claimSyukeihyoService(BaseService):
    """クレーム集計表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # クレーム集計表の印刷処理
        logger.info("【START】クレーム集計表")

        storedname="usp_TK3500クレーム集計表出力"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_クレーム集計表.xlsx')
            ws = wb.active

            # 作成日
            ws['T1'] = f"作成日： {format_jp_date()}"

            # 初期位置指定
            start_row   = 3
            copy_row    = 4

            for row_num, row_data in enumerate(storedresults['results'][0], start=start_row):
                ws.cell(row=row_num, column=1, value=row_data['仕入日付'])._style = copy(ws.cell(row=copy_row, column=1)._style)
                ws.cell(row=row_num, column=2, value=row_data['得意先CD'])._style = copy(ws.cell(row=copy_row, column=2)._style)
                ws.cell(row=row_num, column=3, value=row_data['得意先名1'])._style = copy(ws.cell(row=copy_row, column=3)._style)
                ws.cell(row=row_num, column=4, value=row_data['得意先名2'])._style = copy(ws.cell(row=copy_row, column=4)._style)
                ws.cell(row=row_num, column=5, value=row_data['物件種別'])._style = copy(ws.cell(row=copy_row, column=5)._style)
                ws.cell(row=row_num, column=6, value=row_data['見積番号'])._style = copy(ws.cell(row=copy_row, column=6)._style)
                ws.cell(row=row_num, column=7, value=row_data['見積件名'])._style = copy(ws.cell(row=copy_row, column=7)._style)

                ws.cell(row=row_num, column=8, value=row_data['漢字名称'])._style = copy(ws.cell(row=copy_row, column=8)._style)

                ws.cell(row=row_num, column=9, value=row_data['製品NO'])._style = copy(ws.cell(row=copy_row, column=9)._style)
                ws.cell(row=row_num, column=10, value=row_data['仕様NO'])._style = copy(ws.cell(row=copy_row, column=10)._style)

                ws.cell(row=row_num, column=11, value=row_data['W'])._style = copy(ws.cell(row=copy_row, column=11)._style)
                ws.cell(row=row_num, column=12, value=row_data['D'])._style = copy(ws.cell(row=copy_row, column=12)._style)
                ws.cell(row=row_num, column=13, value=row_data['H'])._style = copy(ws.cell(row=copy_row, column=13)._style)
                ws.cell(row=row_num, column=14, value=row_data['仕入単価'])._style = copy(ws.cell(row=copy_row, column=14)._style)
                ws.cell(row=row_num, column=15, value=row_data['仕入数量'])._style = copy(ws.cell(row=copy_row, column=15)._style)
                ws.cell(row=row_num, column=16, value=row_data['仕入金額'])._style = copy(ws.cell(row=copy_row, column=16)._style)

                ws.cell(row=row_num, column=17, value=row_data['仕入先CD'])._style = copy(ws.cell(row=copy_row, column=17)._style)
                ws.cell(row=row_num, column=18, value=row_data['仕入先名1'])._style = copy(ws.cell(row=copy_row, column=18)._style)
                ws.cell(row=row_num, column=19, value=row_data['仕入先名2'])._style = copy(ws.cell(row=copy_row, column=19)._style)

                ws.cell(row=row_num, column=20, value=row_data['明細備考'])._style = copy(ws.cell(row=copy_row, column=20)._style)

                ws.cell(row=row_num, column=21)._style = copy(ws.cell(row=copy_row, column=21)._style)

                ws.row_dimensions[row_num].height = ws.row_dimensions[copy_row].height

            # 改ページプレビューの設定
            ws.sheet_view.view = 'pageBreakPreview'
            ws.sheet_view.zoomScaleSheetLayoutView= 100

            # 印刷範囲の設定
            ws.print_area = 'A1:T' + str(len(storedresults["results"][0])+2)

            # シート名の変更
            ws.title = 'クレーム集計表'

            # Excelオブジェクトの保存
            save_excel_to_buffer(buffer, wb, None)

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
