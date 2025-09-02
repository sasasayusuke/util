from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell_by_address, ExcelPDFConverter
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class KaikakekinsyukeihyoService(BaseService):
    """買掛金集計表サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 買掛金集計表の印刷処理
        logger.info("【START】買掛金集計表処理")

        storedname="usp_SK0100買掛金集計表出力"
        storedname2="usp_買掛集計"

        # 買掛集計処理
        params2 = {
            "@iDate": request.params['@iDATE'],
            "@i締日": request.params['@i締日']
        }

        # 買掛集計STORED実行
        storedresults2 = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname2, params=params2, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        params = {
            "@iDATE": request.params['@iDATE'],
            "@i開始仕入先": request.params['@i開始仕入先'],
            "@i終了仕入先": request.params['@i終了仕入先']
        }

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_買掛金集計表.xlsx')
            ws = wb["Template"]
            ts = wb["Copy"]

            kaikakeDate = request.params["@iDATE"]
            supplierCodeFrom = request.params["@i開始仕入先"]
            supplierCodeCodeTo = request.params["@i終了仕入先"]

            # ヘッダー設定
            ws["E1"] = str(kaikakeDate)[:4] + '年' + str(kaikakeDate)[5:7] + '月末締    '
            ws["E2"] = '[' + (supplierCodeFrom if supplierCodeFrom is not None else "最初") + ']'
            ws["I2"] = '[' + (supplierCodeCodeTo if supplierCodeCodeTo is not None else "最後") + ']'
            ws["BH1"] = datetime.now() 

            # 初期化
            start_row = 5
            current_row = start_row

            # 総合計用リスト
            sum_list = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]

            # データのループ
            for result in storedresults['results'][0]:

                # 詳細行をコピー
                range_copy_cell_by_address(ts,ws,'A1','BP2','A{}'.format(current_row),True)

                # データ挿入
                ws.cell(row=current_row+1, column=1, value=result['仕入先CD'])
                if result['仕入先名2'] == '':
                    ws.cell(row=current_row+1, column=4, value=result['仕入先名1'])
                else:
                    ws.cell(row=current_row, column=4, value=result['仕入先名1'])
                    ws.cell(row=current_row+1, column=4, value=result['仕入先名2'])
                ws.cell(row=current_row+1, column=15, value=result['前月残高'])
                ws.cell(row=current_row+1, column=19, value=result['仕入金額'])
                ws.cell(row=current_row+1, column=23, value=result['返品金額'])
                ws.cell(row=current_row+1, column=27, value=result['訂正金額'])
                ws.cell(row=current_row+1, column=31, value=result['差引仕入額'])
                ws.cell(row=current_row+1, column=35, value=result['消費税額'])
                ws.cell(row=current_row+1, column=39, value=result['税込純仕入額'])
                ws.cell(row=current_row+1, column=43, value=result['支払金額'])
                ws.cell(row=current_row+1, column=47, value=result['調整金額'])
                ws.cell(row=current_row+1, column=51, value=result['当月残高'])
                ws.cell(row=current_row+1, column=55, value=str(result['伝票枚数']) + ' 枚')
                ws.cell(row=current_row+1, column=59, value=result['非抜税抜仕入金額'])
                ws.cell(row=current_row+1, column=64, value=result['非課税金額'])

                # 総合計に加算
                sum_list[0] += result['前月残高']
                sum_list[1] += result['仕入金額']
                sum_list[2] += result['返品金額']
                sum_list[3] += result['訂正金額']
                sum_list[4] += result['差引仕入額']
                sum_list[5] += result['消費税額']
                sum_list[6] += result['税込純仕入額']
                sum_list[7] += result['支払金額']
                sum_list[8] += result['調整金額']
                sum_list[9] += result['当月残高']
                sum_list[10] += result['非抜税抜仕入金額']
                sum_list[11] += result['非課税金額']

                current_row += 2
            
            # 総合計行をコピー
            range_copy_cell_by_address(ts,ws,'A5','BP6','A{}'.format(current_row),True)

            # データ挿入
            ws.cell(row=current_row+1, column=15, value=sum_list[0])
            ws.cell(row=current_row+1, column=19, value=sum_list[1])
            ws.cell(row=current_row+1, column=23, value=sum_list[2])
            ws.cell(row=current_row+1, column=27, value=sum_list[3])
            ws.cell(row=current_row+1, column=31, value=sum_list[4])
            ws.cell(row=current_row+1, column=35, value=sum_list[5])
            ws.cell(row=current_row+1, column=39, value=sum_list[6])
            ws.cell(row=current_row+1, column=43, value=sum_list[7])
            ws.cell(row=current_row+1, column=47, value=sum_list[8])
            ws.cell(row=current_row+1, column=51, value=sum_list[9])
            ws.cell(row=current_row+1, column=59, value=sum_list[10])
            ws.cell(row=current_row+1, column=64, value=sum_list[11])

            current_row += 2

            # シート名の変更
            ws.title = '買掛金集計表'

            # コピー用シートを削除
            wb.remove(ts)

            res_obj = None
            res_type = 'pdf'
            if res_type == 'pdf':
                # PDFファイルの返却
                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="BP{}".format(current_row)
                )
                res_obj = converter.generate_pdf()

                logger.debug("Pdf file generated successfully")
            else:
                # Excelオブジェクトの保存
                save_excel_to_buffer(buffer, wb, None)
                res_obj = buffer.getvalue()

                logger.debug("Excel file generated successfully")

            media_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" if res_type == "pdf" else "application/pdf"

            return StreamingResponse(
                io.BytesIO(res_obj),
                media_type=media_type,
                headers={
                    'Content-Disposition': 'attachment; filename="sample.{}"'.format(res_type)
                }
            )

    def display_siwake(self, request, session) -> StreamingResponse:

        # 買掛金集計表の印刷処理
        logger.info("【START】買掛金集計表仕分処理")

        storedname="usp_SK0101買掛金集計表仕訳出力"

        params = {
            "@iDATE": request.params['@iDATE'],
            "@i開始仕入先": request.params['@i開始仕入先'],
            "@i終了仕入先": request.params['@i終了仕入先'],
            "@i仕訳日付": request.params['@i仕訳日付']
        }

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')
        
        csv_data = ''
        first_row_bool = True

        # データのループ
        for row_data in storedresults['results'][0]:
            if not first_row_bool:
                csv_data += '\r\n'
            else:
                first_row_bool = False
            if row_data['年'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['年']) + ','
            if row_data['月'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['月']) + ','
            if row_data['日'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['日']) + ','
            if row_data['伝票番号'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['伝票番号']) + ','
            if row_data['借方金額'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方金額']) + ','
            if row_data['借方勘定科目CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方勘定科目CD']) + ','
            if row_data['借方勘定科目名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方勘定科目名']) + ','
            if row_data['借方消費税CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方消費税CD']) + ','
            if row_data['借方消費税率'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方消費税率']) + ','
            if row_data['借方業種'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方業種']) + ','
            if row_data['借方補助科目CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方補助科目CD']) + ','
            if row_data['借方補助科目名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方補助科目名']) + ','
            if row_data['借方部門CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方部門CD']) + ','
            if row_data['借方部門名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['借方部門名']) + ','
            if row_data['貸方勘定科目CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方勘定科目CD']) + ','
            if row_data['貸方勘定科目名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方勘定科目名']) + ','
            if row_data['貸方消費税CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方消費税CD']) + ','
            if row_data['貸方消費税率'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方消費税率']) + ','
            if row_data['貸方業種'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方業種']) + ','
            if row_data['貸方補助科目CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方補助科目CD']) + ','
            if row_data['貸方補助科目名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方補助科目名']) + ','
            if row_data['貸方部門CD'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方部門CD']) + ','
            if row_data['貸方部門名'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方部門名']) + ','
            if row_data['貸方金額'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['貸方金額']) + ','
            if row_data['摘要'] is None:
                csv_data += '"",'
            else:
                csv_data = csv_data + '"' + str(row_data['摘要']) + '",'
            if row_data['期日_年'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['期日_年']) + ','
            if row_data['期日_月'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['期日_月']) + ','
            if row_data['期日_日'] is None:
                csv_data += ','
            else:
                csv_data = csv_data + str(row_data['期日_日']) + ','
            if row_data['手形番号'] is None:
                csv_data += ''
            else:
                csv_data = csv_data + str(row_data['手形番号'])

        csv_data = csv_data.encode('cp932')
        # csv_data = csv_data.encode('shift-jis')

        return StreamingResponse(
            io.BytesIO(csv_data),
            media_type="text/csv; charset=Shift-JIS",
            headers={
                'Content-Disposition': 'attachment; filename="sample.csv"'
            }
        )

        

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass
