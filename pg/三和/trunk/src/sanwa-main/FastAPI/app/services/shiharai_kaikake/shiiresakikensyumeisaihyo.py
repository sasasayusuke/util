from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell
from app.utils.service_utils import ClsShiiresaki
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class shiiresakikensyumeisaihyoService(BaseService):
    """仕入先検収明細表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 仕入先検収明細表の印刷処理
        logger.info("【START】仕入先検収明細表処理")

        storedname="usp_SH0300チーム別仕入先検収明細表"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template検収明細表.xlsx')

            # 初期位置指定
            start_row = 6

            # シートコピー用前回処理仕入先CD
            ws_siiresaki_cd = None

            # Templateシートをコピー
            for result in storedresults['results'][0]:
                if ws_siiresaki_cd != result['仕入先CD']:
                    ws = wb.copy_worksheet(wb['Template'])

                    # シート名の変更
                    if str(result['仕入先CD']) == '':
                        ws.title = ' '
                    else:
                        ws.title = str(result['仕入先CD'])

                # 前回処理仕入先CDの更新
                ws_siiresaki_cd = result['仕入先CD']


            # Templateシートの削除
            wb.remove(wb['Template'])

            for ws in wb.worksheets:
                # コピー時に追加した仕入先CDを取得
                ws_siiresaki_cd = ws.title

                # 各シートの処理開始時にrow_numを初期化
                row_num = start_row

                # 仕入先情報の取得
                siiresaki_info = ClsShiiresaki.GetbyID(session=session, shiiresakicd=ws_siiresaki_cd)

                # シート名の変更
                ws.title = siiresaki_info[0]['仕入先CD'] + siiresaki_info[0]['仕入先名1']

                for row_data in storedresults['results'][0]:
                    if ws_siiresaki_cd == row_data['仕入先CD']:
                        #2行目以降なら行コピー
                        if row_num != start_row:
                            range_copy_cell(ws,ws,1,row_num-1,17,row_num-1,0,1,True)
                        ws.cell(row=row_num, column=1, value=row_data['仕入日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        ws.cell(row=row_num, column=2, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['見積件名'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['得意先名1'])._style = copy(ws.cell(row=start_row, column=4)._style)
                        ws.cell(row=row_num, column=5, value=row_data['製品NO'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        ws.cell(row=row_num, column=6, value=row_data['仕様NO'])._style = copy(ws.cell(row=start_row, column=6)._style)
                        ws.cell(row=row_num, column=7, value=row_data['漢字名称'])._style = copy(ws.cell(row=start_row, column=7)._style)
                        ws.cell(row=row_num, column=8, value=row_data['W'])._style = copy(ws.cell(row=start_row, column=8)._style)
                        ws.cell(row=row_num, column=9, value=row_data['D'])._style = copy(ws.cell(row=start_row, column=9)._style)
                        ws.cell(row=row_num, column=10, value=row_data['H'])._style = copy(ws.cell(row=start_row, column=10)._style)
                        ws.cell(row=row_num, column=11, value=row_data['D1'])._style = copy(ws.cell(row=start_row, column=11)._style)
                        ws.cell(row=row_num, column=12, value=row_data['D2'])._style = copy(ws.cell(row=start_row, column=12)._style)
                        ws.cell(row=row_num, column=13, value=row_data['H1'])._style = copy(ws.cell(row=start_row, column=13)._style)
                        ws.cell(row=row_num, column=14, value=row_data['H2'])._style = copy(ws.cell(row=start_row, column=14)._style)
                        ws.cell(row=row_num, column=15, value=row_data['仕入数量'])._style = copy(ws.cell(row=start_row, column=15)._style)
                        ws.cell(row=row_num, column=16, value=row_data['仕入単価'])._style = copy(ws.cell(row=start_row, column=16)._style)
                        ws.cell(row=row_num, column=17, value=row_data['仕入金額'])._style = copy(ws.cell(row=start_row, column=17)._style)


                        # データを書き込んだ後、次の行に移動
                        row_num += 1
                    else:
                        continue

                # シートの最終行を取得
                end_row = ws.max_row

                # 作成日の追加
                ws['P1'] = '作成日：' + datetime.today().strftime('%Y{0}%m{1}%d').format(*'//')

                # 仕入先情報の追加
                ws['A3'] = '[' + siiresaki_info[0]['仕入先CD'] + ']' + siiresaki_info[0]['仕入先名1']

                # 期間の追加
                start_date = datetime.strptime(request.params['@iS集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                end_date = datetime.strptime(request.params['@iE集計日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
                ws['D3'] = "{}～{}".format(start_date, end_date)

                #集計関数設定
                ws.cell(row=5, column=15, value="=SUM(O6:O{})".format(row_num-1))
                ws.cell(row=5, column=17, value="=SUM(Q6:Q{})".format(row_num-1))

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
