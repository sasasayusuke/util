import logging
from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.utils.db_utils import get_db, managed_session, SQLExecutor
from app.models.sync_models import RecordCreateRequest, RecordUpdateRequest, RecordDeleteRequest
from app.core.config import settings
import datetime

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

# テーブル同期API
router = APIRouter(prefix="/sync-record")

@router.post("/create")
def create_record(request: RecordCreateRequest, db: Session = Depends(get_db)):
    """
    レコード作成エンドポイント
    """
    pleasanter_table_name = request.pleasanter_table_name
    new_data = request.new_data

    logger.info(f"レコード作成開始: 同期元テーブル名={pleasanter_table_name} 作成後データ={new_data}")

    table_info = settings.PLEASANTER_INFO.get(pleasanter_table_name)

    # DB操作は managed_session で管理
    with managed_session(db) as session:
        sql_executor = SQLExecutor(session)

        # データ成型用ヘルパー関数
        insert_data = map_data(new_data, table_info)
        # 製品マスタの場合、TM製品_メモにも処理を行う
        if pleasanter_table_name == '製品マスタ':
            if insert_data['メモ'] != '':
                tmp_insert_memo_data = {}
                tmp_insert_memo_data['メモ'] = insert_data['メモ']
                tmp_insert_memo_data['製品NO'] = insert_data['製品NO']
                tmp_insert_memo_data['仕様NO'] = insert_data['仕様NO']
                # レコード作成用のSQLを実行
                sql_executor.execute_insert('TM製品_メモ', tmp_insert_memo_data)
            del insert_data["メモ"]
        # レコード作成用のSQLを実行
        sql_executor.execute_insert(table_info["SDB_NAME"], insert_data)

    return {"status": "ok", "message": "Record created successfully"}


@router.post("/update")
def update_record(request: RecordUpdateRequest, db: Session = Depends(get_db)):
    """
    レコード更新エンドポイント
    """
    pleasanter_table_name = request.pleasanter_table_name
    old_data = request.old_data
    new_data = request.new_data

    logger.info(f"レコード更新開始: 同期元テーブル名={pleasanter_table_name} 更新前データ={old_data} 更新後データ={new_data}")

    table_info = settings.PLEASANTER_INFO.get(pleasanter_table_name)
    with managed_session(db) as session:
        sql_executor = SQLExecutor(session)
        # 更新前と更新後のデータをそれぞれ変換
        old_update_data = map_data(old_data, table_info)
        new_update_data = map_data(new_data, table_info)

        # 製品マスタの場合、TM製品_メモのカラムをpopする処理を行う
        if pleasanter_table_name == '製品マスタ':
            old_memo_data = old_update_data.pop('メモ')
            new_memo_data = new_update_data.pop('メモ')
            
        # 登録更新日を最新化する
        if pleasanter_table_name != '売掛金額マスタ' and pleasanter_table_name != '買掛金額マスタ' and pleasanter_table_name != '請求金額マスタ' and pleasanter_table_name != '支払金額マスタ':
            old_update_data["登録変更日"] = None
            new_update_data["登録変更日"] = datetime.datetime.now().strftime("%Y/%m/%d %H:%M:%S") 

        # タプルにまとめる（0: 更新前, 1: 更新後）
        update_data = (old_update_data, new_update_data)
        # レコード更新用のSQLを実行
        sql_executor.execute_update(table_info["SDB_NAME"], update_data, table_info["KEYS"])

        # 製品マスタの場合、TM製品_メモにも処理を行う
        if pleasanter_table_name == '製品マスタ':
            #TM製品_メモにレコードがない場合は追加、ある場合は更新を行う
            query = f"SELECT COUNT(*) as count FROM TM製品_メモ AS a WHERE a.製品NO = '{old_update_data['製品NO']}' AND a.仕様NO = '{old_update_data['仕様NO']}';"

            # STORED実行
            result = dict(SQLExecutor(session).execute_query(query=query))
            if int(result["results"][0]["count"]) < 1:
                tmp_insert_memo_data = {}
                tmp_insert_memo_data['メモ'] = new_memo_data
                tmp_insert_memo_data['製品NO'] = new_update_data['製品NO']
                tmp_insert_memo_data['仕様NO'] = new_update_data['仕様NO']
                # レコード作成用のSQLを実行
                sql_executor.execute_insert('TM製品_メモ', tmp_insert_memo_data)
            else:
                tmp_old_update_memo_data = {}
                tmp_old_update_memo_data['メモ'] = old_memo_data
                tmp_old_update_memo_data['製品NO'] = old_update_data['製品NO']
                tmp_old_update_memo_data['仕様NO'] = old_update_data['仕様NO']
                tmp_new_update_memo_data = {}
                tmp_new_update_memo_data['メモ'] = new_memo_data
                tmp_new_update_memo_data['製品NO'] = new_update_data['製品NO']
                tmp_new_update_memo_data['仕様NO'] = new_update_data['仕様NO']
                # タプルにまとめる（0: 更新前, 1: 更新後）
                update_memo_data = (tmp_old_update_memo_data, tmp_new_update_memo_data)

                # レコード更新用のSQLを実行
                sql_executor.execute_update('TM製品_メモ', update_memo_data, table_info["KEYS"])

    return {"status": "ok", "message": "Record updated successfully"}


@router.post("/delete")
def delete_record(request: RecordDeleteRequest, db: Session = Depends(get_db)):
    """
    レコード削除エンドポイント
    """
    pleasanter_table_name = request.pleasanter_table_name
    old_data = request.old_data

    logger.info(f"レコード削除開始: 同期元テーブル名={pleasanter_table_name} 削除前データ={old_data}")

    table_info = settings.PLEASANTER_INFO.get(request.pleasanter_table_name)

    with managed_session(db) as session:
        sql_executor = SQLExecutor(session)
        # データ成型用ヘルパー関数
        delete_data = map_data(old_data, table_info)
        
        # レコード削除用のSQLを実行
        sql_executor.execute_delete(table_info["SDB_NAME"], delete_data, table_info["KEYS"])

        # 製品マスタの場合、TM製品_メモにも処理を行う
        if pleasanter_table_name == '製品マスタ':
            # レコード削除用のSQLを実行
            sql_executor.execute_delete('TM製品_メモ', delete_data, table_info["KEYS"])

    return {"status": "ok", "message": "Record deleted successfully"}


# --- 以下にヘルパー関数を配置する場合 ---
def map_data(source_data: dict, table_info: dict) -> dict:
    """
    source_data のキーと table_info のマッピングに基づいて、
    数値に変換できる文字列は整数に変換しながら新しい辞書を作成する。
    ⇒整数に変換しないよう修正 2025/02/27 ootani
    """
    result = {}
    for key, col_name in table_info.items():
        if key in source_data:
            value = source_data[key]
            # if isinstance(value, str) and value.isdigit():
            #     value = int(value)
            result[col_name] = value
    return result

@router.post("/import")
def import_record():

    return {"status": "ok", "message": "Record deleted successfully"}