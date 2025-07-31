from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class mmseikyumeisaihyoService(BaseService):
    """マミーマート請求明細表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # マミーマート請求明細表の印刷処理
        logger.info("【START】マミーマート請求明細表処理")

        storedname="usp_SE2000MM請求明細表発行"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Temp_MM請求明細.xlsx')

            # 初期位置指定
            start_row = 8

            # シートコピー用前回処理発注担当者名
            ws_last_tokuisaki_cd = None

            # Templateシートをコピー
            for result in storedresults['results'][0]:
                if ws_last_tokuisaki_cd != result['発注担当者名']:
                    ws = wb.copy_worksheet(wb['Template'])

                    # シート名の変更
                    if str(result['発注担当者名']) == '':
                        ws.title = ' '
                    else:
                        ws.title = str(result['発注担当者名'])

                    # 前回処理発注担当者名の更新
                    ws_last_tokuisaki_cd = result['発注担当者名']

                else:
                    # 前回処理発注担当者名の更新
                    ws_last_tokuisaki_cd = result['発注担当者名']

            # Templateシートの削除
            wb.remove(wb['Template'])

            for ws in wb.worksheets:
                # コピー時に追加した発注担当者名を取得
                ws_hattyuutanotusya_cd = ws.title

                # 各シートの処理開始時にrow_numを初期化
                row_num = start_row

                for row_data in storedresults['results'][0]:
                    if ws_hattyuutanotusya_cd == row_data['発注担当者名'] or (ws_hattyuutanotusya_cd == ' ' and row_data['発注担当者名'] == ''):
                        ws.cell(row=row_num, column=2, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['受付日付'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['表示用店舗名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                        ws.cell(row=row_num, column=5, value=row_data['発注担当者名'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        ws.cell(row=row_num, column=6, value=row_data['作業内容'])._style = copy(ws.cell(row=start_row, column=6)._style)
                        ws.cell(row=row_num, column=9, value="1")._style = copy(ws.cell(row=start_row, column=9)._style)
                        ws.cell(row=row_num, column=11, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=11)._style)
                        ws.cell(row=row_num, column=12, value=row_data['完工日付'])._style = copy(ws.cell(row=start_row, column=12)._style)

                        # データを書き込んだ後、次の行に移動
                        row_num += 1
                    else:
                        continue

                # シートの最終行を取得
                end_row = ws.max_row

                # 作成日の追加
                ws['C1'] = request.params['@iE請求日付'][0:4] + '年'
                ws['D1'] = int(request.params['@iE請求日付'][5:7])

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
