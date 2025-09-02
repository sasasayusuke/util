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
from app.utils.service_utils import ClsTokuisaki

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class joytechSeikyumeisaiitiranhyoService(BaseService):
    """ジョイテック請求明細一覧表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 支払予定表の印刷処理
        logger.info("【START】ジョイテック請求明細一覧表処理")

        storedname="usp_SE2400B請求明細一覧表"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('TempB請求明細一覧表.xlsx')
            ws = wb.active

            # 初期位置指定
            start_row = 6

            # シートコピー用前回処理得意先CD
            ws_last_Bkubun = None

            # Templateシートをコピー
            for result in storedresults['results'][0]:
                if ws_last_Bkubun != result['B請求管轄区分']:
                    ws = wb.copy_worksheet(wb['Template'])

                    # B請求管轄区分の追加
                    ws['A2'] = result['B請求管轄区分']

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                    # シート名の変更
                    ws.title = str(result['B請求管轄区分'])

                    # 前回処理B請求管轄区分の更新
                    ws_last_Bkubun = result['B請求管轄区分']

                else:
                    # 前回処理B請求管轄区分の更新
                    ws_last_Bkubun = result['B請求管轄区分']

            # Templateシートの削除
            wb.remove(wb['Template'])

            for ws in wb.worksheets:
                # コピー時に追加したB請求管轄区分を取得
                ws_Bkubun = ws['A2'].value

                # 各シートの処理開始時にrow_numを初期化
                row_num = start_row

                for row_data in storedresults['results'][0]:
                    if ws_Bkubun == row_data['B請求管轄区分']:
                        ws.cell(row=row_num, column=1, value=row_data['請求日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        ws.cell(row=row_num, column=2, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['見積件名'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=4)._style)
                        ws.cell(row=row_num, column=5, value=row_data['外税額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        ws.cell(row=row_num, column=6, value="")._style = copy(ws.cell(row=start_row, column=6)._style)
                        ws.cell(row=row_num, column=7, value=row_data['合計金額'] + row_data['外税額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                        if row_data['鏡番号'] == 0:
                            ws.cell(row=row_num, column=8, value="")._style = copy(ws.cell(row=start_row, column=8)._style)
                        else:
                            ws.cell(row=row_num, column=8, value=row_data['鏡番号'])._style = copy(ws.cell(row=start_row, column=8)._style)
                        ws.cell(row=row_num, column=9, value=row_data['備考'])._style = copy(ws.cell(row=start_row, column=9)._style)

                        # 高さ調整
                        ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                        # データを書き込んだ後、次の行に移動
                        row_num += 1
                    else:
                        continue

                # シートの最終行を取得
                end_row = ws.max_row

                # 期間の追加
                start_date = datetime.strptime(request.params['@iS集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                end_date = datetime.strptime(request.params['@iE集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                ws['A3'] = "{}～{}".format(start_date, end_date)

                # 作成日の追加
                ws['H1'] = datetime.today().strftime('%Y{0}%m{1}%d{2}').format(*'年月日')

                # 合計式の追加
                # 税抜金額合計
                ws['D5'] = "=SUM(D{}:D{})".format(start_row, end_row)
                # 消費税合計
                ws['E5'] = "=SUM(E{}:E{})".format(start_row, end_row)
                # 非課税合計
                ws['F5'] = "=SUM(F{}:F{})".format(start_row, end_row)
                # 税込金額合計
                ws['G5'] = "=SUM(G{}:G{})".format(start_row, end_row)

                # B請求管轄区分の削除
                ws['A2'] = ""

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
