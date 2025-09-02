import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.excel_utils import range_copy_cell

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class yaokoSeikyumeisaihyoService(BaseService):
    """ヤオコー請求明細表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 支払予定表の印刷処理
        logger.info("【START】ヤオコー請求明細表処理")

        storedname="usp_SE1900YK請求明細表発行"

        # params作成
        params = {
            "@iS請求日付":request.params["@iS請求日付"],
            "@iE請求日付":request.params["@iE請求日付"],
            "@iYKサプライ区分":request.params["@iYKサプライ区分"],
            "@iYK物件区分":request.params["@iYK物件区分"]
        }

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Temp_YK請求明細表.xlsx')
            ws = wb.active

            # 初期位置指定
            start_row = 10

            # シートコピー用前回処理納入先CD
            ws_last_nounyusaki_cd = None

            # Templateシートを定義
            temp_ws = wb['Template']

            # 物件の場合は納入先ごとにシートを分ける
            if request.params['@iYK物件区分'] == "1":

                # Templateシートをコピー
                for result in storedresults['results'][0]:
                    if ws_last_nounyusaki_cd != result['納入先CD']:
                        ws = wb.copy_worksheet(temp_ws)

                        # シート名の変更
                        ws.title = str(result['納入先CD'])

                        # 前回処理納入先CDの更新
                        ws_last_nounyusaki_cd = result['納入先CD']

                    else:
                        # 前回処理納入先CDの更新
                        ws_last_nounyusaki_cd = result['納入先CD']

            else:
                # Templateシートをコピー
                ws = wb.copy_worksheet(wb['Template'])

                # シート名の変更
                ws.title = str('請求書')

            for ws in wb.worksheets:
                # シート名を取得
                ws_sheet_name = ws.title
                # 各シートの処理開始時にrow_numを初期化
                row_num = start_row

                # 20レコード毎に改ページ処理をするためにレコード数をカウント
                record_count = 0

                # 1シート内の表の数をカウント
                table_count = 1

                # 表を何行ずらすか設定
                shift_row = 75

                # 表ごとに値が変わらないものを先に設定
                # 請求年
                ws['D3'] = int(request.params['@iE請求日付'][0:4])
                # 請求月
                ws['D4'] = int(request.params['@iE請求日付'][5:7])
                # 作成日（画面発行日付）
                ws['P7'] = datetime.datetime.strptime(request.params['@i発行日付'], '%Y/%m/%d').strftime('%Y{}%m{}%d{}').format(*'年月日')
                # 合計用の変数を追加
                sum_abc_total = 71
                sum_abc_from = 10
                sum_abc_to = 69
                sumif_from = 10
                sumif_to = 73

                sum_row_num = 71
                sumif_row_num = 74

                # 1番目の表の合計を追加
                ws.cell(row=sum_row_num, column=12, value="=SUM(M{sum_abc_total}:O{sum_abc_total})".format(sum_abc_total=sum_abc_total))
                ws.cell(row=sum_row_num, column=13, value="=SUM(M{}:M{})".format(sum_abc_from, sum_abc_to))
                ws.cell(row=sum_row_num, column=14, value="=SUM(N{}:N{})".format(sum_abc_from, sum_abc_to))
                ws.cell(row=sum_row_num, column=15, value="=SUM(O{}:O{})".format(sum_abc_from, sum_abc_to))
                ws.cell(row=sumif_row_num, column=10, value='=SUMIF(B{sumif_to}:B{sumif_from},"pageTotal",L{sumif_to}:L{sumif_from})'.format(sumif_to=sumif_to, sumif_from=sumif_from))

                # シート名が納入先CDのパターン
                if ws.title != "請求書":
                    for row_data in storedresults['results'][0]:
                        # シートが納入先で分かれているときの処理
                        if ws_sheet_name == row_data['納入先CD']:
                            ws.cell(row=row_num + 1, column=4, value="")
                            ws.cell(row=row_num + 1, column=6, value=row_data['納入先CD'])
                            ws.cell(row=row_num, column=7, value=row_data['表示用店舗名'])
                            ws.cell(row=row_num + 2, column=7, value=row_data['見積番号'])
                            ws.cell(row=row_num + 1, column=8, value=row_data['受付日付'])
                            ws.cell(row=row_num + 1, column=9, value=row_data['完工日付'])
                            ws.cell(row=row_num, column=10, value=row_data['発注担当者名'])
                            ws.cell(row=row_num, column=11, value="")
                            ws.cell(row=row_num + 1, column=12, value="=SUM(M{row_num_plus1}:O{row_num_plus1})".format(row_num_plus1 = row_num + 1))
                            if row_data['YK請求区分'] == 1:
                                ws.cell(row=row_num + 1, column=13, value=row_data['合計金額'])
                            elif row_data['YK請求区分'] == 2:
                                ws.cell(row=row_num + 1, column=14, value=row_data['合計金額'])
                            elif row_data['YK請求区分'] == 3:
                                ws.cell(row=row_num + 1, column=15, value=row_data['合計金額'])
                            else:
                                ws.cell(row=row_num + 1, column=13, value=row_data['合計金額'])
                            ws.cell(row=row_num + 1, column=16, value=row_data['作業内容'])

                            # データを書き込んだ後、次の行に移動
                            row_num += 3

                            #レコードカウント
                            record_count += 1

                            if record_count == 20:
                                row_num += 15

                                # 表を次のエリアにコピー（値あり）
                                range_copy_cell(temp_ws, ws, 1, 1, 18, 75, 0, shift_row, True)
                                # コピーした表をカウント
                                table_count += 1
                                # コピーした表の合計を追加
                                ws.cell(row=sum_row_num + shift_row, column=12, value="=SUM(M{sum_abc_total}:O{sum_abc_total})".format(sum_abc_total=sum_abc_total + shift_row))
                                ws.cell(row=sum_row_num + shift_row, column=13, value="=SUM(M{}:M{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                                ws.cell(row=sum_row_num + shift_row, column=14, value="=SUM(N{}:N{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                                ws.cell(row=sum_row_num + shift_row, column=15, value="=SUM(O{}:O{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                                ws.cell(row=sumif_row_num + shift_row, column=10, value='=SUMIF(B{sumif_to}:B{sumif_from},"pageTotal",L{sumif_to}:L{sumif_from})'.format(sumif_to=sumif_to + shift_row, sumif_from=sumif_from + shift_row))
                                # 表Noを更新
                                ws.cell(row=7 + shift_row, column=18, value="　№{}".format(table_count))
                                ws.cell(row=71 + shift_row, column=4, value="№{}".format(table_count))
                                # 次のコピー先を設定
                                shift_row += 75
                                # レコードカウントを20から0にリセット
                                record_count = 0

                        else:
                            continue

                # シート名が請求書のパターン
                elif ws.title == "請求書":
                    for row_data in storedresults['results'][0]:
                        ws.cell(row=row_num + 1, column=4, value="")
                        ws.cell(row=row_num + 1, column=6, value=row_data['納入先CD'])
                        ws.cell(row=row_num, column=7, value=row_data['表示用店舗名'])
                        ws.cell(row=row_num + 2, column=7, value=row_data['見積番号'])
                        ws.cell(row=row_num + 1, column=8, value=row_data['受付日付'])
                        ws.cell(row=row_num + 1, column=9, value=row_data['完工日付'])
                        ws.cell(row=row_num, column=10, value=row_data['発注担当者名'])
                        ws.cell(row=row_num, column=11, value="")
                        ws.cell(row=row_num + 1, column=12, value="=SUM(M{row_num_plus1}:O{row_num_plus1})".format(row_num_plus1 = row_num + 1))
                        if row_data['YK請求区分'] == 1:
                            ws.cell(row=row_num + 1, column=13, value=row_data['合計金額'])
                        elif row_data['YK請求区分'] == 2:
                            ws.cell(row=row_num + 1, column=14, value=row_data['合計金額'])
                        elif row_data['YK請求区分'] == 3:
                            ws.cell(row=row_num + 1, column=15, value=row_data['合計金額'])
                        else:
                            ws.cell(row=row_num + 1, column=13, value=row_data['合計金額'])
                        ws.cell(row=row_num + 1, column=16, value=row_data['作業内容'])

                        # データを書き込んだ後、次の行に移動
                        row_num += 3

                        #レコードカウント
                        record_count += 1

                        if record_count == 20:
                            row_num += 15

                            # 表を次のエリアにコピー（値あり）
                            range_copy_cell(temp_ws, ws, 1, 1, 18, 75, 0, shift_row, True)
                            # コピーした表をカウント
                            table_count += 1
                            # コピーした表の合計を追加
                            ws.cell(row=sum_row_num + shift_row, column=12, value="=SUM(M{sum_abc_total}:O{sum_abc_total})".format(sum_abc_total=sum_abc_total + shift_row))
                            ws.cell(row=sum_row_num + shift_row, column=13, value="=SUM(M{}:M{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                            ws.cell(row=sum_row_num + shift_row, column=14, value="=SUM(N{}:N{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                            ws.cell(row=sum_row_num + shift_row, column=15, value="=SUM(O{}:O{})".format(sum_abc_from + shift_row, sum_abc_to + shift_row))
                            ws.cell(row=sumif_row_num + shift_row, column=10, value='=SUMIF(B{sumif_to}:B{sumif_from},"pageTotal",L{sumif_to}:L{sumif_from})'.format(sumif_to=sumif_to + shift_row, sumif_from=sumif_from + shift_row))
                            # 表Noを更新
                            ws.cell(row=7 + shift_row, column=18, value="　№{}".format(table_count))
                            ws.cell(row=71 + shift_row, column=4, value="№{}".format(table_count))
                            # 次のコピー先を設定
                            shift_row += 75
                            # レコードカウントを20から0にリセット
                            record_count = 0

            # Templateシートの削除
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
