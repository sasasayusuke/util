from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell, relative_reference_copy, ExcelPDFConverter
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.service_utils import ClsTokuisaki

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class BuppinjuryousyoService(BaseService):
    """物品受領書サービス"""

    def display(self, request, session) -> StreamingResponse:

        # 物品受領書の印刷処理
        logger.info("【START】物品受領書処理")

        storedname="usp_MT0301見積書出力_客在"

        params = {
            "@i見積番号": request.params['@i見積番号']
        }

        # STORED実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=params, outputparams={"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}))

        if storedresults["count"] < 1:
            raise ServiceError('該当データなし')

        with get_excel_buffer() as buffer:
            wb = create_excel_object('Template_物品受領書.xlsx')
            ws = wb.active

            # 初期位置指定
            start_row = 21

            # シート名の変更
            ws.title = '物品受領書'

            #ヘッダーデータ記載
            for row_hedder_data in storedresults['results'][0]:
                # 作成日の追加
                if str(row_hedder_data['見積日出力']) == '1':
                    ws['L1'] = row_hedder_data['見積日付'].strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
                else:
                    ws['L1'] = datetime.today().strftime('%Y{0}%m{1}%d{2}').format(*'年月日')

                # 見積番号の追加
                ws['A1'] = 'No.' + str(row_hedder_data['見積番号'])
                # 得意先名の追加
                if str(row_hedder_data['得意先名2']) == '':
                    ws['A4'] = row_hedder_data['得意先名1'] + '　御中'
                else:
                    ws['A3'] = row_hedder_data['得意先名1']
                    ws['A4'] = row_hedder_data['得意先名2'] + '　御中'
                # 金額の追加
                # ws.cell(row=9, column=4, value=row_siwake_data['仕分番号'])._style = copy(ws.cell(row=start_row, column=1)._style)
                ws['D9'] = row_hedder_data['合計金額']
                zeiritu = str(Bu.getZeiritu(session, request))
                ws['A10'] = '消費税等(' + zeiritu + '%)'
                ws.cell(row=10, column=4, value="=D9/{}".format(zeiritu))
                logger.debug('金額OK')
                # 見積件名
                ws['D13'] = row_hedder_data['見積件名']
                # 納期
                if str(row_hedder_data['納期表示']) == '0':
                    if str(row_hedder_data['納期S']) != 'None':
                        if str(row_hedder_data['納期E']) != 'None':
                            ws['D15'] = row_hedder_data['納期S'] + '～' + row_hedder_data['納期E']
                            # ws['D15'] = datetime.row_hedder_data['納期S'].strftime('%Y{0}%m{1}%d{2}').format(*'年月日') + '～' + datetime.row_hedder_data['納期E'].strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
                        else:
                            ws['D15'] = row_hedder_data['納期S']
                            # ws['D15'] = datetime.row_hedder_data['納期S'].strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
                    else:
                        if str(row_hedder_data['納期E']) != 'None':
                            ws['D15'] = row_hedder_data['納期E']
                            # ws['D15'] = datetime.row_hedder_data['納期E'].strftime('%Y{0}%m{1}%d{2}').format(*'年月日')
                elif str(row_hedder_data['納期表示']) == '1':
                    ws['D15'] = '別途御打ち合せによる'
                elif str(row_hedder_data['納期表示']) == '2':
                    ws['D15'] = row_hedder_data['納期その他']
                # お支払い条件
                if str(row_hedder_data['支払条件']) == '0':
                    ws['D16'] = '別途御打ち合せによる'
                elif str(row_hedder_data['支払条件']) == '1':
                    ws['D16'] = row_hedder_data['支払条件その他']
                # 備考
                ws['D18'] = row_hedder_data['備考']
                # 三和情報追加
                company_info = BuppinjuryousyoService.getKaisya(session, request)
                ws['L10'] = '〒' + company_info['郵便番号']
                ws['L11'] = company_info['住所1'] + company_info['住所2']
                ws['L12'] = 'TEL ' + company_info['電話番号']
                ws['L13'] = 'FAX ' + company_info['FAX番号']
                ws['L15'] = 'コールセンター：' + company_info['コールセンター番号']
                ws['L16'] = '問い合わせ先：' + row_hedder_data['担当者名']
                ws['L17'] = 'E-mail:'
            # 初期位置指定
            start_row = 21

            row_num = start_row

            # 明細データ仕分別合計記載
            # for row_siwake_data in storedresults['results'][1]:
            #     if row_num == 29:
            #         row_num += 3

            #     ws.cell(row=row_num, column=1, value=row_siwake_data['仕分番号'])._style = copy(ws.cell(row=start_row, column=1)._style)
            #     ws.cell(row=row_num, column=2, value=row_siwake_data['仕分名称'])._style = copy(ws.cell(row=start_row, column=2)._style)
            #     ws.cell(row=row_num, column=13, value=row_siwake_data['売上金額'])._style = copy(ws.cell(row=start_row, column=13)._style)
            #     # データを書き込んだ後、次の行に移動
            #     row_num += 1

            # 明細データ記載
            page_break_flg = False
            page_num = 1
            page_row_num = 1
            for row_siwake_data in storedresults['results'][1]:
                # 改ページする
                if page_break_flg:
                    page_break_flg = False

                    # ページ追加
                    row_num = row_num + (31 - page_row_num)
                    page_num += 1
                    range_copy_cell(ws,ws,1,30,13,59,0,row_num-32,False)
                    ws.unmerge_cells('B{t1}:D{t1}'.format(t1=str(row_num-1)))
                    range_copy_cell(ws,ws,1,row_num-31,13,row_num-31,0,30,True)
                    relative_reference_copy(ws,ws,9,row_num-30,0,30)
                    relative_reference_copy(ws,ws,12,row_num-30,0,30)
                    page_row_num = 1

                # 仕分名称を追加
                ws.cell(row=row_num, column=2, value=row_siwake_data['仕分名称'])._style = copy(ws.cell(row=start_row, column=2)._style)
                # データを書き込んだ後、次の行に移動
                row_num += 1
                page_row_num += 1

                for row_data in storedresults['results'][2]:
                    if row_siwake_data['仕分番号'] == row_data['仕分番号']:
                        # 改ページ
                        if page_num == 1:
                            if page_row_num == 9:
                                row_num += 3
                                page_num += 1
                                page_row_num = 1
                        else:
                            if page_row_num == 28:
                                row_num += 3
                                page_num += 1
                                # ページ追加
                                range_copy_cell(ws,ws,1,30,13,59,0,row_num-32,False)
                                ws.unmerge_cells('B{t1}:D{t1}'.format(t1=str(row_num-1)))
                                range_copy_cell(ws,ws,1,row_num-31,13,row_num-31,0,30,True)
                                relative_reference_copy(ws,ws,9,row_num-30,0,30)
                                relative_reference_copy(ws,ws,12,row_num-30,0,30)
                                page_row_num = 1
  
                        #2行目以降なら行コピー
                        if page_row_num != 1:
                            range_copy_cell(ws,ws,1,row_num-1,13,row_num-1,0,1,False)
                            relative_reference_copy(ws,ws,9,row_num-1,0,1)
                            relative_reference_copy(ws,ws,12,row_num-1,0,1)
                        ws.cell(row=row_num, column=1, value=row_data['行番号'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        if row_data['見積区分'] == 'C' or row_data['見積区分'] == 'A' or row_data['見積区分'] == 'S':
                            ws.cell(row=row_num, column=2, value='  ' + row_data['漢字名称'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        else:
                            ws.cell(row=row_num, column=2, value=row_data['漢字名称'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=5, value=row_data['W'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        if row_data['D'] != 0:
                            ws.cell(row=row_num, column=6, value=row_data['D'])._style = copy(ws.cell(row=start_row, column=6)._style)
                        else:
                            if row_data['D1'] != 0:
                                if row_data['D2'] != 0:
                                    ws.cell(row=row_num, column=6, value=str(row_data['D1']) + '/' + str(row_data['D2']))._style = copy(ws.cell(row=start_row, column=6)._style)
                                else:
                                    ws.cell(row=row_num, column=6, value=row_data['D1'])._style = copy(ws.cell(row=start_row, column=6)._style)
                            else:
                                if row_data['D2'] != 0:
                                    ws.cell(row=row_num, column=6, value=row_data['D2'])._style = copy(ws.cell(row=start_row, column=6)._style)
                        if row_data['H'] != 0:
                            ws.cell(row=row_num, column=7, value=row_data['D'])._style = copy(ws.cell(row=start_row, column=7)._style)
                        else:
                            if row_data['H1'] != 0:
                                if row_data['H2'] != 0:
                                    ws.cell(row=row_num, column=7, value=str(row_data['H1']) + '/' + str(row_data['H2']))._style = copy(ws.cell(row=start_row, column=7)._style)
                                else:
                                    ws.cell(row=row_num, column=7, value=row_data['H1'])._style = copy(ws.cell(row=start_row, column=7)._style)
                            else:
                                if row_data['H2'] != 0:
                                    ws.cell(row=row_num, column=7, value=row_data['H2'])._style = copy(ws.cell(row=start_row, column=7)._style)
                        ws.cell(row=row_num, column=8, value=row_data['数量'])._style = copy(ws.cell(row=start_row, column=8)._style)
                        ws.cell(row=row_num, column=10, value=row_data['単位名'])._style = copy(ws.cell(row=start_row, column=10)._style)

                        # データを書き込んだ後、次の行に移動
                        row_num += 1
                        page_row_num += 1
                    # else:
                    #     continue
                page_break_flg = True

            # pdf、excel判定
            if request.params['type'] == 'pdf':
                # PDFファイルの返却
                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="M{}".format(str(row_num + (27 - page_row_num)))
                )

                pdf_content = converter.generate_pdf()

                return StreamingResponse(
                    io.BytesIO(pdf_content),
                    media_type="application/pdf",
                    headers={
                        "Content-Disposition": f'attachment; filename="sample.pdf"'
                    }
                )
            
            else:
                # 印刷範囲を設定
                ws.print_area = 'A1:M' + str(row_num + (27 - page_row_num))

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
    
    def getKaisya(session, request):
        query = "SELECT * FROM TM会社;"

        # STORED実行
        result = dict(SQLExecutor(session).execute_query(query=query))

        res_kaisya = result['results'][0]

        return res_kaisya
    