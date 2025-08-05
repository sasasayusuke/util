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
    ExcelPDFConverter
)
from app.utils.string_utils import encode_string, AnsiLeftB, SpcToNull, NullToZero, format_jp_date
from app.core.config import settings
from copy import copy
from app.utils.service_utils import Snw_cm, ClsKamoku, ClsConvTanto, ClsTanto
import zipfile
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class keihitorikomiService(BaseService):
    """経費取込サービス"""

    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        # 経費データの取込処理
        logger.info("【START】経費データ取込処理")

        # CreataTable
        # 保存用リストの作成
        gRsA = []
        gRsA_tempdic = {
            "経費番号": None,
            "経費日付": None,
            "科目CD": None,
            "科目名": None,
            "金額": None,
            "担当者CD": None,
            "担当者名": None,
            "科目摘要名": None,
            "消費税額": None,
            "補助CD": None,
            "ERR区分": None
        }

        output = request.params['output'] == "する"

        errflg = False
        torikomiflg = False

        testcounter = 1

        # 最終行('\x1a')を削除
        request.params['csvData'] = [csvData for csvData in request.params['csvData'] if csvData[0] != '\x1a']

        for csvData in request.params['csvData']:

            gRsA_dic = gRsA_tempdic.copy()

            # 担当者変換
            cConvTanto = ClsConvTanto(csvData[10])
            cConvTanto.ConvertByID(session)
            # 担当者名取得
            errKBN = 0
            TantoResult = []
            if cConvTanto.m_積算担当者CD != "":
                cTanto = ClsTanto()
                TantoResult = cTanto.GetbyID(session,cConvTanto.m_積算担当者CD)
                if len(TantoResult) == 0:
                    errKBN = 1
                    errflg = True
                else:
                    errKBN = 0
            # 借方科目名取得
            cKamoku = ClsKamoku(csvData[5])

            if cKamoku.GetbyID(session):

                # エラーチェック
                # 支払日付が存在しない日付だった場合エラー
                # try:
                #     datetime.datetime.strptime("{}/{}/{}".format(csvData[0],csvData[1],csvData[2]), "%Y/%m/%d")
                # except (ValueError, IndexError):
                #     # パースに失敗、または日付が存在しない場合、errKBNを1に設定
                #     errKBN = 1

                # # 金額が空の場合
                # if not csvData[4]:
                #     errKBN = 1

                # # または0未満の場合
                # if csvData[4] < 0:
                #     errKBN = 1

                gRsA_dic['経費日付'] = "{}/{}/{}".format(csvData[0],csvData[1],csvData[2])
                gRsA_dic['科目CD'] = csvData[5]
                gRsA_dic['科目名'] = cKamoku.科目名
                gRsA_dic['担当者CD'] = cConvTanto.m_積算担当者CD if cConvTanto.m_積算担当者CD else ""
                gRsA_dic['担当者名'] = TantoResult[0].担当者名 if len(TantoResult) > 0 else ""
                gRsA_dic['科目摘要名'] = AnsiLeftB(SpcToNull(csvData[24], ""), 30).replace('"','')
                gRsA_dic['消費税額'] = 0
                gRsA_dic['金額'] = NullToZero(csvData[4])
                gRsA_dic['補助CD'] = SpcToNull(csvData[10],0)
                gRsA_dic['ERR区分'] = errKBN

                # デバッグログ：借方データ
                if csvData[5] in ['527']:  # 租税公課のみ（受取利息、受取配当金は貸方に出現）
                    logger.debug(f"[借方] 科目CD: {csvData[5]}, 科目名: {cKamoku.科目名}, 金額: {csvData[4]}, 補助CD(csv[10]): {csvData[10]}, 変換後補助CD: {gRsA_dic['補助CD']}")

                gRsA.append(gRsA_dic)

            # 貸方科目名取得
            cKamoku2 = ClsKamoku(csvData[14])
            if cKamoku2.GetbyID(session):

                # エラーチェック
                # 支払日付が存在しない日付だった場合エラー
                # try:
                #     datetime.datetime.strptime("{}/{}/{}".format(csvData[0],csvData[1],csvData[2]), "%Y/%m/%d")
                # except (ValueError, IndexError):
                #     # パースに失敗、または日付が存在しない場合、errKBNを1に設定
                #     errKBN = 1

                # # 金額が空の場合
                # if not csvData[23]:
                #     errKBN = 1

                # # または0未満の場合
                # if csvData[23] < 0:
                #     errKBN = 1

                gRsA_dic['経費日付'] = "{}/{}/{}".format(csvData[0],csvData[1],csvData[2])
                gRsA_dic['科目CD'] = csvData[14]
                gRsA_dic['科目名'] = cKamoku2.科目名
                gRsA_dic['担当者CD'] = cConvTanto.m_積算担当者CD if cConvTanto.m_積算担当者CD else ""
                gRsA_dic['担当者名'] = TantoResult[0].担当者名 if len(TantoResult) > 0 else ""
                gRsA_dic['科目摘要名'] = AnsiLeftB(SpcToNull(csvData[24], ""), 30).replace('"','')
                gRsA_dic['消費税額'] = 0
                if errKBN != 1:
                    gRsA_dic['金額'] = NullToZero(csvData[23]) * -1
                else:
                    gRsA_dic['金額'] = NullToZero(csvData[23])
                gRsA_dic['補助CD'] = SpcToNull(csvData[19],0)
                gRsA_dic['ERR区分'] = errKBN

                # デバッグログ：貸方データ
                if csvData[14] in ['600', '601']:  # 受取利息、受取配当金（貸方に出現）
                    logger.debug(f"[貸方] 科目CD: {csvData[14]}, 科目名: {cKamoku2.科目名}, 金額: {csvData[23]}, 補助CD(csv[19]): {csvData[19]}, 変換後補助CD: {gRsA_dic['補助CD']}")

                gRsA.append(gRsA_dic)

            testcounter += 1

        zip_buffer = io.BytesIO()

        #データ取込の場合
        if request.params['kubun'] == "取込":
            # UpdateServer
            # 支払番号取得
            Hdate = None
            Mcnt = 0
            for insert_data in gRsA:
                if insert_data['ERR区分'] == 0:
                    if Hdate != insert_data['経費日付'] or Mcnt >= 99:

                        Hdate = insert_data['経費日付']

                        cls_Snw_cm = Snw_cm
                        GetCounter_result = cls_Snw_cm.GetCounter(self,session=session,sItemName="経費番号")
                        Number = GetCounter_result['output_values']['@GetNO']

                        # ヘッダー部作成
                        query = "INSERT INTO TD経費 \
                            (経費番号, 経費日付, 初期登録日, 登録変更日) \
                            VALUES ({}, '{}', '{}', '{}')".format(
                                Number,
                                insert_data['経費日付'],
                                datetime.date.today(),
                                datetime.date.today()
                            )
                        SQLExecutor(session).execute_query(query=query)

                        # 明細番号初期化
                        Mcnt = 0

                    Mcnt = Mcnt + 1

                    # デバッグログ：INSERT前のデータ確認
                    if insert_data['科目CD'] in ['527', '600', '601']:  # 租税公課、受取利息、受取配当金
                        logger.debug(f"[INSERT] 経費番号: {Number}, 枝番: {Mcnt}, 科目CD: {insert_data['科目CD']}, 科目名: {insert_data['科目名']}, 金額: {insert_data['金額']}, 補助CD: {insert_data['補助CD']}")

                    # 明細部作成
                    query = "INSERT INTO TD経費明細 \
                        (経費番号, 枝番, 科目CD, 科目名, 金額, 担当者CD, 担当者名, 購入先名, 科目摘要cd, 科目摘要名, 初期登録日, 登録変更日, 消費税額, 補助CD) \
                        VALUES ({}, {}, {}, '{}', {}, {}, '{}', '{}', null, '{}', '{}', '{}', {}, {})".format(
                            Number,
                            Mcnt,
                            insert_data['科目CD'],
                            insert_data['科目名'],
                            insert_data['金額'],
                            f"'{insert_data['担当者CD']}'" if insert_data.get('担当者CD') else "null",
                            insert_data['担当者名'],
                            "",
                            insert_data['科目摘要名'],
                            datetime.date.today(),
                            datetime.date.today(),
                            insert_data['消費税額'],
                            insert_data['補助CD']
                        )
                    SQLExecutor(session).execute_query(query=query)


            # 出力かつ取込があった場合
            if output == True:

                # 取込リスト
                with get_excel_buffer() as torikomi_buffer:

                    wb = create_excel_object('Temp経費データ取込リスト.xlsx')

                    ws = wb.active

                    ws['F1'] = "作成日：{}".format(format_jp_date())

                    start_row = 4

                    row_num = start_row

                    for row_data in gRsA:
                        if row_data['ERR区分'] == 0:
                            torikomiflg = True
                            # 日付
                            ws.cell(row=row_num, column=1, value=row_data['経費日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                            ws.cell(row=row_num, column=2, value=row_data['科目CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                            if row_data['補助CD'] == 0:
                                hozyoCD = ""
                            else:
                                hozyoCD = row_data['補助CD']
                            ws.cell(row=row_num, column=3, value=hozyoCD)._style = copy(ws.cell(row=start_row, column=3)._style)
                            ws.cell(row=row_num, column=4, value=row_data['科目名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                            ws.cell(row=row_num, column=5, value=row_data['金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                            ws.cell(row=row_num, column=6, value="")._style = copy(ws.cell(row=start_row, column=6)._style)
                            ws.cell(row=row_num, column=7, value=cConvTanto.m_積算担当者CD)._style = copy(ws.cell(row=start_row, column=7)._style)
                            ws.cell(row=row_num, column=8, value=row_data['科目摘要名'])._style = copy(ws.cell(row=start_row, column=8)._style)

                            # 高さ調整
                            ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                            row_num += 1

                    ws.title = "経費データ取込リスト"

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                    # Excelオブジェクトの保存
                    save_excel_to_buffer(torikomi_buffer, wb, None)

                    # PDFファイルの返却
                    converter = ExcelPDFConverter(
                        wb=wb,
                        ws=ws,
                        start_cell="A1",
                        end_cell="H{}".format(row_num-1)
                    )

                    pdf_content = converter.generate_pdf()

                    filename = "経費データ取込リスト.pdf"

                    # エラーがなかったら取込リストのみ出力
                    if torikomiflg == True and errflg == False:
                        return StreamingResponse(
                            io.BytesIO(pdf_content),
                            media_type="application/pdf",
                            headers={
                                "Content-Disposition": f'attachment; filename="{encode_string(filename)}"',
                            }
                        )

                    # エラーリストも取込リストも出力する場合、zipbufferに追加
                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("ImportList.pdf", pdf_content)


                with get_excel_buffer() as error_buffer:

                    wb = create_excel_object('Temp経費データ取込エラーリスト.xlsx')

                    ws = wb.active

                    ws['D1'] = "作成日：{}".format(format_jp_date())

                    start_row = 4

                    row_num = start_row

                    for row_data in gRsA:
                        if row_data['ERR区分'] == 1:
                            # 日付
                            ws.cell(row=row_num, column=1, value=row_data['経費日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                            ws.cell(row=row_num, column=2, value=row_data['科目CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                            if row_data['補助CD'] == 0:
                                hozyoCD = ""
                            else:
                                hozyoCD = row_data['補助CD']
                            ws.cell(row=row_num, column=3, value=hozyoCD)._style = copy(ws.cell(row=start_row, column=3)._style)
                            ws.cell(row=row_num, column=4, value=row_data['金額'])._style = copy(ws.cell(row=start_row, column=4)._style)
                            ws.cell(row=row_num, column=5, value="")._style = copy(ws.cell(row=start_row, column=5)._style)
                            ws.cell(row=row_num, column=6, value=row_data['科目摘要名'])._style = copy(ws.cell(row=start_row, column=6)._style)

                            # 高さ調整
                            ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                            row_num += 1

                    ws.title = "経費データ取込エラーリスト"

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                    # Excelオブジェクトの保存
                    save_excel_to_buffer(error_buffer, wb, None)

                    # # PDFファイルの返却
                    converter = ExcelPDFConverter(
                        wb=wb,
                        ws=ws,
                        start_cell="A1",
                        end_cell="F{}".format(row_num-1)
                    )

                    pdf_content = converter.generate_pdf()

                    filename = "経費データ取込エラーリスト.pdf"

                    # エラーリストのみ出力
                    if torikomiflg == False and errflg == True:
                        return StreamingResponse(
                            io.BytesIO(pdf_content),
                            media_type="application/pdf",
                            headers={
                                "Content-Disposition": f'attachment; filename="{encode_string(filename)}"',
                            }
                        )

                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("ErrorList.pdf", pdf_content)
                
                zip_buffer.seek(0)

                if torikomiflg == False and errflg == False:
                    return JSONResponse(
                        content={},
                        headers={
                            'Download-Skip': "yes",
                            'Success-Message-Code': 'NOT_FOUND'
                        }
                    )
            

                # 取込リストとエラーリストをzipで出力
                filename = "経費データ取込結果.zip"
                return StreamingResponse(
                    zip_buffer,
                    media_type="application/x-zip-compressed",
                    headers={
                        "Content-Disposition": f'attachment; filename="{encode_string(filename)}"'
                    }
                )
                

            # リスト出力しない場合
            else:
                return JSONResponse(
                        content={},
                        headers={
                            'Download-Skip': "yes"
                        }
                    )
            

        else:
            # チェックの場合
            logger.info("チェックが完了しました")

            if output:

                # 取込リスト
                with get_excel_buffer() as torikomi_buffer:

                    wb = create_excel_object('Temp経費データ取込リスト.xlsx')

                    ws = wb.active

                    ws['F1'] = "作成日：{}".format(format_jp_date())

                    start_row = 4

                    row_num = start_row

                    for row_data in gRsA:
                        if row_data['ERR区分'] == 0:
                            torikomiflg = True
                            # 日付
                            ws.cell(row=row_num, column=1, value=row_data['経費日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                            ws.cell(row=row_num, column=2, value=row_data['科目CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                            if row_data['補助CD'] == 0:
                                hozyoCD = ""
                            else:
                                hozyoCD = row_data['補助CD']
                            ws.cell(row=row_num, column=3, value=hozyoCD)._style = copy(ws.cell(row=start_row, column=3)._style)
                            ws.cell(row=row_num, column=4, value=row_data['科目名'])._style = copy(ws.cell(row=start_row, column=4)._style)
                            ws.cell(row=row_num, column=5, value=row_data['金額'])._style = copy(ws.cell(row=start_row, column=5)._style)
                            ws.cell(row=row_num, column=6, value="")._style = copy(ws.cell(row=start_row, column=6)._style)
                            ws.cell(row=row_num, column=7, value=cConvTanto.m_積算担当者CD)._style = copy(ws.cell(row=start_row, column=7)._style)
                            ws.cell(row=row_num, column=8, value=row_data['科目摘要名'])._style = copy(ws.cell(row=start_row, column=8)._style)

                            # 高さ調整
                            ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                            row_num += 1

                    ws.title = "経費データ取込リスト"

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                    # Excelオブジェクトの保存
                    save_excel_to_buffer(torikomi_buffer, wb, None)

                    # PDFファイルの返却
                    converter = ExcelPDFConverter(
                        wb=wb,
                        ws=ws,
                        start_cell="A1",
                        end_cell="H{}".format(row_num-1)
                    )

                    pdf_content = converter.generate_pdf()

                    filename = "経費データ取込リスト.pdf"

                    # エラーがなかったら取込リストのみ出力
                    if torikomiflg == True and errflg == False:
                        return StreamingResponse(
                            io.BytesIO(pdf_content),
                            media_type="application/pdf",
                            headers={
                                "Content-Disposition": f'attachment; filename="{encode_string(filename)}"',
                            }
                        )

                    # エラーリストも取込リストも出力する場合、zipbufferに追加
                    filename = "経費データ取込リスト.pdf"
                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("ImportList.pdf", pdf_content)


                with get_excel_buffer() as error_buffer:

                    wb = create_excel_object('Temp経費データ取込エラーリスト.xlsx')

                    ws = wb.active

                    ws['D1'] = "作成日：{}".format(format_jp_date())

                    start_row = 4

                    row_num = start_row

                    for row_data in gRsA:
                        if row_data['ERR区分'] == 1:
                            # 日付
                            ws.cell(row=row_num, column=1, value=row_data['経費日付'])._style = copy(ws.cell(row=start_row, column=1)._style)
                            ws.cell(row=row_num, column=2, value=row_data['科目CD'])._style = copy(ws.cell(row=start_row, column=2)._style)
                            if row_data['補助CD'] == 0:
                                hozyoCD = ""
                            else:
                                hozyoCD = row_data['補助CD']
                            ws.cell(row=row_num, column=3, value=hozyoCD)._style = copy(ws.cell(row=start_row, column=3)._style)
                            ws.cell(row=row_num, column=4, value=row_data['金額'])._style = copy(ws.cell(row=start_row, column=4)._style)
                            ws.cell(row=row_num, column=5, value="")._style = copy(ws.cell(row=start_row, column=5)._style)
                            ws.cell(row=row_num, column=6, value=row_data['科目摘要名'])._style = copy(ws.cell(row=start_row, column=6)._style)

                            # 高さ調整
                            ws.row_dimensions[row_num].height = ws.row_dimensions[start_row].height
                            row_num += 1

                    ws.title = "経費データエラー取込リスト"

                    # 改ページプレビューの設定
                    ws.sheet_view.view = 'pageBreakPreview'
                    ws.sheet_view.zoomScaleSheetLayoutView= 100

                    # Excelオブジェクトの保存
                    save_excel_to_buffer(error_buffer, wb, None)

                    # # PDFファイルの返却
                    converter = ExcelPDFConverter(
                        wb=wb,
                        ws=ws,
                        start_cell="A1",
                        end_cell="F{}".format(row_num-1)
                    )

                    pdf_content = converter.generate_pdf()

                    filename = "経費データ取込エラーリスト.pdf"

                    # エラーリストのみ出力
                    if torikomiflg == False and errflg == True:
                        return StreamingResponse(
                            io.BytesIO(pdf_content),
                            media_type="application/pdf",
                            headers={
                                "Content-Disposition": f'attachment; filename="{encode_string(filename)}"',
                            }
                        )

                    with zipfile.ZipFile(zip_buffer, "a") as zf:
                        zf.writestr("ErrorList.pdf", pdf_content)
                
                zip_buffer.seek(0)

                if torikomiflg == False and errflg == False:
                    return JSONResponse(
                        content={},
                        headers={
                            'Download-Skip': "yes",
                            'Success-Message-Code': 'SUCCESS_CHECK'
                        }
                    )

                # 取込リストとエラーリストをzipで出力
                filename = "経費データ取込結果.zip"
                return StreamingResponse(
                    zip_buffer,
                    media_type="application/x-zip-compressed",
                    headers={
                        "Content-Disposition": f'attachment; filename="{encode_string(filename)}"',
                        'Success-Message-Code': 'SUCCESS_CHECK'
                    }
                )
                

            # リスト出力しない場合
            else:
                return JSONResponse(
                        content={},
                        headers={
                            'Download-Skip': "yes",
                            'Success-Message-Code': 'SUCCESS_CHECK'
                        }
                    )


    def delete(self, request, session) -> Dict[str, Any]:
        """経費データの削除処理を行うメソッド"""
        # 経費データの削除処理
        logger.info("【START】経費データ削除処理")

        is_date = request.params['@is日付']
        ie_date = request.params['@ie日付']
    
        keihimeisai_query = "DELETE	FROM TD経費明細 FROM TD経費 INNER JOIN TD経費明細 ON TD経費.経費番号 = TD経費明細.経費番号 WHERE TD経費.経費日付 >= '{}' AND TD経費.経費日付 <= '{}'".format(is_date, ie_date)
        Keihi_query = "DELETE FROM TD経費 WHERE	TD経費.経費日付 >= '{}' AND TD経費.経費日付 <= '{}'".format(is_date, ie_date)


        SQLExecutor(session).execute_query(query=keihimeisai_query)
        SQLExecutor(session).execute_query(query=Keihi_query)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes",
            }
        )

    def getLastInfo(self, request, session) -> Dict[str, Any]:
        """前回出力情報の取得を行うメソッド"""
        pass
