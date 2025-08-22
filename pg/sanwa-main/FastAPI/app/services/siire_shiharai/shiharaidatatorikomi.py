import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from app.services.base_service import BaseService
from app.utils.excel_utils import (
    get_excel_buffer,
    create_excel_object,
    save_excel_to_buffer,
    ExcelPDFConverter,
    range_copy_cell
)
from app.utils.string_utils import encode_string
from app.core.config import settings
from copy import copy
from app.utils.service_utils import ClsShiiresaki, Snw_cm
import zipfile
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class shiharaidatatorikomiService(BaseService):
    """支払データ取込サービス"""

    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        # 支払データの取込処理
        logger.info("【START】支払データ取込処理")

        # CreataTable
        # 保存用リストの作成
        gRsA = []
        gRsA_tempdic = {
            "支払日付": None,
            "仕入先CD": None,
            "仕入先名1": None,
            "仕入先名2": None,
            "支払区分CD": None,
            "支払区分名": None,
            "支払種別": None,
            "支払金額": None,
            "摘要名": None,
            "合計金額": None,
            "ERR区分": None
        }

        error = False
        output = request.params['output'] == "する"

        # ImportFixdTextToRs
        # CSVの取込方式が変わったことでEOFが取り込まれることはないためコメントアウト
        # for csvData in request.params['csvData']:
        #     if csvData[0] != chr(0x1A) and csvData[0] != "":
        #         pass
        #     else:
        #         #EOFは削除
        #         del csvData
        for csvData in request.params['csvData']:
            errKBN = 0
            gRsA_dic = gRsA_tempdic.copy()
            # 仕入先CDが空だった場合エラー
            if not csvData[0]:
                errKBN = 1
                error = True
            else:
                shiiresaki_data = ClsShiiresaki.GetbyID(session, csvData[0])
                # 存在しない仕入先CDだった場合エラー
                if len(shiiresaki_data) < 1:
                    errKBN = 1
                    error = True
                else:
                    # 支払日付が空だった場合エラー
                    if not csvData[1]:
                        errKBN = 1
                        error = True

                    # 支払日付が存在しない日付だった場合エラー
                    try:
                        datetime.datetime.strptime(csvData[1], "%Y/%m/%d")
                    except (ValueError, IndexError):
                        # パースに失敗、または日付が存在しない場合、errKBNを1に設定
                        errKBN = 1
                        error = True

                    # 支払金額が空エラー
                    if not csvData[2]:
                        errKBN = 1
                        error = True
                    else:
                        # または0未満の場合
                        if int(csvData[2]) < 0:
                            errKBN = 1
                            error = True

                    gRsA_dic['仕入先名1'] = shiiresaki_data[0]['仕入先名1']
                    gRsA_dic['仕入先名2'] = shiiresaki_data[0]['仕入先名2']
                    gRsA_dic['摘要名'] = shiiresaki_data[0]['略称']

                    gRsA_dic['支払区分CD'] = 1
                    gRsA_dic['支払区分名'] = "銀行振込"
                    gRsA_dic['支払種別'] = 1
                    gRsA_dic['合計金額'] = csvData[2]

            gRsA_dic['仕入先CD'] = csvData[0]
            gRsA_dic['支払日付'] = csvData[1]
            gRsA_dic['支払金額'] = csvData[2]
            gRsA_dic['ERR区分'] = errKBN
            gRsA.append(gRsA_dic)

        zip_buffer = io.BytesIO()

        #処理区分が「取込」の場合、DB登録処理を実施
        if request.params['kubun'] == "取込":
            # UpdateServer
            # 支払番号取得
            for insert_data in gRsA:
                if insert_data['ERR区分'] == 0:
                    cls_Snw_cm = Snw_cm
                    GetCounter_result = cls_Snw_cm.GetCounter(self,session=session,sItemName="支払番号")
                    Number = GetCounter_result['output_values']['@GetNO']
                    query = "INSERT INTO TD支払 \
                        (支払番号, 枝番, 支払日付, 仕入先CD, 仕入先名1, 仕入先名2, 支払区分CD, 支払区分名, 支払種別, 支払金額, 摘要名, 合計金額, 初期登録日, 登録変更日) \
                        VALUES ({}, {}, '{}', '{}', '{}', '{}', {}, '{}', {}, {}, '{}', {}, '{}', '{}')".format(
                            Number,
                            1,
                            insert_data['支払日付'],
                            insert_data['仕入先CD'],
                            insert_data['仕入先名1'],
                            insert_data['仕入先名2'],
                            insert_data['支払区分CD'],
                            insert_data['支払区分名'],
                            insert_data['支払種別'],
                            insert_data['支払金額'],
                            insert_data['摘要名'],
                            insert_data['合計金額'],
                            datetime.date.today(),
                            datetime.date.today()
                        )
                    SQLExecutor(session).execute_query(query=query)

        # 「リスト出力する」の場合、取込リストを出力
        if output:
            # 取込リスト
            with get_excel_buffer() as torikomi_buffer:

                wb = create_excel_object('Temp支払取込リスト.xlsx')

                ws = wb.active

                ws['H1'] = "作成日：{}".format(datetime.date.today().strftime('%Y/%m/%d'))

                start_row = 5

                row_num = start_row

                for row_data in gRsA:
                    if row_data['ERR区分'] == 0:
                        # 日付
                        ws.cell(row=row_num, column=1, value=row_data['支払日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        ws.cell(row=row_num, column=2, value=row_data['仕入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['仕入先名1'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['仕入先名2'])._style = copy(ws.cell(row=start_row, column=4)._style)
                        ws.cell(row=row_num, column=5, value=row_data['支払区分CD'])._style = copy(ws.cell(row=start_row, column=5)._style)
                        ws.cell(row=row_num, column=6, value=row_data['支払区分名'])._style = copy(ws.cell(row=start_row, column=6)._style)
                        ws.cell(row=row_num, column=7, value=row_data['支払金額'])._style = copy(ws.cell(row=start_row, column=7)._style)
                        ws.cell(row=row_num, column=8, value=row_data['摘要名'])._style = copy(ws.cell(row=start_row, column=8)._style)

                        # 高さ調整
                        ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                        row_num += 1

                ws.title = "支払取込リスト"

                # 合計
                ws['G4'] = "=SUM(G5:G{})".format(row_num-1)

                # 改ページプレビューの設定
                ws.sheet_view.view = 'pageBreakPreview'
                ws.sheet_view.zoomScaleSheetLayoutView= 100

                # Excelオブジェクトの保存
                save_excel_to_buffer(torikomi_buffer, wb, None)

                # エラーリストも取込リストも出力する場合、zipbufferに追加
                filename = "支払取込リスト.xlsx"
                if error:
                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("ImportList.xlsx", torikomi_buffer.getvalue())
                # エラーリストは出力しない場合、取込リストのみ出力
                else:
                    return StreamingResponse(
                        io.BytesIO(torikomi_buffer.getvalue()),
                        media_type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        headers={
                            'Content-Disposition': f'attachment; filename="{encode_string(filename)}"'
                        }
                    )

        # エラーがあった場合
        if error:
            with get_excel_buffer() as error_buffer:

                wb = create_excel_object('Temp支払取込エラーリスト.xlsx')

                ws = wb.active

                ws['C1'] = "作成日：{}".format(datetime.date.today().strftime('%Y/%m/%d'))

                start_row = 4

                row_num = start_row

                for row_data in gRsA:
                    if row_data['ERR区分'] == 1:
                        # 日付
                        ws.cell(row=row_num, column=1, value=row_data['支払日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                        ws.cell(row=row_num, column=2, value=row_data['仕入先CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                        ws.cell(row=row_num, column=3, value=row_data['支払金額'])._style = copy(ws.cell(row=start_row, column=3)._style)
                        ws.cell(row=row_num, column=4, value=row_data['摘要名'])._style = copy(ws.cell(row=start_row, column=4)._style)

                        # 高さ調整
                        ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                        row_num += 1

                ws.title = "支払取込エラーリスト"

                # 改ページプレビューの設定
                ws.sheet_view.view = 'pageBreakPreview'
                ws.sheet_view.zoomScaleSheetLayoutView= 100

                # # Excelオブジェクトの保存
                # save_excel_to_buffer(error_buffer, wb, None)

                converter = ExcelPDFConverter(
                    wb=wb,
                    ws=ws,
                    start_cell="A1",
                    end_cell="O{}".format(row_num-1)
                )

                pdf_content = converter.generate_pdf()

                # 取込リストを出力していない場合
                if not output:
                    # PDFファイルの返却
                    filename = "支払取込エラーリスト.pdf"
                    return StreamingResponse(
                        io.BytesIO(pdf_content),
                        media_type="application/pdf",
                        headers={
                            "Content-Disposition": f'attachment; filename="{encode_string(filename)}"'
                        }
                    )

                # 取込リストを出力していた場合、エラーリストをzipbufferに追加
                filename = "支払取込エラーリスト.pdf"
                with zipfile.ZipFile(zip_buffer, "a") as zf:
                    zf.writestr("ErrorList.pdf", pdf_content)

                zip_buffer.seek(0)

                # 取込リストとエラーリストをzipで出力
                filename = "支払データ取込結果.zip"
                return StreamingResponse(
                    zip_buffer,
                    media_type="application/x-zip-compressed",
                    headers={
                        "Content-Disposition": f'attachment; filename="{encode_string(filename)}"'
                    }
                )

        # 取込リストもエラーリストを出力しない場合
        if not output and not error:
            return JSONResponse(
                content={},
                headers={
                    'Download-Skip': "yes"
                }
            )



    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass
