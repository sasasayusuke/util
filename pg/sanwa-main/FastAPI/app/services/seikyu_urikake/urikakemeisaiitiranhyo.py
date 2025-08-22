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

class urikakemeisaiitiranhyoService(BaseService):
    """売掛明細一覧表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 売掛明細一覧表の印刷処理
        logger.info("【START】売掛明細一覧表処理")

        storedname="usp_HK0500売掛明細一覧表"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template売掛明細一覧表.xlsx')

            # 初期位置指定
            start_row = 6

            # シートコピー用前回処理得意先CD
            ws_tokuisaki_cd = None

            # シートコピー用前回処理得意先CD
            ws_last_tokuisaki_cd = None

            # Templateシートをコピー
            for result in storedresults['results'][0]:
                if ws_last_tokuisaki_cd != result['得意先CD']:
                    ws = wb.copy_worksheet(wb['Template'])

                    # シート名の変更
                    if str(result['得意先CD']) == '':
                        ws.title = ' '
                    else:
                        ws.title = str(result['得意先CD'])

                    # 前回処理得意先CDの更新
                    ws_last_tokuisaki_cd = result['得意先CD']

                else:
                    # 前回処理得意先CDの更新
                    ws_last_tokuisaki_cd = result['得意先CD']


            # Templateシートの削除
            wb.remove(wb['Template'])

            for ws in wb.worksheets:
                # コピー時に追加した得意先CDを取得
                ws_tokuisaki_cd = ws.title

                # 各シートの処理開始時にrow_numを初期化
                row_num = start_row

                # 得意先情報の取得
                tokuisaki_info = ClsTokuisaki.GetbyID(session=session, tokuisakicd=ws_tokuisaki_cd)

                # シート名の変更
                ws.title = tokuisaki_info[0]['得意先CD'] + tokuisaki_info[0]['得意先名1']

                for row_data in storedresults['results'][0]:
                    if ws_tokuisaki_cd == row_data['得意先CD']:
                        #2行目以降なら行コピー
                        if row_num != start_row:
                            range_copy_cell(ws,ws,1,row_num-1,6,row_num-1,0,1,True)
                        ws.cell(row=row_num, column=1, value=row_data['売上日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        ws.cell(row=row_num, column=2, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['見積件名'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=4)._style)
                        ws.cell(row=row_num, column=5, value=row_data['外税額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        ws.cell(row=row_num, column=6, value="=SUM(D{},E{})".format(row_num, row_num))


                        # データを書き込んだ後、次の行に移動
                        row_num += 1
                    else:
                        continue

                # シートの最終行を取得
                end_row = ws.max_row

                # 作成日の追加
                ws['F1'] = '作成日：' + datetime.today().strftime('%Y{0}%m{1}%d').format(*'//')

                # 得意先情報の追加
                ws['A2'] = '[' + tokuisaki_info[0]['得意先CD'] + ']' + tokuisaki_info[0]['得意先名1']

                # 期間の追加
                start_date = datetime.strptime(request.params['@iS集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                end_date = datetime.strptime(request.params['@iE集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                ws['A3'] = "{}～{}".format(start_date, end_date)

                # 合計欄の関数を修正
                ws.cell(row=5, column=4, value="=SUM(D6:D{})".format(row_num-1))
                ws.cell(row=5, column=5, value="=SUM(E6:E{})".format(row_num-1))
                ws.cell(row=5, column=6, value="=SUM(F6:F{})".format(row_num-1))

                ws.sheet_view.view = 'pageBreakPreview'


            # Excelオブジェクトの保存
            save_excel_to_buffer(buffer, wb,  'sanwa55')

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
