import io
import logging
from fastapi import APIRouter, HTTPException, Query
from fastapi.responses import StreamingResponse
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer
from app.core.config import settings


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

router = APIRouter()

@router.get("/test-excel")
def test_excel(
    template_file: str = Query(None, description="テンプレートを使用する場合のファイル名"),
    password: str = Query(None, description="Excelファイルに設定する場合のパスワード"),
):
    """Excelファイル生成テストエンドポイント"""
    try:
        # バッファを取得しExcelファイルを生成
        with get_excel_buffer() as buffer:
            wb = create_excel_object(template_file)
            save_excel_to_buffer(buffer, wb, password)

            filename = "example.xlsx"

            return StreamingResponse(
                io.BytesIO(buffer.getvalue()),
                media_type="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                headers={
                    'Content-Disposition': f'attachment; filename="{filename}"'
                }
            )
    except FileNotFoundError as e:
        message = f"ファイルが見つかりません: {str(e)}"
        logger.exception(message)
        raise HTTPException(status_code=404, detail=message)
    except Exception as e:
        # その他の例外が発生した場合のロールバック
        message = f"内部サーバーエラーが発生しました: {str(e)}"
        logger.exception(message)
        raise HTTPException(status_code=500, detail=message)

