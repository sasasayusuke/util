from datetime import datetime
from typing import Dict, Any
import io
import logging
from fastapi.responses import StreamingResponse
from fastapi.responses import JSONResponse
from copy import copy
from app.services.base_service import BaseService
from app.utils.db_utils import SQLExecutor
from app.utils.excel_utils import get_excel_buffer, create_excel_object, save_excel_to_buffer, range_copy_cell_by_address, ExcelPDFConverter
from app.utils.service_utils import Snw_cm
from app.core.config import settings
from app.core.exceptions import ServiceError

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class SanwaServiceInputService(BaseService):
    """三和サービス実績入力サービス"""

    def insert(self, request, session) -> StreamingResponse:

        # 三和サービス実績入力の登録処理
        logger.info("【START】三和サービス実績入力登録処理")

        # TD手入力鏡更新処理
        insert_sql = "INSERT INTO TMサービス実績情報 (年月,売額_貨物自_三商,原額_貨物自_三商,売額_貨物自_店装,原額_貨物自_店装,売額_貨物自_サンプラ,原額_貨物自_サンプラ,売額_貨物自_三シャ,原額_貨物自_三シャ,売額_貨物自_トラン,原額_貨物自_トラン,売額_貨物自_その他,原額_貨物自_その他,売額_貨物自_事故,原額_貨物自_事故,売額_貨物庸_三商,原額_貨物庸_三商,売額_貨物庸_店装,原額_貨物庸_店装,売額_貨物庸_サンプラ,原額_貨物庸_サンプラ,売額_貨物庸_三シャ,原額_貨物庸_三シャ,売額_貨物庸_トラン,原額_貨物庸_トラン,売額_貨物庸_その他,原額_貨物庸_その他,売額_貨物庸_事故,原額_貨物庸_事故,売額_物流_ルート,原額_物流_ルート,売額_物流_丸和,原額_物流_丸和,売額_物流_その他,原額_物流_その他,売額_物流_事故,原額_物流_事故,人件一般費_貨物,営業外費_貨物,初期登録日,登録変更日,人件一般費_物流,営業外費_物流) VALUES ("
        insert_sql += "'" + request.params['@年月'] + "'"
        insert_sql += "," + str(request.params['@売額_貨物自_三商'])
        insert_sql += "," + str(request.params['@原額_貨物自_三商'])
        insert_sql += "," + str(request.params['@売額_貨物自_店装'])
        insert_sql += "," + str(request.params['@原額_貨物自_店装'])
        insert_sql += "," + str(request.params['@売額_貨物自_サンプラ'])
        insert_sql += "," + str(request.params['@原額_貨物自_サンプラ'])
        insert_sql += "," + str(request.params['@売額_貨物自_三シャ'])
        insert_sql += "," + str(request.params['@原額_貨物自_三シャ'])
        insert_sql += "," + str(request.params['@売額_貨物自_トラン'])
        insert_sql += "," + str(request.params['@原額_貨物自_トラン'])
        insert_sql += "," + str(request.params['@売額_貨物自_その他'])
        insert_sql += "," + str(request.params['@原額_貨物自_その他'])
        insert_sql += "," + str(request.params['@売額_貨物自_事故'])
        insert_sql += "," + str(request.params['@原額_貨物自_事故'])
        insert_sql += "," + str(request.params['@売額_貨物庸_三商'])
        insert_sql += "," + str(request.params['@原額_貨物庸_三商'])
        insert_sql += "," + str(request.params['@売額_貨物庸_店装'])
        insert_sql += "," + str(request.params['@原額_貨物庸_店装'])
        insert_sql += "," + str(request.params['@売額_貨物庸_サンプラ'])
        insert_sql += "," + str(request.params['@原額_貨物庸_サンプラ'])
        insert_sql += "," + str(request.params['@売額_貨物庸_三シャ'])
        insert_sql += "," + str(request.params['@原額_貨物庸_三シャ'])
        insert_sql += "," + str(request.params['@売額_貨物庸_トラン'])
        insert_sql += "," + str(request.params['@原額_貨物庸_トラン'])
        insert_sql += "," + str(request.params['@売額_貨物庸_その他'])
        insert_sql += "," + str(request.params['@原額_貨物庸_その他'])
        insert_sql += "," + str(request.params['@売額_貨物庸_事故'])
        insert_sql += "," + str(request.params['@原額_貨物庸_事故'])
        insert_sql += "," + str(request.params['@売額_物流_ルート'])
        insert_sql += "," + str(request.params['@原額_物流_ルート'])
        insert_sql += "," + str(request.params['@売額_物流_丸和'])
        insert_sql += "," + str(request.params['@原額_物流_丸和'])
        insert_sql += "," + str(request.params['@売額_物流_その他'])
        insert_sql += "," + str(request.params['@原額_物流_その他'])
        insert_sql += "," + str(request.params['@売額_物流_事故'])
        insert_sql += "," + str(request.params['@原額_物流_事故'])
        insert_sql += "," + str(request.params['@人件一般費_貨物'])
        insert_sql += "," + str(request.params['@営業外費_貨物'])
        insert_sql += ",CONVERT(DATE, GETDATE())"
        insert_sql += ",CONVERT(DATE, GETDATE())"
        insert_sql += "," + str(request.params['@人件一般費_物流'])
        insert_sql += "," + str(request.params['@営業外費_物流'])
        insert_sql += ");"
        insert_result = dict(SQLExecutor(session).execute_query(query=insert_sql))

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )


    def update(self, request, session) -> StreamingResponse:

        # 三和サービス実績入力の更新処理
        logger.info("【START】三和サービス実績入力_更新処理")

        # TD手入力鏡更新処理
        update_sql = "UPDATE TMサービス実績情報 SET "
        update_sql += '売額_貨物自_三商 = ' + str(request.params['@売額_貨物自_三商'])
        update_sql += ',原額_貨物自_三商 = ' + str(request.params['@原額_貨物自_三商'])
        update_sql += ',売額_貨物自_店装 = ' + str(request.params['@売額_貨物自_店装'])
        update_sql += ',原額_貨物自_店装 = ' + str(request.params['@原額_貨物自_店装'])
        update_sql += ',売額_貨物自_サンプラ = ' + str(request.params['@売額_貨物自_サンプラ'])
        update_sql += ',原額_貨物自_サンプラ = ' + str(request.params['@原額_貨物自_サンプラ'])
        update_sql += ',売額_貨物自_三シャ = ' + str(request.params['@売額_貨物自_三シャ'])
        update_sql += ',原額_貨物自_三シャ = ' + str(request.params['@原額_貨物自_三シャ'])
        update_sql += ',売額_貨物自_トラン = ' + str(request.params['@売額_貨物自_トラン'])
        update_sql += ',原額_貨物自_トラン = ' + str(request.params['@原額_貨物自_トラン'])
        update_sql += ',売額_貨物自_その他 = ' + str(request.params['@売額_貨物自_その他'])
        update_sql += ',原額_貨物自_その他 = ' + str(request.params['@原額_貨物自_その他'])
        update_sql += ',売額_貨物自_事故 = ' + str(request.params['@売額_貨物自_事故'])
        update_sql += ',原額_貨物自_事故 = ' + str(request.params['@原額_貨物自_事故'])
        update_sql += ',売額_貨物庸_三商 = ' + str(request.params['@売額_貨物庸_三商'])
        update_sql += ',原額_貨物庸_三商 = ' + str(request.params['@原額_貨物庸_三商'])
        update_sql += ',売額_貨物庸_店装 = ' + str(request.params['@売額_貨物庸_店装'])
        update_sql += ',原額_貨物庸_店装 = ' + str(request.params['@原額_貨物庸_店装'])
        update_sql += ',売額_貨物庸_サンプラ = ' + str(request.params['@売額_貨物庸_サンプラ'])
        update_sql += ',原額_貨物庸_サンプラ = ' + str(request.params['@原額_貨物庸_サンプラ'])
        update_sql += ',売額_貨物庸_三シャ = ' + str(request.params['@売額_貨物庸_三シャ'])
        update_sql += ',原額_貨物庸_三シャ = ' + str(request.params['@原額_貨物庸_三シャ'])
        update_sql += ',売額_貨物庸_トラン = ' + str(request.params['@売額_貨物庸_トラン'])
        update_sql += ',原額_貨物庸_トラン = ' + str(request.params['@原額_貨物庸_トラン'])
        update_sql += ',売額_貨物庸_その他 = ' + str(request.params['@売額_貨物庸_その他'])
        update_sql += ',原額_貨物庸_その他 = ' + str(request.params['@原額_貨物庸_その他'])
        update_sql += ',売額_貨物庸_事故 = ' + str(request.params['@売額_貨物庸_事故'])
        update_sql += ',原額_貨物庸_事故 = ' + str(request.params['@原額_貨物庸_事故'])
        update_sql += ',売額_物流_ルート = ' + str(request.params['@売額_物流_ルート'])
        update_sql += ',原額_物流_ルート = ' + str(request.params['@原額_物流_ルート'])
        update_sql += ',売額_物流_丸和 = ' + str(request.params['@売額_物流_丸和'])
        update_sql += ',原額_物流_丸和 = ' + str(request.params['@原額_物流_丸和'])
        update_sql += ',売額_物流_その他 = ' + str(request.params['@売額_物流_その他'])
        update_sql += ',原額_物流_その他 = ' + str(request.params['@原額_物流_その他'])
        update_sql += ',売額_物流_事故 = ' + str(request.params['@売額_物流_事故'])
        update_sql += ',原額_物流_事故 = ' + str(request.params['@原額_物流_事故'])
        update_sql += ',人件一般費_貨物 = ' + str(request.params['@人件一般費_貨物'])
        update_sql += ',営業外費_貨物 = ' + str(request.params['@営業外費_貨物'])
        update_sql += ',登録変更日 = CONVERT(DATE, GETDATE())'
        update_sql += ',人件一般費_物流 = ' + str(request.params['@人件一般費_物流'])
        update_sql += ',営業外費_物流 = ' + str(request.params['@営業外費_物流'])

        update_sql += " WHERE 年月 = '"
        update_sql += request.params['@年月']
        update_sql += "';"
        update_result = dict(SQLExecutor(session).execute_query(query=update_sql))

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )

    def delete(self, request, session) -> StreamingResponse:

        # 三和サービス実績入力の削除処理
        logger.info("【START】三和サービス実績入力データ_削除処理")

        # TMサービス実績情報の削除
        delete_sql = "DELETE FROM TMサービス実績情報 WHERE 年月 = '" + request.params['@年月'] + "';"
        delete_result = dict(SQLExecutor(session).execute_query(query=delete_sql))

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass