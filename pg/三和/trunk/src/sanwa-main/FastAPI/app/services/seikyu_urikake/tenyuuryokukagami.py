from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse, JSONResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell_by_address, ExcelPDFConverter
from app.utils.service_utils import Snw_cm
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class TenyuuryokukagamiService(BaseService):
    """手入力鏡発行サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 手入力鏡の印刷処理
        logger.info("【START】手入力鏡発行処理")

        mirrorNo = request.params['@鏡番号']
        requestDate = datetime.strptime(request.params['@請求日付'], "%Y/%m/%d")
        tokuiCD = request.params['@得意先CD']
        tokuiName = request.params['@得意先名']
        nounyuCD = request.params['@納入先CD']
        nounyuName = request.params['@納入先名']
        mitumoriNO = request.params['@見積番号']
        sheetsCnt = request.params['@明細書数']
        location = request.params['@受渡地']
        amount = request.params['@金額']
        tax = request.params['@消費税']
        if amount is not None:
            amount = int((request.params['@金額']).replace(',',''))
            tax = int((request.params['@消費税']).replace(',',''))
            sum = int((request.params['@金額']).replace(',','')) + int((request.params['@消費税']).replace(',',''))
        else:
            amount = 0
            tax = 0
            sum = 0
        tekiyou1 = request.params['@摘要1']
        tekiyou2 = request.params['@摘要2']
        tekiyou3 = request.params['@摘要3']
        tekiyou4 = request.params['@摘要4']
        tekiyou5 = request.params['@摘要5']
        tekiyou6 = request.params['@摘要6']
        transfer_flg = request.params['@振込負担テキスト表示flg']
        if tokuiCD is not None:
            tokui_name1 = tokuiName
            tokui_name2 = nounyuName
        else:
            tokui_name1 = ''
            tokui_name2 = ''

        # TD手入力鏡更新処理
        insert_sql = "INSERT INTO TD手入力鏡 (鏡番号,請求日付,得意先CD,得意先名1,得意先名2,納入先CD,受渡地,明細書数,金額,消費税,初期登録日,登録変更日,摘要1,摘要2,摘要3,摘要4,摘要5,摘要6) VALUES ("
        if mirrorNo is None:
            mirrorNo = Snw_cm.GetCounter(self,session,'鏡番号')['output_values']['@GetNO']
        insert_sql += str(mirrorNo)
        if requestDate is None:
            insert_sql += ",null"
        else:
            insert_sql += ",'" + str(requestDate) + "'"
        if tokuiCD is None:
            insert_sql += ",null"
        else:
            insert_sql += ",'" + str(tokuiCD) + "'"
        if tokui_name1 == '':
            insert_sql += ",null"
        else:
            insert_sql += ",'" + tokui_name1 + "'"
        if tokui_name2 == '':
            insert_sql += ",null"
        else:
            insert_sql += ",'" + tokui_name2 + "'"
        if nounyuCD is None:
            insert_sql += ",null"
        else:
            insert_sql += ",'" + str(nounyuCD) + "'"
        if location is None:
            insert_sql += ",null"
        else:
            insert_sql += ",'" + location + "'"
        if sheetsCnt is None:
            insert_sql += ",0"
        else:
            insert_sql += "," + str(sheetsCnt) + ""
        insert_sql += "," + str(amount) + ""
        insert_sql += "," + str(tax) + ""
        insert_sql += ",CONVERT(DATE, GETDATE())"
        insert_sql += ",CONVERT(DATE, GETDATE())"
        if tekiyou1 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou1 + "'"
        if tekiyou2 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou2 + "'"
        if tekiyou3 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou3 + "'"
        if tekiyou4 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou4 + "'"
        if tekiyou5 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou5 + "'"
        if tekiyou6 is None:
            insert_sql += ",''"
        else:
            insert_sql += ",'" + tekiyou6 + "'"
        insert_sql += ");"
        insert_result1 = dict(SQLExecutor(session).execute_query(query=insert_sql))
        # TD手入力鏡内訳の更新
        if mitumoriNO is not None:
            m_list = mitumoriNO.split(',')
            for i,item in enumerate(m_list):
                insert_sql2 = "INSERT INTO TD手入力鏡内訳 (鏡番号,枝番,見積番号,初期登録日,登録変更日) VALUES ("
                insert_sql2 += str(mirrorNo)
                insert_sql2 += "," + str(i + 1)
                insert_sql2 += "," + str(item)
                insert_sql2 += ",CONVERT(DATE, GETDATE())"
                insert_sql2 += ",CONVERT(DATE, GETDATE())"
                insert_sql2 += ");"
                insert_result2 = dict(SQLExecutor(session).execute_query(query=insert_sql2))

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_請求書_鏡.xlsx')
            ws = wb["Template"]

            # ヘッダー情報追加
            ws['D4'] = tokuiCD
            if tokui_name2 == '' or tokui_name2 is None:
                ws['A8'] = tokui_name1
            else:
                ws['A6'] = tokui_name1
                ws['A8'] = tokui_name2

            # 見積番号の展開
            if mitumoriNO is not None:
                m_list_3 = []
                m_str = ''
                for i, item in enumerate(m_list):
                    if i == 10:
                        break
                    if i != 0:
                        m_str += ','
                    if i != 0 and  i % 3 == 0:
                        m_list_3.append(m_str)
                        m_str = item
                    else:
                        m_str += item
                if len(m_list) > 10:
                    m_str += ' 他'
                if m_str != '':
                    m_list_3.append(m_str)
                    num = 4
                for m_str_3 in m_list_3[::-1]:
                    ws['Q{}'.format(num)] = m_str_3
                    num -= 1
            ws['A11'] = requestDate

            # 三和情報追加
            company_info = TenyuuryokukagamiService.getKaisya(session, request)
            ws['O9'] = company_info['インボイス登録番号']
            ws['M11'] = company_info['郵便番号']
            ws['L12'] = company_info['住所1'] + company_info['住所2']
            ws['O13'] = company_info['電話番号']
            ws['O14'] = company_info['FAX番号']

            # 明細行追加
            ws['H18'] = sheetsCnt
            ws['O18'] = amount
            ws['F20'] = location
            ws['H22'] = TenyuuryokukagamiService.getZeiritu(session, request)
            ws['O22'] = tax
            ws['D24'] = tekiyou1
            ws['D25'] = tekiyou2
            ws['D26'] = tekiyou3
            ws['D27'] = tekiyou4
            ws['D28'] = tekiyou5
            ws['D29'] = tekiyou6
            ws['O30'] = sum

            if transfer_flg == '1':
                ws['M34'] = '※振込手数料のご負担をお願いいたします。'


            # シート名の変更
            ws.title = '請求書_鏡'

            res_obj = None
            res_type = 'pdf'
            if res_type == 'pdf':
                # PDFファイルの返却
                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="S41"
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

    def display_update(self, request, session) -> StreamingResponse:

        # 手入力鏡の印刷処理
        logger.info("【START】手入力鏡発行_更新処理")

        mirrorNo = request.params['@鏡番号']
        requestDate = datetime.strptime(request.params['@請求日付'], "%Y/%m/%d")
        tokuiCD = request.params['@得意先CD']
        tokuiName = request.params['@得意先名']
        nounyuCD = request.params['@納入先CD']
        nounyuName = request.params['@納入先名']
        mitumoriNO = request.params['@見積番号']
        sheetsCnt = request.params['@明細書数']
        location = request.params['@受渡地']
        amount = request.params['@金額']
        tax = request.params['@消費税']
        if amount is not None:
            amount = int((request.params['@金額']).replace(',',''))
            tax = int((request.params['@消費税']).replace(',',''))
            sum = int((request.params['@金額']).replace(',','')) + int((request.params['@消費税']).replace(',',''))
        else:
            amount = 0
            tax = 0
            sum = 0
        tekiyou1 = request.params['@摘要1']
        tekiyou2 = request.params['@摘要2']
        tekiyou3 = request.params['@摘要3']
        tekiyou4 = request.params['@摘要4']
        tekiyou5 = request.params['@摘要5']
        tekiyou6 = request.params['@摘要6']
        transfer_flg = request.params['@振込負担テキスト表示flg']
        if tokuiCD is not None:
            tokui_name1 = tokuiName
            tokui_name2 = nounyuName
        else:
            tokui_name1 = ''
            tokui_name2 = ''

        # TD手入力鏡更新処理
        update_sql = "UPDATE TD手入力鏡 SET "
        update_sql += '鏡番号 = '
        update_sql += str(mirrorNo)
        update_sql += ',請求日付 = '
        if requestDate is None:
            update_sql += "null"
        else:
            update_sql += "'" + str(requestDate) + "'"
        update_sql += ',得意先CD = '
        if tokuiCD is None:
            update_sql += "null"
        else:
            update_sql += "'" + str(tokuiCD) + "'"
        update_sql += ',得意先名1 = '
        if tokui_name1 == '':
            update_sql += "null"
        else:
            update_sql += "'" + tokui_name1 + "'"
        update_sql += ',得意先名2 = '
        if tokui_name2 == '':
            update_sql += "null"
        else:
            update_sql += "'" + tokui_name2 + "'"
        update_sql += ',納入先CD = '
        if nounyuCD is None:
            update_sql += "null"
        else:
            update_sql += "'" + str(nounyuCD) + "'"
        update_sql += ',受渡地 = '
        if location is None:
            update_sql += "null"
        else:
            update_sql += "'" + location + "'"
        update_sql += ',明細書数 = '
        if sheetsCnt is None:
            update_sql += "0"
        else:
            update_sql += str(sheetsCnt)
        update_sql += ",金額 = " + str(amount)
        update_sql += ",消費税 = " + str(tax)
        update_sql += ",登録変更日 = CONVERT(DATE, GETDATE())"
        update_sql += ',摘要1 = '
        if tekiyou1 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou1 + "'"
        update_sql += ',摘要2 = '
        if tekiyou2 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou2 + "'"
        update_sql += ',摘要3 = '
        if tekiyou3 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou3 + "'"
        update_sql += ',摘要4 = '
        if tekiyou4 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou4 + "'"
        update_sql += ',摘要5 = '
        if tekiyou5 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou5 + "'"
        update_sql += ',摘要6 = '
        if tekiyou6 is None:
            update_sql += "''"
        else:
            update_sql += "'" + tekiyou6 + "'"
        update_sql += " WHERE 鏡番号 = "
        update_sql += str(mirrorNo)
        update_sql += ";"
        insert_result1 = dict(SQLExecutor(session).execute_query(query=update_sql))

        # TD手入力鏡内訳の削除
        delete_sql = "DELETE FROM TD手入力鏡内訳 WHERE 鏡番号 = " + str(mirrorNo) + ";"

        delete_result1 = dict(SQLExecutor(session).execute_query(query=delete_sql))

        # TD手入力鏡内訳の登録
        if mitumoriNO is not None:
            m_list = mitumoriNO.split(',')
            for i,item in enumerate(m_list):
                insert_sql2 = "INSERT INTO TD手入力鏡内訳 (鏡番号,枝番,見積番号,初期登録日,登録変更日) VALUES ("
                insert_sql2 += str(mirrorNo)
                insert_sql2 += "," + str(i + 1)
                insert_sql2 += "," + str(item)
                insert_sql2 += ",CONVERT(DATE, GETDATE())"
                insert_sql2 += ",CONVERT(DATE, GETDATE())"
                insert_sql2 += ");"
                insert_result2 = dict(SQLExecutor(session).execute_query(query=insert_sql2))

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_請求書_鏡.xlsx')
            ws = wb["Template"]

            # ヘッダー情報追加
            ws['D4'] = tokuiCD
            if tokui_name2 == '' or tokui_name2 is None:
                ws['A8'] = tokui_name1
            else:
                ws['A6'] = tokui_name1
                ws['A8'] = tokui_name2

            # 見積番号の展開
            if mitumoriNO is not None:
                m_list_3 = []
                m_str = ''
                for i, item in enumerate(m_list):
                    if i == 10:
                        break
                    if i != 0:
                        m_str += ','
                    if i != 0 and  i % 3 == 0:
                        m_list_3.append(m_str)
                        m_str = item
                    else:
                        m_str += item
                if len(m_list) > 10:
                    m_str += ' 他'
                if m_str != '':
                    m_list_3.append(m_str)
                    num = 4
                for m_str_3 in m_list_3[::-1]:
                    ws['Q{}'.format(num)] = m_str_3
                    num -= 1
            ws['A11'] = requestDate

            # 三和情報追加
            company_info = TenyuuryokukagamiService.getKaisya(session, request)
            ws['O9'] = company_info['インボイス登録番号']
            ws['M11'] = company_info['郵便番号']
            ws['L12'] = company_info['住所1'] + company_info['住所2']
            ws['O13'] = company_info['電話番号']
            ws['O14'] = company_info['FAX番号']

            # 明細行追加
            ws['H18'] = sheetsCnt
            ws['O18'] = amount
            ws['F20'] = location
            ws['H22'] = TenyuuryokukagamiService.getZeiritu(session, request)
            ws['O22'] = tax
            ws['D24'] = tekiyou1
            ws['D25'] = tekiyou2
            ws['D26'] = tekiyou3
            ws['D27'] = tekiyou4
            ws['D28'] = tekiyou5
            ws['D29'] = tekiyou6
            ws['O30'] = sum

            if transfer_flg == '1':
                ws['M34'] = '※振込手数料のご負担をお願いいたします。'


            # シート名の変更
            ws.title = '請求書_鏡'

            res_obj = None
            res_type = 'pdf'
            if res_type == 'pdf':
                # PDFファイルの返却
                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="S41"
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

    def display_delete(self, request, session) -> StreamingResponse:

        # 手入力鏡の削除処理
        logger.info("【START】手入力鏡データ_削除処理")

        mirrorNo = request.params['@鏡番号']

        # TD手入力鏡内訳の削除
        delete_sql = "DELETE FROM TD手入力鏡内訳 WHERE 鏡番号 = " + str(mirrorNo) + ";"
        delete_result1 = dict(SQLExecutor(session).execute_query(query=delete_sql))

        # TD手入力鏡の削除
        delete_sql = "DELETE FROM TD手入力鏡 WHERE 鏡番号 = " + str(mirrorNo) + ";"
        delete_result1 = dict(SQLExecutor(session).execute_query(query=delete_sql))

        return JSONResponse(
        content={},
        headers={
            'Download-Skip': "yes"
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

    def getTokuisaki(session, request,tokusakiCD):
        query = f"SELECT 得意先名1,得意先名2 FROM TM得意先 where 得意先CD = '{tokusakiCD}';"

        # STORED実行
        result = dict(SQLExecutor(session).execute_query(query=query))

        res_tokuisaki = result['results'][0]

        return res_tokuisaki

    def getZeiritu(session, request):
        date_str = datetime.today().strftime('%Y/%m/%d')
        query = f"select dbo.fn_GetTax('{date_str}') AS 税率"

        # STORED実行
        result = dict(SQLExecutor(session).execute_query(query=query))
        if len(result["results"]) < 1:
            res_par = 0
        else:
            res_par = int(result['results'][0]['税率'])

        return res_par