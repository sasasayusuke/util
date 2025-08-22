import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, delete_row_with_merged_ranges
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.excel_utils import range_copy_cell
from copy import copy

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class welciaSeikyumeisaiService(BaseService):
    """ウエルシア請求明細サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 支払予定表の印刷処理
        logger.info("【START】ウエルシア請求明細処理")

        storedname="usp_SE1400Wel請求明細"

        outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams=outputparams))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        if request.params['@i書式区分'] == "0":
            wb = create_excel_object('TemplateWel請求明細.xlsx')
            temp_ws = wb['Template']
            temp_total_ws = wb['Template_Total']
        else:
            wb = create_excel_object('TemplateWel請求明細_経理用.xlsx')
            temp_ws = wb['Template']

        end_month = request.params['@iE集計日付'][5:7].lstrip('0')


        # TemplateWel請求明細は リースごとにシート　物件区分ごとに表
        with get_excel_buffer() as buffer:
            ws = wb.active

            # 初期位置指定
            start_row = 3

            # シートコピー用前回処理納入先CD
            cp_last_lease_cd = None
            cp_last_bukken_name = None

            # 書式区分が0の場合はウエルシアリース区分ごとにシートを分ける
            # ウエルシア請求明細
            if request.params['@i書式区分'] == "0":

                # Templateシートをコピー
                for result in storedresults['results'][0]:
                    if cp_last_lease_cd != result['ウエルシアリース区分']:
                        ws = wb.copy_worksheet(temp_ws)

                        if result['ウエルシアリース区分'] == 1:
                            ws.title = "通常"
                            ws['B3'] = 1
                            delete_row_with_merged_ranges(ws, 5)
                        elif result['ウエルシアリース区分'] == 2:
                            ws.title = "リース"
                            ws['B3'] = 2
                            delete_row_with_merged_ranges(ws, 5)
                        else:
                            ws.title = "なし"
                            ws['B3'] = 0
                            delete_row_with_merged_ranges(ws, 5)

                        # 前回処理ウエルシアリース区分の更新
                        cp_last_lease_cd = result['ウエルシアリース区分']

                    else:
                        # 前回処理ウエルシアリース区分の更新
                        cp_last_lease_cd = result['ウエルシアリース区分']

                for ws in wb.worksheets:

                    # 各シートの処理開始時にrow_numを初期化
                    row_num = start_row

                    start_row_num = start_row

                    # レコード数をカウント
                    record_count = 0
                    table_record_count = 0

                    first_exec = False

                    # シート名が納入先CDのパターン
                    if ws.title == "なし":
                        for row_data in storedresults['results'][0]:
                            # シートが納入先で分かれているときの処理
                            if ws['B3'].value == row_data['ウエルシアリース区分']:

                                if first_exec == False:
                                    last_bukken_cd = row_data['ウエルシア物件区分CD']
                                    first_exec = True

                                # 物件CDが切り替わったら合計を追加
                                if last_bukken_cd != row_data['ウエルシア物件区分CD']:
                                    # ヘッダーの追加
                                    range_copy_cell(temp_ws, ws, 1, 1, 20, 2, 0, row_num +1, True)
                                    # 合計式の追加
                                    range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                                    # 合計式の更新
                                    ws.cell(row=row_num, column=1, value="合計")
                                    ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                                    start_row_num += 4 + table_record_count#表ごとのレコード数
                                    row_num += 4
                                    table_record_count = 0

                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=row_data['ウエルシアリース区分'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=row_data['ウエルシア物件区分CD'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=start_row_num-2, column=4, value="{}月締切".format(end_month))
                                ws.cell(row=row_num, column=4, value=row_data['納期S'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=row_data['納入先CD'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=row_data['納入先名1'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=row_data['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=8)._style)
                                ws.cell(row=row_num, column=9, value=row_data['部材費'])._style = copy(ws.cell(row=start_row, column=9)._style)
                                ws.cell(row=row_num, column=10, value=row_data['施工費'])._style = copy(ws.cell(row=start_row, column=10)._style)
                                ws.cell(row=start_row_num-2, column=11, value="作成日：{}".format(datetime.datetime.now().strftime("%Y/%m/%d")))
                                ws.cell(row=row_num, column=11, value=row_data['販売割戻金'])._style = copy(ws.cell(row=start_row, column=11)._style)
                                ws.cell(row=row_num, column=12, value=row_data['外税額'])._style = copy(ws.cell(row=start_row, column=12)._style)
                                ws.cell(row=row_num, column=13, value=row_data['特別出精値引'])._style = copy(ws.cell(row=start_row, column=13)._style)
                                ws.cell(row=row_num, column=14, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=14)._style)
                                ws.cell(row=row_num, column=15, value=row_data['税込金額'])._style = copy(ws.cell(row=start_row, column=15)._style)
                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1
                                table_record_count += 1
                                # 最終処理物件CDを更新
                                last_bukken_cd = row_data['ウエルシア物件区分CD']
                                last_bukken_name = row_data['ウエルシア物件区分名']

                        range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                        # 合計式の更新
                        ws.cell(row=row_num, column=1, value="合計")
                        ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                        range_copy_cell(temp_total_ws, ws, 5, 3, 15, 4, 0, row_num, True)
                        #総合計
                        ws.cell(row=row_num+3, column=5, value='{}月支払合計'.format(end_month))
                        ws.cell(row=row_num+4, column=9, value='=SUMIF(A1:A{end_col},"*合計",I1:I{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=10, value='=SUMIF(A1:A{end_col},"*合計",J1:J{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=11, value='=SUMIF(A1:A{end_col},"*合計",K1:K{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=12, value='=SUMIF(A1:A{end_col},"*合計",L1:L{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=13, value='=SUMIF(A1:A{end_col},"*合計",M1:M{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=14, value='=SUMIF(A1:A{end_col},"*合計",N1:N{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=15, value='=SUMIF(A1:A{end_col},"*合計",O1:O{end_col})'.format(end_col=row_num+3))

                        # 改ページプレビューの設定
                        ws.sheet_view.view = 'pageBreakPreview'
                        ws.sheet_view.zoomScaleSheetLayoutView= 100

                    if ws.title == "通常":
                        for row_data in storedresults['results'][0]:
                            # シートが納入先で分かれているときの処理
                            if ws['B3'].value == row_data['ウエルシアリース区分']:

                                if first_exec == False:
                                    last_bukken_cd = row_data['ウエルシア物件区分CD']
                                    first_exec = True

                                # 物件CDが切り替わったら合計を追加
                                if last_bukken_cd != row_data['ウエルシア物件区分CD']:
                                    # ヘッダーの追加
                                    range_copy_cell(temp_ws, ws, 1, 1, 20, 2, 0, row_num +1, True)
                                    # 合計式の追加
                                    range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                                    # 合計式の更新
                                    ws.cell(row=row_num, column=1, value="{} 合計".format(last_bukken_name))
                                    ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                                    start_row_num += 4 + table_record_count#表ごとのレコード数
                                    row_num += 4
                                    table_record_count = 0

                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=row_data['ウエルシアリース区分'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=row_data['ウエルシア物件区分CD'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=start_row_num-2, column=4, value="{}月締切".format(end_month))
                                ws.cell(row=row_num, column=4, value=row_data['納期S'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=row_data['納入先CD'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=row_data['納入先名1'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=row_data['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=8)._style)
                                ws.cell(row=row_num, column=9, value=row_data['部材費'])._style = copy(ws.cell(row=start_row, column=9)._style)
                                ws.cell(row=row_num, column=10, value=row_data['施工費'])._style = copy(ws.cell(row=start_row, column=10)._style)
                                ws.cell(row=start_row_num-2, column=11, value="作成日：{}".format(datetime.datetime.now().strftime("%Y/%m/%d")))
                                ws.cell(row=row_num, column=11, value=row_data['販売割戻金'])._style = copy(ws.cell(row=start_row, column=11)._style)
                                ws.cell(row=row_num, column=12, value=row_data['外税額'])._style = copy(ws.cell(row=start_row, column=12)._style)
                                ws.cell(row=row_num, column=13, value=row_data['特別出精値引'])._style = copy(ws.cell(row=start_row, column=13)._style)
                                ws.cell(row=row_num, column=14, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=14)._style)
                                ws.cell(row=row_num, column=15, value=row_data['税込金額'])._style = copy(ws.cell(row=start_row, column=15)._style)
                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1
                                table_record_count += 1
                                # 最終処理物件CDを更新
                                last_bukken_cd = row_data['ウエルシア物件区分CD']
                                last_bukken_name = row_data['ウエルシア物件区分名']

                        range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                        # 合計式の更新
                        ws.cell(row=row_num, column=1, value="{} 合計".format(last_bukken_name))
                        ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                        range_copy_cell(temp_total_ws, ws, 5, 3, 15, 4, 0, row_num, True)
                        #総合計
                        ws.cell(row=row_num+3, column=5, value='{}月支払合計'.format(end_month))
                        ws.cell(row=row_num+4, column=9, value='=SUMIF(A1:A{end_col},"*合計",I1:I{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=10, value='=SUMIF(A1:A{end_col},"*合計",J1:J{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=11, value='=SUMIF(A1:A{end_col},"*合計",K1:K{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=12, value='=SUMIF(A1:A{end_col},"*合計",L1:L{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=13, value='=SUMIF(A1:A{end_col},"*合計",M1:M{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=14, value='=SUMIF(A1:A{end_col},"*合計",N1:N{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=15, value='=SUMIF(A1:A{end_col},"*合計",O1:O{end_col})'.format(end_col=row_num+3))

                        # 改ページプレビューの設定
                        ws.sheet_view.view = 'pageBreakPreview'
                        ws.sheet_view.zoomScaleSheetLayoutView= 100

                    if ws.title == "リース":
                        for row_data in storedresults['results'][0]:
                            # シートが納入先で分かれているときの処理
                            if ws['B3'].value == row_data['ウエルシアリース区分']:

                                if first_exec == False:
                                    last_bukken_cd = row_data['ウエルシア物件区分CD']
                                    first_exec = True

                                # 物件CDが切り替わったら合計を追加
                                if last_bukken_cd != row_data['ウエルシア物件区分CD']:
                                    # ヘッダーの追加
                                    range_copy_cell(temp_ws, ws, 1, 1, 20, 2, 0, row_num +1, True)
                                    # 合計式の追加
                                    range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                                    # 合計式の更新
                                    ws.cell(row=row_num, column=1, value="{} 合計".format(last_bukken_name))
                                    ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                                    ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                                    start_row_num += 4 + table_record_count#表ごとのレコード数
                                    row_num += 4
                                    table_record_count = 0

                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=row_data['ウエルシアリース区分'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=row_data['ウエルシア物件区分CD'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=start_row_num-2, column=4, value="{}月締切".format(end_month))
                                ws.cell(row=row_num, column=4, value=row_data['納期S'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=row_data['納入先CD'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=row_data['納入先名1'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=row_data['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=8)._style)
                                ws.cell(row=row_num, column=9, value=row_data['部材費'])._style = copy(ws.cell(row=start_row, column=9)._style)
                                ws.cell(row=row_num, column=10, value=row_data['施工費'])._style = copy(ws.cell(row=start_row, column=10)._style)
                                ws.cell(row=start_row_num-2, column=11, value="作成日：{}".format(datetime.datetime.now().strftime("%Y/%m/%d")))
                                ws.cell(row=row_num, column=11, value=row_data['販売割戻金'])._style = copy(ws.cell(row=start_row, column=11)._style)
                                ws.cell(row=row_num, column=12, value=row_data['外税額'])._style = copy(ws.cell(row=start_row, column=12)._style)
                                ws.cell(row=row_num, column=13, value=row_data['特別出精値引'])._style = copy(ws.cell(row=start_row, column=13)._style)
                                ws.cell(row=row_num, column=14, value=row_data['合計金額'])._style = copy(ws.cell(row=start_row, column=14)._style)
                                ws.cell(row=row_num, column=15, value=row_data['税込金額'])._style = copy(ws.cell(row=start_row, column=15)._style)
                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1
                                table_record_count += 1
                                # 最終処理物件CDを更新
                                last_bukken_cd = row_data['ウエルシア物件区分CD']
                                last_bukken_name = row_data['ウエルシア物件区分名']

                        range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                        # 合計式の更新
                        ws.cell(row=row_num, column=1, value="{} 合計".format(last_bukken_name))
                        ws.cell(row=row_num, column=9, value="=SUM(I{}:I{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=10, value="=SUM(J{}:J{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=11, value="=SUM(K{}:K{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=12, value="=SUM(L{}:L{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=13, value="=SUM(M{}:M{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=14, value="=SUM(N{}:N{})".format(start_row_num,row_num-1))
                        ws.cell(row=row_num, column=15, value="=SUM(O{}:O{})".format(start_row_num,row_num-1))

                        range_copy_cell(temp_total_ws, ws, 5, 3, 15, 4, 0, row_num, True)
                        #総合計
                        ws.cell(row=row_num+3, column=5, value='{}月支払合計'.format(end_month))
                        ws.cell(row=row_num+4, column=9, value='=SUMIF(A1:A{end_col},"*合計",I1:I{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=10, value='=SUMIF(A1:A{end_col},"*合計",J1:J{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=11, value='=SUMIF(A1:A{end_col},"*合計",K1:K{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=12, value='=SUMIF(A1:A{end_col},"*合計",L1:L{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=13, value='=SUMIF(A1:A{end_col},"*合計",M1:M{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=14, value='=SUMIF(A1:A{end_col},"*合計",N1:N{end_col})'.format(end_col=row_num+3))
                        ws.cell(row=row_num+4, column=15, value='=SUMIF(A1:A{end_col},"*合計",O1:O{end_col})'.format(end_col=row_num+3))

                        # 改ページプレビューの設定
                        ws.sheet_view.view = 'pageBreakPreview'
                        ws.sheet_view.zoomScaleSheetLayoutView= 100

                wb.remove(wb['Template'])
                wb.remove(wb['Template_Total'])

                # Excelオブジェクトの保存
                save_excel_to_buffer(buffer, wb, None)


            else:
                # ウエルシア請求明細_経理用
                # 初回コピー用変数
                copied_lease = False
                copied_none = False

                # Templateシートをコピー
                for result in storedresults['results'][0]:
                    if result['ウエルシアリース区分'] == 1 and cp_last_bukken_name != result['ウエルシア物件区分名']:
                        ws = wb.copy_worksheet(temp_ws)
                        ws.title = "通常 {}".format(result['ウエルシア物件区分名'])
                        ws['A1'] = 1
                        ws['D1'] = result['ウエルシア物件区分名']
                        delete_row_with_merged_ranges(ws, 5)
                        cp_last_bukken_name = result['ウエルシア物件区分名']
                    elif result['ウエルシアリース区分'] == 2 and copied_lease == False:
                        ws = wb.copy_worksheet(temp_ws)
                        copied_lease = True
                        ws.title = "リース".format(result['ウエルシア物件区分名'])
                        ws['A1'] = 2
                        delete_row_with_merged_ranges(ws, 5)
                    elif result['ウエルシアリース区分'] == 0 and copied_none == False:
                        ws = wb.copy_worksheet(temp_ws)
                        copied_none = True
                        ws.title = "なし".format(result['ウエルシア物件区分名'])
                        ws['A1'] = 0
                        delete_row_with_merged_ranges(ws, 5)

                for ws in wb.worksheets:
                    # レコード数をカウント
                    record_count = 0
                    # 各シートの処理開始時にrow_numを初期化
                    row_num = start_row

                    if ws.title == "なし":
                        for result in storedresults['results'][0]:
                            if result['ウエルシアリース区分'] == ws['A1'].value:
                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=result['納入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=result['納入先名1'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=row_num, column=4, value=result['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=result['合計金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=result['外税額'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=result['合計金額']+result['外税額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=result['見積番号'])._style = copy(ws.cell(row=start_row, column=8)._style)

                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1

                    elif ws.title == "リース":
                        for result in storedresults['results'][0]:
                            if result['ウエルシアリース区分'] == ws['A1'].value:
                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=result['納入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=result['納入先名1'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=row_num, column=4, value=result['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=result['合計金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=result['外税額'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=result['合計金額']+result['外税額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=result['見積番号'])._style = copy(ws.cell(row=start_row, column=8)._style)

                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1

                    else:
                        for result in storedresults['results'][0]:
                            if result['ウエルシア物件区分名'] == ws['D1'].value and result['ウエルシアリース区分'] == ws['A1'].value:
                                record_count += 1
                                ws.cell(row=row_num, column=1, value=record_count)._style = copy(ws.cell(row=start_row, column=1)._style)
                                ws.cell(row=row_num, column=2, value=result['納入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                                ws.cell(row=row_num, column=3, value=result['納入先名1'])._style = copy(ws.cell(row=start_row, column=3)._style)
                                ws.cell(row=row_num, column=4, value=result['ウエルシア物件内容名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                                ws.cell(row=row_num, column=5, value=result['合計金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                                ws.cell(row=row_num, column=6, value=result['外税額'])._style = copy(ws.cell(row=start_row, column=6)._style)
                                ws.cell(row=row_num, column=7, value=result['合計金額']+result['外税額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                                ws.cell(row=row_num, column=8, value=result['見積番号'])._style = copy(ws.cell(row=start_row, column=8)._style)

                                # 高さ調整
                                ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                                # データを書き込んだ後、次の行に移動
                                row_num += 1
                    range_copy_cell(temp_ws, ws, 1, 5, 20, 5, 0, row_num -5, True)
                    # 合計式の更新
                    if ws.title == "リース":
                        ws.cell(row=row_num, column=2, value="リース合計金額")
                    else:
                        ws.cell(row=row_num, column=2, value="合計金額")
                    ws.cell(row=row_num, column=5, value="=SUM(E{}:E{})".format(start_row,row_num-1))
                    ws.cell(row=row_num, column=6, value="=SUM(F{}:F{})".format(start_row,row_num-1))
                    ws.cell(row=row_num, column=7, value="=SUM(G{}:G{})".format(start_row,row_num-1))

                    ws.cell(row=start_row-2, column=7, value="{}".format(datetime.datetime.strptime(request.params['@iS集計日付'], '%Y/%m/%d').strftime('%Y年%m月')))

                    ws['A1'] = ""
                    ws['D1'] = ""

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                wb.remove(wb['Template'])

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
