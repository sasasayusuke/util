from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
import datetime
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import (
    get_excel_buffer,
    create_excel_object,
    save_excel_to_buffer,
    range_copy_cell,
    range_copy_cell_by_address,
    insert_image,
    ExcelPDFConverter,
)
from app.utils.string_utils import encode_string, format_jp_date
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class TestService(BaseService):
    """testサービス"""

    def display(self, request, session) -> StreamingResponse:
        """
        xlsx出力テスト(実行パラメータ)
        {
            "category": "test",
            "title": "test",
            "button": "xlsx",
            "user": "string",
            "opentime": "2024-11-07T00:21:48.212Z",
            "params": {"@iS入金予定日付":"2004/02/09", "@iE入金予定日付":"2004/07/11", "@iS得意先CD":"1001", "@iE得意先CD":"1003"}
        }
        """

        # 入金予定表の印刷処理
        logger.info("【START】xlsx Test")

        storedname="usp_ND0900入金予定表出力"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params,  outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

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
            start_date = format_jp_date(request.params['@iS入金予定日付'])
            end_date = format_jp_date(request.params['@iE入金予定日付'])
            ws['B2'] = "{} ～ {}".format(start_date, end_date)

            # 作成日の追加
            ws['G2'] = f"作成日： {format_jp_date()}"

            # 改ページプレビューの設定
            ws.sheet_view.view = 'pageBreakPreview'
            ws.sheet_view.zoomScaleSheetLayoutView= 100

            # シート名の変更
            ws.title = 'コピー元'

            insert_image(ws, "sanwa.ico", 35, 35, 'F2')
            # 新規シートの作成
            new_ws1 = wb.create_sheet(title='range_copy_cell使用')
            range_copy_cell(ws, new_ws1, 2, 2, 5, 10, 7, 7, True)

            new_ws2 = wb.create_sheet(title='range_copy_cell_by_address使用')
            range_copy_cell_by_address(ws, new_ws2, "B2", "F10", "E3", True)

            # Excelオブジェクトの保存
            save_excel_to_buffer(buffer, wb, None)

            # Excelファイルの返却
            return StreamingResponse(
                io.BytesIO(buffer.getvalue()),
                media_type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                headers={
                    'Content-Disposition': 'attachment; filename="sample.xlsx"'
                }
            )


    def displayPDF(self, request, session) -> Dict[str, Any]:
        """
        PDFテスト(実行パラメータ)
        {
            "category": "test",
            "title": "test",
            "button": "pdf",
            "user": "string",
            "opentime": "2024-11-07T00:21:48.212Z",
            "params": {"@iS入金予定日付":"2004/02/09", "@iE入金予定日付":"2004/07/11", "@iS得意先CD":"1001", "@iE得意先CD":"1003"}
        }
        """

        # 入金予定表の印刷処理
        logger.info("【START】pdf Test")

        storedname="usp_ND0900入金予定表出力"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params,  outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

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
            start_date = format_jp_date(request.params['@iS入金予定日付'])
            end_date = format_jp_date(request.params['@iE入金予定日付'])
            ws['B2'] = "{} ～ {}".format(start_date, end_date)

            # 作成日の追加
            ws['G2'] = f"作成日： {format_jp_date()}"

            # PDFファイルの返却
            converter = ExcelPDFConverter(
                wb=wb,
                ws=ws,
                start_cell="A1",
                end_cell="G17"
            )
            pdf_content = converter.generate_pdf()


            return StreamingResponse(
                io.BytesIO(pdf_content),
                media_type="application/pdf",
                headers={
                    "Content-Disposition": f'attachment; filename="sample.pdf"'
                }
            )

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

