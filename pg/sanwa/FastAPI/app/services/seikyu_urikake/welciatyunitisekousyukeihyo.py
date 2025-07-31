from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.service_utils import ClsTokuisaki

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class welciatyunitisekousyukeihyoService(BaseService):
    """ウエルシア中日施工集計表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 売掛明細一覧表の印刷処理
        logger.info("【START】ウエルシア中日施工集計表処理")

        storedname="usp_SE1700Wel中日施工集計"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('TemplateWel中日施工集計表.xlsx')

            ws = wb['Template']

            # 初期位置指定
            start_row = 5

            # 各シートの処理開始時にrow_numを初期化
            row_num = start_row

            # シート名の変更
            ws.title = 'Wel中日施工'

            for row_data in storedresults['results'][0]:
                #2行目以降なら行コピー
                if row_num != start_row:
                    range_copy_cell(ws,ws,1,row_num-1,16,row_num-1,0,1,True)
                ws.cell(row=row_num, column=1, value=row_data['納入得意先CD'])._style = copy(ws.cell(row=start_row, column=1)._style)
                ws.cell(row=row_num, column=2, value=row_data['納入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                ws.cell(row=row_num, column=3, value=row_data['納入先名1'])._style = copy(ws.cell(row=start_row, column=3)._style)
                ws.cell(row=row_num, column=4, value=row_data['物件種別FLG'])._style = copy(ws.cell(row=start_row, column=4)._style)
                ws.cell(row=row_num, column=5, value=row_data['調剤FLG'])._style = copy(ws.cell(row=start_row, column=5)._style)
                ws.cell(row=row_num, column=6, value=row_data['売上金額_中日_施工以外'])._style = copy(ws.cell(row=start_row, column=6)._style)
                ws.cell(row=row_num, column=7, value=row_data['原価金額_中日_施工以外'])._style = copy(ws.cell(row=start_row, column=7)._style)
                ws.cell(row=row_num, column=8, value=row_data['売上金額_中日以外_施工以外'])._style = copy(ws.cell(row=start_row, column=8)._style)
                ws.cell(row=row_num, column=9, value=row_data['原価金額_中日以外_施工以外'])._style = copy(ws.cell(row=start_row, column=9)._style)
                ws.cell(row=row_num, column=10, value=row_data['販売割戻金'])._style = copy(ws.cell(row=start_row, column=10)._style)
                ws.cell(row=row_num, column=11, value=row_data['特別出精値引'])._style = copy(ws.cell(row=start_row, column=11)._style)
                ws.cell(row=row_num, column=12, value=row_data['売上金額_中日_施工'])._style = copy(ws.cell(row=start_row, column=12)._style)
                ws.cell(row=row_num, column=13, value=row_data['原価金額_中日_施工'])._style = copy(ws.cell(row=start_row, column=13)._style)
                ws.cell(row=row_num, column=14, value=row_data['売上金額_中日以外_施工'])._style = copy(ws.cell(row=start_row, column=14)._style)
                ws.cell(row=row_num, column=15, value=row_data['原価金額_中日以外_施工'])._style = copy(ws.cell(row=start_row, column=15)._style)
                ws.cell(row=row_num, column=16, value=row_data['出精値引'])._style = copy(ws.cell(row=start_row, column=16)._style)

                # データを書き込んだ後、次の行に移動
                row_num += 1

            # シートの最終行を取得
            end_row = ws.max_row

            # 作成日の追加
            # ws.cell(row=1, column=16, value=datetime.today().strftime('%Y{0}%m{1}%d{2}').format(*'年月日'))._style = copy(ws.cell(row=start_row, column=16)._style)
            ws['O1'] = datetime.today().strftime('%Y{0}%m{1}%d{2}').format(*'年月日')

            # 期間の追加
            start_date = datetime.strptime(request.params['@iS請求日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
            end_date = datetime.strptime(request.params['@iE請求日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
            ws['A2'] = "{}～{}".format(start_date, end_date)
            ws.sheet_view.view = 'pageBreakPreview'

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
