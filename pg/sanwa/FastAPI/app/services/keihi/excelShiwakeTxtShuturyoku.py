from typing import Dict, Any
import logging
import xlrd
import io
from fastapi.responses import StreamingResponse, JSONResponse
from app.services.base_service import BaseService
from app.utils.string_utils import decode_base64, SpcToNull, encode_string, format_jp_date
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, ExcelPDFConverter
from app.core.config import settings
from app.core.exceptions import ServiceError
from copy import copy
import zipfile

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class ExcelShiwakeTxtShuturyokuService(BaseService):
    """Excel仕分Txt出力サービス"""

    def display(self, request, session) -> StreamingResponse:
        """印刷処理を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        # 経費データの取込処理
        logger.info("【START】EXCEL仕訳TXT出力処理")

        # 保存用リストの作成
        gRsA = []
        gRsA_tempdic = {
            "年": None,
            "月": None,
            "日": None,
            "伝票番号": None,
            "借方金額": None,
            "借方勘定科目CD": None,
            "借方勘定科目名": None,
            "借方消費税CD": None,
            "借方消費税率": None,
            "借方業種": None,
            "借方補助科目CD": None,
            "借方補助科目名": None,
            "借方部門CD": None,
            "借方部門名": None,
            "貸方勘定科目CD": None,
            "貸方勘定科目名": None,
            "貸方消費税CD": None,
            "貸方消費税率": None,
            "貸方業種": None,
            "貸方補助科目CD": None,
            "貸方補助科目名": None,
            "貸方部門CD": None,
            "貸方部門名": None,
            "貸方金額": None,
            "摘要": None,
            "期日_年": None,
            "期日_月": None,
            "期日_日": None,
            "手形番号": None,
        }

        # update

        # EXCEL取込
        obj = decode_base64(request.params["excelData"])

        wb = xlrd.open_workbook(file_contents=obj.read())

        # target_sheets = ["借方勘定科目", "貸方勘定科目", "振替伝票"]

        # for sheet_name in target_sheets:
        #     ws = wb.sheet_by_name(sheet_name)
        #     print(ws.cell_value(rowx=0, colx=0)) # A1の値
        #     print(ws.cell_value(rowx=4, colx=1)) # B5の値

        ws = wb.sheet_by_name("振替伝票")

        # 開始行
        start_row = 3

        # excel行カウント
        row_count = 0

        for row_index in range(start_row,99):
            print(f'row_index:{row_index}')
            xls_year = str(ws.cell_value(rowx=row_index, colx=1)).strip()
            print(f'年:{xls_year}')

            if xls_year == "":
                xls_year_next = str(ws.cell_value(rowx=row_index+1, colx=1)).strip()
                if xls_year_next == "":
                    break

            row_count += 1


        if row_count == 0:
            raise ServiceError('取込データがありません')
        
        csv_data = ''

        for row_index in range(start_row, row_count + start_row):


            # 値チェック
            # 年
            xls_year = str(ws.cell_value(rowx=row_index, colx=1)).strip().split('.')[0]
            if xls_year == None or xls_year == "":
                continue
            else:
                try:
                    int(xls_year)
                except(ValueError, TypeError):
                    continue

            # 月
            xls_month = str(ws.cell_value(rowx=row_index, colx=2)).strip().split('.')[0]
            if xls_month == None or xls_month == "":
                continue
            else:
                try:
                    int(xls_month)
                except(ValueError, TypeError):
                    continue

            # 日
            xls_day = str(ws.cell_value(rowx=row_index, colx=3)).strip().split('.')[0]
            if xls_day == None or xls_day == "":
                continue
            else:
                try:
                    int(xls_day)
                except(ValueError, TypeError):
                    continue

            # 借方金額
            xls_KarikataKingaku = str(ws.cell_value(rowx=row_index, colx=16)).strip().split('.')[0]
            if xls_KarikataKingaku == None or xls_KarikataKingaku == "":
                continue
            else:
                try:
                    int(xls_KarikataKingaku)
                except(ValueError, TypeError):
                    continue

            # 借方勘定科目CD
            xls_KarikataCD = str(ws.cell_value(rowx=row_index, colx=4)).strip().split('.')[0]
            if xls_KarikataCD == None or xls_KarikataCD == "":
                continue
            else:
                try:
                    int(xls_KarikataCD)
                except(ValueError, TypeError):
                    continue

            # 借方勘定科目名
            xls_KarikataKamokumei = str(ws.cell_value(rowx=row_index, colx=8)).strip()
            if xls_KarikataKamokumei != None and xls_KarikataKamokumei != "":
                if len(xls_KarikataKamokumei) > 24:
                    continue

            # 借方消費税率
            xls_KarikataSyohizeiritsu = SpcToNull(ws.cell_value(rowx=row_index, colx=7))
            if xls_KarikataSyohizeiritsu != None:
                try:
                    int(xls_KarikataSyohizeiritsu.split('.')[0])
                except(ValueError, TypeError):
                    continue

            # 借方補助科目CD
            xls_KarikataHozyokamokuCD = SpcToNull(ws.cell_value(rowx=row_index, colx=5))
            if xls_KarikataHozyokamokuCD != None:
                try:
                    int(xls_KarikataHozyokamokuCD.split('.')[0])
                except(ValueError, TypeError):
                    continue

            # 借方補助科目名
            xls_KarikataHozyokamokumei = SpcToNull(ws.cell_value(rowx=row_index, colx=9))
            if xls_KarikataHozyokamokumei != None and xls_KarikataHozyokamokumei != "":
                if len(xls_KarikataHozyokamokumei) > 24:
                    continue

            # 貸方勘定科目CD
            xls_KashikataCD = str(ws.cell_value(rowx=row_index, colx=10)).strip().split('.')[0]
            if xls_KashikataCD == None and xls_KashikataCD == "":
                continue
            else:
                try:
                    int(xls_KashikataCD)
                except(ValueError, TypeError):
                    continue

            # 貸方勘定科目名
            xls_KashikataKamokumei = str(ws.cell_value(rowx=row_index, colx=14)).strip()
            if xls_KashikataKamokumei != None and xls_KashikataKamokumei != "":
                if len(xls_KashikataKamokumei) > 24:
                    continue

            # 貸方消費税CD
            xls_KashikataSyohizeiCD = SpcToNull(ws.cell_value(rowx=row_index, colx=12))
            if xls_KashikataSyohizeiCD != None:
                try:
                    int(xls_KashikataSyohizeiCD.split('.')[0])
                except(ValueError, TypeError):
                    continue

            # 貸方消費税率
            xls_KashikataSyohizeiCD = SpcToNull(ws.cell_value(rowx=row_index, colx=12))
            if xls_KashikataSyohizeiCD != None:
                try:
                    int(xls_KashikataSyohizeiCD.split('.')[0])
                except(ValueError, TypeError):
                    continue

            # 貸方補助科目CD
            xls_KashikataHozyokamokuCD = SpcToNull(ws.cell_value(rowx=row_index, colx=11))
            if xls_KashikataHozyokamokuCD != None:
                try:
                    int(xls_KashikataHozyokamokuCD.split('.')[0])
                except(ValueError, TypeError):
                    continue

            # 貸方補助科目名
            xls_KashikataHozyokamokumei = str(ws.cell_value(rowx=row_index, colx=15)).strip()
            if xls_KashikataHozyokamokumei != None and xls_KashikataHozyokamokumei != "":
                if len(xls_KashikataHozyokamokumei) > 24:
                    continue

            # 貸方金額
            xls_KashikataKingaku = str(ws.cell_value(rowx=row_index, colx=17)).strip().split('.')[0]
            if xls_KashikataKingaku == None or xls_KashikataKingaku == "":
                continue
            else:
                try:
                    int(xls_KashikataKingaku)
                except(ValueError, TypeError):
                    continue

            # 摘要
            xls_tekiyo = str(ws.cell_value(rowx=row_index, colx=18)).strip()
            if xls_tekiyo != None and xls_tekiyo != "":
                if len(xls_tekiyo) > 96:
                    continue



            # csv追加
            # 年
            xls_year = str(int(ws.cell_value(rowx=row_index, colx=1))).strip()
            csv_data = csv_data + xls_year + ','
            # 月
            xls_month = str(int(ws.cell_value(rowx=row_index, colx=2))).strip()
            csv_data = csv_data + xls_month + ','
            # 日
            xls_day = str(int(ws.cell_value(rowx=row_index, colx=3))).strip()
            csv_data = csv_data + xls_day + ','
            # 伝票番号
            xls_DenpyoBangou = None
            csv_data += ','
            # 借方金額
            xls_KarikataKingaku = str(int(ws.cell_value(rowx=row_index, colx=16))).strip()
            csv_data = csv_data + xls_KarikataKingaku + ','

            # 借方勘定科目CD
            xls_KarikataCD = str(int(ws.cell_value(rowx=row_index, colx=4))).strip()
            csv_data = csv_data + xls_KarikataCD + ','
            # 借方勘定科目名
            xls_KarikataKamokumei = str(ws.cell_value(rowx=row_index, colx=8)).strip()
            csv_data = csv_data + '"' + xls_KarikataKamokumei + '"' + ','
            # 借方消費税CD
            xls_KarikataSyohizeiCD = SpcToNull(ws.cell_value(rowx=row_index, colx=6))
            if xls_KarikataSyohizeiCD == None:
                xls_KarikataSyohizeiCD = ""
            else:
                xls_KarikataSyohizeiCD = xls_KarikataSyohizeiCD.split('.')[0]
            csv_data = csv_data + xls_KarikataSyohizeiCD + ','
            # 借方消費税率
            xls_KarikataSyohizeiritsu = SpcToNull(ws.cell_value(rowx=row_index, colx=7))
            if xls_KarikataSyohizeiritsu == None:
                xls_KarikataSyohizeiritsu = ""
            else:
                xls_KarikataSyohizeiritsu = xls_KarikataSyohizeiritsu.split('.')[0]
            csv_data = csv_data + xls_KarikataSyohizeiritsu + ','
            # 借方業種
            xls_KarikataGyosyu = None
            csv_data += ','
            # 借方補助科目CD
            xls_KarikataHozyokamokuCD = SpcToNull(ws.cell_value(rowx=row_index, colx=5))
            if xls_KarikataHozyokamokuCD == None:
                xls_KarikataHozyokamokuCD = ""
            else:
                xls_KarikataHozyokamokuCD = xls_KarikataHozyokamokuCD.split('.')[0]
            csv_data = csv_data + xls_KarikataHozyokamokuCD + ','
            # 借方補助科目名
            xls_KarikataHozyokamokumei = SpcToNull(ws.cell_value(rowx=row_index, colx=9))
            if xls_KarikataHozyokamokumei == None:
                csv_data += ','
            else:
                csv_data = csv_data + '"' + xls_KarikataHozyokamokumei + '"' + ','
            # 借方部門CD
            xls_KarikataBumonCD = None
            csv_data += ','
            # 借方部門名
            xls_KarikataBumonmei = None
            csv_data += ','

            # 貸方勘定科目CD
            xls_KashikataCD = str(int(ws.cell_value(rowx=row_index, colx=10))).strip()
            csv_data = csv_data + xls_KashikataCD + ','
            # 貸方勘定科目名
            xls_KashikataKamokumei = str(ws.cell_value(rowx=row_index, colx=14)).strip()
            csv_data = csv_data + '"' + xls_KashikataKamokumei + '"' + ','
            # 貸方消費税CD
            xls_KashikataSyohizeiCD = SpcToNull(ws.cell_value(rowx=row_index, colx=12))
            if xls_KashikataSyohizeiCD == None:
                csv_data += ','
            else:
                xls_KashikataSyohizeiCD = xls_KashikataSyohizeiCD.split('.')[0]
                csv_data = csv_data + xls_KashikataSyohizeiCD + ','
            # 貸方消費税率
            xls_KashikataSyohizeiritsu = SpcToNull(ws.cell_value(rowx=row_index, colx=13))
            if xls_KashikataSyohizeiritsu == None:
                csv_data += ','
            else:
                xls_KashikataSyohizeiritsu = xls_KashikataSyohizeiritsu.split('.')[0]
                csv_data = csv_data + xls_KashikataSyohizeiritsu + ','
            # 貸方業種
            xls_KashikataGyosyu = None
            csv_data += ','
            # 貸方補助科目CD
            xls_KashikataHozyokamokuCD = SpcToNull(ws.cell_value(rowx=row_index, colx=11))
            if xls_KashikataHozyokamokuCD == None:
                csv_data += ','
            else:
                xls_KashikataHozyokamokuCD = xls_KashikataHozyokamokuCD.split('.')[0]
                csv_data = csv_data + xls_KashikataHozyokamokuCD + ','
            # 貸方補助科目名
            xls_KashikataHozyokamokumei = str(ws.cell_value(rowx=row_index, colx=15)).strip()
            csv_data = csv_data + '"' + xls_KashikataHozyokamokumei + '"' + ','
            # 貸方部門CD
            xls_KashikataBumonCD = None
            csv_data += ','
            # 貸方部門名
            xls_KashikataBumonmei = None
            csv_data += ','

            # 貸方金額
            xls_KashikataKingaku = str(int(ws.cell_value(rowx=row_index, colx=17))).strip()
            csv_data = csv_data + xls_KashikataKingaku + ','

            # 摘要
            xls_tekiyo = str(ws.cell_value(rowx=row_index, colx=18)).strip()
            csv_data = csv_data + '"' + xls_tekiyo + '"' + ','
            # 期日_年
            xls_KijitsuYear = None
            csv_data += ','
            # 期日_月
            xls_KijitsuMonth = None
            csv_data += ','
            # 期日_日
            xls_KijitsuDay = None
            csv_data += ','
            # 手形番号
            xls_TegataBango = None

            csv_data += '\r\n'


            # 取込リストに追加
            gRsA_dic = gRsA_tempdic.copy()
            gRsA_dic['年'] = xls_year
            gRsA_dic['月'] = xls_month
            gRsA_dic['日'] = xls_day
            gRsA_dic['借方勘定科目CD'] = xls_KarikataCD
            gRsA_dic['借方補助科目CD'] = xls_KarikataHozyokamokuCD
            gRsA_dic['借方消費税CD'] = xls_KarikataSyohizeiCD
            gRsA_dic['借方消費税率'] = xls_KarikataSyohizeiritsu
            gRsA_dic['借方勘定科目名'] = xls_KarikataKamokumei
            gRsA_dic['借方補助科目名'] = xls_KarikataHozyokamokumei
            gRsA_dic['貸方勘定科目CD'] = xls_KashikataCD
            gRsA_dic['貸方補助科目CD'] = xls_KashikataHozyokamokuCD
            gRsA_dic['貸方消費税CD'] = xls_KashikataSyohizeiCD
            gRsA_dic['貸方消費税率'] = xls_KashikataSyohizeiritsu
            gRsA_dic['貸方勘定科目名'] = xls_KashikataKamokumei
            gRsA_dic['貸方補助科目名'] = xls_KashikataHozyokamokumei
            gRsA_dic['借方金額'] = xls_KarikataKingaku
            gRsA_dic['貸方金額'] = xls_KashikataKingaku
            gRsA_dic['摘要'] = xls_tekiyo

            gRsA.append(gRsA_dic)


        enc_csv_data = csv_data.encode('cp932')
        # csv_data = csv_data.encode('shift-jis')

        if request.params['kubun'] == "CSV出力":
            if request.params['output'] == "しない":
                return StreamingResponse(
                    io.BytesIO(enc_csv_data),
                    media_type="text/csv; charset=Shift-JIS",
                    headers={
                        'Content-Disposition': f'attachment; filename="{encode_string("仕訳.txt")}"'
                    }
                )
            else:
                zip_buffer = io.BytesIO()
                with zipfile.ZipFile(zip_buffer, "a") as zf:
                    zf.writestr("仕訳.txt", enc_csv_data)

        
        # 印刷する
        if request.params['output'] == "する":

            with get_excel_buffer() as buffer:

                start_row = 4

                row_num = start_row

                wb = create_excel_object('Template_EXCEL仕訳TXTチェックリスト.xlsx')
                
                ws = wb.active

                # 作成日
                ws['O1'] = "作成日：{}".format(format_jp_date())

                for row_data in gRsA:
                    ws.cell(row=row_num, column=1, value=row_data['年'])._style = copy(ws.cell(row=start_row, column=1)._style)
                    ws.cell(row=row_num, column=2, value=row_data['月'])._style = copy(ws.cell(row=start_row, column=2)._style)
                    ws.cell(row=row_num, column=3, value=row_data['日'])._style = copy(ws.cell(row=start_row, column=3)._style)
                    ws.cell(row=row_num, column=4, value=row_data['借方勘定科目CD'])._style = copy(ws.cell(row=start_row, column=4)._style)
                    ws.cell(row=row_num, column=5, value=row_data['借方補助科目CD'])._style = copy(ws.cell(row=start_row, column=5)._style)
                    ws.cell(row=row_num, column=6, value=row_data['借方消費税CD'])._style = copy(ws.cell(row=start_row, column=6)._style)
                    ws.cell(row=row_num, column=7, value=row_data['借方消費税率'])._style = copy(ws.cell(row=start_row, column=7)._style)
                    ws.cell(row=row_num, column=8, value=row_data['借方勘定科目名'])._style = copy(ws.cell(row=start_row, column=8)._style)
                    ws.cell(row=row_num, column=9, value=row_data['借方補助科目名'])._style = copy(ws.cell(row=start_row, column=9)._style)
                    ws.cell(row=row_num, column=10, value=row_data['貸方勘定科目CD'])._style = copy(ws.cell(row=start_row, column=10)._style)
                    ws.cell(row=row_num, column=11, value=row_data['貸方補助科目CD'])._style = copy(ws.cell(row=start_row, column=11)._style)
                    ws.cell(row=row_num, column=12, value=row_data['貸方消費税CD'])._style = copy(ws.cell(row=start_row, column=12)._style)
                    ws.cell(row=row_num, column=13, value=row_data['貸方消費税率'])._style = copy(ws.cell(row=start_row, column=13)._style)
                    ws.cell(row=row_num, column=14, value=row_data['貸方勘定科目名'])._style = copy(ws.cell(row=start_row, column=14)._style)
                    ws.cell(row=row_num, column=15, value=row_data['貸方補助科目名'])._style = copy(ws.cell(row=start_row, column=15)._style)
                    ws.cell(row=row_num, column=16, value=row_data['借方金額'])._style = copy(ws.cell(row=start_row, column=16)._style)
                    ws.cell(row=row_num, column=17, value=row_data['貸方金額'])._style = copy(ws.cell(row=start_row, column=17)._style)
                    ws.cell(row=row_num, column=18, value=row_data['摘要'])._style = copy(ws.cell(row=start_row, column=18)._style)

                    # 高さ調整
                    ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height

                    row_num += 1

                ws.title = "経費データ取込リスト"

                # 改ページプレビューの設定
                ws.sheet_view.view = 'pageBreakPreview'
                ws.sheet_view.zoomScaleSheetLayoutView= 100

                # Excelオブジェクトの保存
                save_excel_to_buffer(buffer, wb, None)

                # PDFファイルの返却
                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="R{}".format(row_num-1)
                )

                pdf_content = converter.generate_pdf()

                if request.params['kubun'] == "チェックのみ":
                
                    return StreamingResponse(
                        io.BytesIO(pdf_content),
                        media_type="application/pdf",
                        headers={
                            "Content-Disposition": f'attachment; filename="{encode_string("EXCEL取込リスト.pdf")}"'
                        }
                    )
                
                else:
                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("EXCEL取込リスト.pdf", pdf_content)

        if request.params['kubun'] == "CSV出力" and request.params['output'] == "する":

            zip_buffer.seek(0)

            # 取込リストとエラーリストをzipで出力
            filename = "EXCEL取込結果.zip"
            return StreamingResponse(
                zip_buffer,
                media_type="application/x-zip-compressed",
                headers={
                    "Content-Disposition": f'attachment; filename="{encode_string(filename)}"'
                }
            )
        
        
        return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes",
                    'Success-Message-Code': 'SUCCESS_CHECK'
                }
            )

                    
                    
                    
                    
                    
                    


