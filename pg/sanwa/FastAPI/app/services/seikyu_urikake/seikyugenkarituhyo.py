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
from app.utils.service_utils import ModKubuns

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class seikyugenkarituhyoService(BaseService):
    """請求原価率表サービス"""

    def display(self, request, session) -> Dict[str, Any]:

        # 請求原価率表の印刷処理
        logger.info("【START】請求原価率表処理")

        storedname="usp_SE2500請求原価率表出力"

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=request.params, outputparams={"@RetDcnt":"INT", "@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["output_values"]["@RetST"] == -1:
            raise ServiceError(storedresults["output_values"]["@RetMsg"])

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')


        with get_excel_buffer() as buffer:
            wb = create_excel_object('Temp請求原価率表.xlsx')

            ws = wb['Template']

            # 初期位置指定
            start_row = 6

            # 各シートの処理開始時にrow_numを初期化
            row_num = start_row

            # シート名の変更
            ws.title = '請求原価率表'

            for row_data in storedresults['results'][0]:
                #2行目以降なら行コピー
                if row_num != start_row:
                    range_copy_cell(ws,ws,1,row_num-1,10,row_num-1,0,1,True)
                ws.cell(row=row_num, column=1, value=row_data['請求日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                ws.cell(row=row_num, column=2, value=row_data['見積番号'])._style = copy(ws.cell(row=start_row, column=2)._style)
                ws.cell(row=row_num, column=3, value=row_data['見積件名'])._style = copy(ws.cell(row=start_row, column=3)._style)
                ws.cell(row=row_num, column=4, value=row_data['得意先CD'])._style = copy(ws.cell(row=start_row, column=4)._style)
                ws.cell(row=row_num, column=5, value=row_data['物件種別'])._style = copy(ws.cell(row=start_row, column=5)._style)
                ws.cell(row=row_num, column=6, value=ModKubuns.GetBukkenSyubetumei(bukken_no=row_data['物件種別']))._style = copy(ws.cell(row=start_row, column=6)._style)
                ws.cell(row=row_num, column=7, value=row_data['売上金額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                ws.cell(row=row_num, column=8, value=row_data['仕入金額'])._style = copy(ws.cell(row=start_row, column=8)._style)
                ws.cell(row=row_num, column=9, value=row_data['原価率'])._style = copy(ws.cell(row=start_row, column=9)._style)
                ws.cell(row=row_num, column=10, value=seikyugenkarituhyoService.getHyoujungenkaritu(session, row_data['得意先CD'], request))._style = copy(ws.cell(row=start_row, column=10)._style)

                # データを書き込んだ後、次の行に移動
                row_num += 1

            # 作成日の追加
            ws['I1'] = '作成日：' + datetime.today().strftime('%Y/%m/%d')

            # 期間の追加
            start_date = datetime.strptime(request.params['@is請求日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
            end_date = datetime.strptime(request.params['@ie請求日付'], '%Y/%m/%d').strftime('%Y/%m/%d')
            ws['A3'] = "{}～{}".format(start_date, end_date)

            #集計関数設定
            ws.cell(row=5, column=7, value="=SUM(G6:G{})".format(row_num-1))
            ws.cell(row=5, column=8, value="=SUM(H6:H{})".format(row_num-1))
            ws.cell(row=5, column=9, value="=IF(ISERROR(H5*100/G5),0,H5*100/G5)")

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

    def getHyoujungenkaritu(session, tokusakicd, request):
        query = f"""
            SELECT
                ROUND(SUM(
                        CASE
                            WHEN MSM.仕入先CD = '9999'
                            THEN 0
                            ELSE UM.仕入金額
                        END
                ) * 100 / SUM(UM.売上金額),2) AS 標準原価率
            FROM
                TD売上請求H AS SEH
                INNER JOIN TD売上v2 AS UH
                    ON SEH.見積番号 = UH.見積番号
                INNER JOIN TD売上明細v2 AS UM
                    ON UH.売上番号 = UM.売上番号
                LEFT JOIN TD見積 AS MH
                    ON SEH.見積番号 = MH.見積番号
                INNER JOIN TD見積シートM AS MSM
                    ON UM.見積明細連番 = MSM.見積明細連番
            WHERE
                SEH.請求日付 >= DATEADD(dd, 1, EOMONTH(dateadd(month, -6, '{request.params['@is請求日付']}'), -1))
                AND SEH.請求日付 <= EOMONTH('{request.params['@ie請求日付']}', -1)
                AND (MSM.作業区分CD <> 1 OR MSM.作業区分CD IS NULL)
                AND MH.得意先CD = '{tokusakicd}'
            GROUP BY
                MH.得意先CD
        """

        # SQL実行
        result = dict(SQLExecutor(session).execute_query(query=query))

        if result["count"] < 1:
            res_par = 0
        else:
            res_par = float(result['results'][0]['標準原価率'])

        return res_par