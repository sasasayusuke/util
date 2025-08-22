from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse, JSONResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell_by_address, ExcelPDFConverter
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class KaikakekinmototyoService(BaseService):
    """買掛金元帳サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 買掛金元帳の印刷処理
        logger.info("【START】買掛金元帳出力処理")

        storedname1="usp_SH0100買掛金元帳H"
        storedname2="usp_SH0100買掛金元帳M"
        storedname3="usp_支払集計"

        # 支払集計処理
        params3 = {
            "@is仕入先CD": request.params['@is仕入先CD'],
            "@ie仕入先CD": request.params['@ie仕入先CD'],
            "@iSDATE": request.params['@iSDate'],
            "@iEDATE": request.params['@iEDate'],
            "@i締日": request.params['@i締日']
        }

        # 支払集計STORED実行
        storedresults3 = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname3, params=params3, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults3["output_values"]["@RetST"] == -1:
            raise ServiceError(storedresults3["output_values"]["@RetMsg"])

        hakkou_kbn = request.params['@発行区分']

        if hakkou_kbn == '1':
            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )

        params1 = {
            "@is仕入先CD": request.params['@is仕入先CD'],
            "@ie仕入先CD": request.params['@ie仕入先CD'],
            "@iDATE": request.params['@iDATE'],
            "@i締日": request.params['@i締日']
        }

        # ヘッダーSTORED実行
        storedresults1 = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname1, params=params1, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults1["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_買掛金元帳.xlsx')
            ws = wb["Template"]
            ts = wb["Copy"]

            requestDate = request.params['@iDATE']

            # 三和情報追加
            company_info = KaikakekinmototyoService.getKaisya(session, request)
            ts['BJ4'] = company_info['郵便番号']
            ts['BI5'] = company_info['住所1'] + company_info['住所2']
            ts['BK6'] = company_info['電話番号']
            ts['BK7'] = company_info['FAX番号']

            ts['AB5'] = str(requestDate)[:4] + '年 ' + str(requestDate)[5:7] + '月 ' + str(requestDate)[8:10] + '日　締'

            # 初期化
            start_row = 1
            current_row = start_row
            print_last_row = 0

            # データのループ
            for result in storedresults1['results'][0]:

                params2 = {
                "@i仕入先CD": result['仕入先CD'],
                "@iSDATE": request.params['@iSDate'],
                "@iEDATE": request.params['@iEDate']
                }

                # 明細STORED実行
                storedresults2 = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname2, params=params2, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))
                
                if storedresults2["count"] < 1:
                    continue

                # ヘッダー情報をコピー元に入力
                ts['D5'] = result['郵便番号']
                ts['C6'] = result['住所1']
                ts['C7'] = result['住所2']
                ts['C8'] = result['仕入先名1']
                ts['C9'] = result['仕入先名2']
                ts['D11'] = result['仕入先CD']
                ts['H11'] = result['電話番号']

                # 1ページ分をコピー
                range_copy_cell_by_address(ts,ws,'A1','BP52','A{}'.format(current_row),True)
                # 変数初期化
                page_num = 0
                page_row_num = 1
                page_num += 1
                first_page_bool = True
                print_last_row += 53
                ws['BL{}'.format(current_row)] = page_num

                # 初回のページのみ金額情報を入力
                ws['O{}'.format(current_row+10)] = result['前月残高']
                ws['U{}'.format(current_row+9)] = result['支払金額']
                ws['U{}'.format(current_row+10)] = result['調整金額']
                ws['AA{}'.format(current_row+10)] = result['前月残高'] - result['支払金額'] - result['調整金額']
                ws['AG{}'.format(current_row+10)] = result['仕入金額']
                ws['AM{}'.format(current_row+10)] = result['返品金額']
                ws['AS{}'.format(current_row+10)] = result['訂正金額']
                ws['AY{}'.format(current_row+10)] = result['消費税額']
                ws['BE{}'.format(current_row+10)] = result['仕入金額'] + result['返品金額'] + result['訂正金額'] + result['消費税額']
                ws['BK{}'.format(current_row+10)] = result['当月残高']

                sum = result['前月残高']

                current_row += 13

                # データのループ
                for result_detail in storedresults2['results'][0]:
                    if page_row_num == 1:
                        # 繰越残高 or 前頁残高
                        if first_page_bool:
                            ws.cell(row=current_row + 1, column=10, value='繰　越　残　高')
                            ws.cell(row=current_row, column=53, value=sum)
                            first_page_bool = False
                            current_row += 2
                        else:
                            ws.cell(row=current_row + 1, column=10, value='前　頁　残　高')
                            ws.cell(row=current_row, column=53, value=sum)
                            current_row += 2
                    if result_detail['区分'] != 3:
                        if result_detail['見積行番号'] < 8999:
                            if result_detail['区分'] == 0:
                                ws.cell(row=current_row, column=5, value=str(result_detail['見積番号']) + '-' + str(result_detail['見積行番号']))
                            elif result_detail['区分'] == 1:
                                ws.cell(row=current_row, column=5, value=str(result_detail['見積番号']) + '-' + str(result_detail['見積行番号']))
                            ws.cell(row=current_row, column=1, value=result_detail['日付'])
                        ws.cell(row=current_row, column=11, value=result_detail['製品NO'])
                        ws.cell(row=current_row, column=15, value=result_detail['仕様NO'])
                        ws.cell(row=current_row+1, column=10, value=result_detail['品名'])
                        if result_detail['区分'] == 0 and result_detail['見積行番号'] < 8999:
                            if result_detail['W'] != 0 and result_detail['W'] is not None:
                                size = str(result_detail['W']) + 'W'
                            else:
                                size = ''
                            if result_detail['D'] != 0 and result_detail['D'] is not None:
                                if size != '':
                                    size = size + '×' + str(result_detail['D']) + 'D'
                                else:
                                    size = str(result_detail['D']) + 'D'
                            else:
                                if result_detail['D1'] != 0 and result_detail['D1'] is not None and result_detail['D2'] != 0 and result_detail['D2'] is not None:
                                    if size != '':
                                        size = size + '×' + str(result_detail['D1']) + '/' + str(result_detail['D2']) + 'D'
                                    else:
                                        size = str(result_detail['D1']) + '/' + str(result_detail['D2']) + 'D'
                            if result_detail['H'] != 0 and result_detail['H'] is not None:
                                if size != '':
                                    size = size + '×' + str(result_detail['H']) + 'H'
                                else:
                                    size = str(result_detail['H']) + 'H'
                            else:
                                if result_detail['H1'] != 0 and result_detail['H1'] is not None and result_detail['H2'] != 0 and result_detail['H2'] is not None:
                                    if size != '':
                                        size = size + '×' + str(result_detail['H1']) + '/' + str(result_detail['H2']) + 'H'
                                    else:
                                        size = str(result_detail['H1']) + '/' + str(result_detail['H2']) + 'H'
                            ws.cell(row=current_row, column=22, value=size)
                        if result_detail['数量'] != 0:
                            ws.cell(row=current_row, column=31, value=result_detail['数量'])
                        ws.cell(row=current_row, column=35, value=result_detail['単位名'])
                        if result_detail['単価'] != 0:
                            ws.cell(row=current_row, column=38, value=result_detail['単価'])
                        if result_detail['区分'] != 1:
                            ws.cell(row=current_row, column=43, value=result_detail['金額'])
                            if result_detail['見積行番号'] != 8999 and result_detail['見積行番号'] != 9001:
                                sum += result_detail['金額']
                        else:
                            ws.cell(row=current_row, column=48, value=result_detail['金額'])
                            sum -= result_detail['金額']
                        if result_detail['見積行番号'] != 8999 and result_detail['見積行番号'] != 9001:
                            ws.cell(row=current_row, column=53, value=sum)
                        ws.cell(row=current_row, column=58, value=result_detail['納入先名1'])
                    page_row_num += 1
                    current_row += 2
                    if page_row_num == 19:
                        #次ページをコピー
                        range_copy_cell_by_address(ts,ws,'A1','BP52','A{}'.format(print_last_row+1),True)
                        # 変数初期化
                        page_row_num = 1
                        page_num += 1
                        current_row = print_last_row + 1
                        print_last_row += 53
                        ws['BL{}'.format(current_row)] = page_num
                        current_row += 13
                # 次の得意先に移る
                current_row = print_last_row + 1


            # シート名の変更
            ws.title = '買掛金元帳'

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
                    end_cell="BP{}".format(print_last_row)
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
    

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

    def getKaisya(session, request):
        query = "SELECT * FROM TM会社;"

        # STORED実行
        result = dict(SQLExecutor(session).execute_query(query=query))

        res_kaisya = result['results'][0]

        return res_kaisya
    