import logging
import time
from typing import Union, List
from fastapi import HTTPException
from sqlalchemy import create_engine, text
from sqlalchemy.orm import sessionmaker, Session
from sqlalchemy.exc import OperationalError, SQLAlchemyError
from app.core.config import settings
from app.core.exceptions import ServiceError
from app.utils.string_utils import decode_str_value, decode_bytes_value
from app.models.sql_models import SQLQueryResponse, StoredQueryResponse
from contextlib import contextmanager
from decimal import Decimal

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

# データベースエンジンの作成（再利用可能）
db_engine = create_engine(settings.DB_URL)

# セッションの取得
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=db_engine)

# セッションの依存性を注入する関数(トランザクション管理を必要としない操作に使用)
def get_db():
    # セッションを生成
    db = SessionLocal()
    try:
        # 呼び出し先にセッションを提供
        yield db
    finally:
        # 使用後にセッションをクローズ
        db.close()

# コンテキストマネージャの定義(複雑なトランザクションやエラーハンドリングが必要な場合に使用し、エラー発生時にはロールバック、正常時にはコミットを行う。)
@contextmanager
def managed_session(db: Session, retries: int = 5, wait: float = 1.0):
    attempt = 0
    while attempt < retries:
        try:
            # トランザクション開始
            with db.begin():  # ←ここで begin() を明示的に開始
                # セッション提供
                yield db
                # 正常終了時にコミット
                # db.commit() # commit は with db.begin() のブロックを抜けると自動で行われる
            break  # 成功したらループ抜け
        except ServiceError as e:
            # データベースエラーが発生した場合のロールバック
            db.rollback()
            raise HTTPException(status_code=500, detail=str(e))
        except OperationalError as e:
            # SQL Server デッドロック判定コード 1205
            if "1205" in str(e):
                attempt += 1
                logger.warning(f"SQLクエリ実行中にデッドロック検出: リトライ {attempt}/{retries}")
                db.rollback()
                time.sleep(wait)  # 少し待って再試行
                continue
            else:
                db.rollback()
                message = f"SQLクエリ実行中にデータベースエラーが発生しました: {str(e)}"
                logger.exception(message)
                raise HTTPException(status_code=503, detail=message)
        except SQLAlchemyError as e:
            # データベースエラーが発生した場合のロールバック
            db.rollback()
            message = f"SQLクエリ実行中にデータベースエラーが発生しました: {str(e)}"
            logger.exception(message)
            raise HTTPException(status_code=503, detail=message)
        except Exception as e:
            # その他の例外が発生した場合のロールバック
            db.rollback()
            message = f"内部サーバーエラーが発生しました: {str(e)}"
            logger.exception(message)
            raise HTTPException(status_code=500, detail=message)
    else:
        # リトライ上限に達した場合
        message = f"SQLクエリ実行中にデッドロックが連続発生しました。"
        logger.exception(message)
        raise HTTPException(status_code=503, detail=message)

# SQLExecutor クラスの定義
class SQLExecutor:
    def __init__(self, db: Session):
        self.db = db

    def _convert_value(self, val):
        """
        データベースから取得した値を適切な型に変換する

        Args:
            val: 変換対象の値

        Returns:
            変換後の値
        """
        # Decimal 型
        if isinstance(val, Decimal):
            # 整数の場合
            if val == val.to_integral():
                return int(val)
            # 小数の場合
            else:
                return float(val)
        # string 型
        elif isinstance(val, str):
            return decode_str_value(val)
        # bytes 型
        elif isinstance(val, bytes):
            return decode_bytes_value(val)
        # その他
        return val

    def execute_query(self, query: str):
        """
        SQLクエリを実行する関数

        Args:
            query (str): 実行するSQLクエリ

        Returns:
            SQLQueryResponse: クエリの結果のリスト、または操作完了メッセージを含むレスポンス
        """
        logger.info(f"SQL実行クエリ: {query}")

        # セッションを使ってクエリを実行
        result = self.db.execute(text(query))

        # クエリが結果を返すものかどうかを判断
        if result.returns_rows:
            query_result_list = []
            for row in result:
                tmp = {}
                for key,val in dict(row).items():
                    tmp[key] = self._convert_value(val)
                query_result_list.append(tmp)

            count = len(query_result_list)
            response = SQLQueryResponse(
                results=query_result_list,
                count=count,
                message=f"クエリが正常に完了し {count} 件のデータを取得しました"
            )
        else:
            response = SQLQueryResponse(results=[], count=0, message="操作が正常に完了しました")


        logger.info(
            f"実行結果: {response.results}\n"
            f"実行メッセージ: {response.message}\n"
        )
        return response

    def execute_insert(self, table_name: str, data: Union[dict, List[dict]]):
        """
        INSERT 用クエリを実行する関数（None の値は空文字とし、挿入する値がないレコードはスキップ）

        Args:
            table_name (str): 挿入先のテーブル名
            data (Union[dict, List[dict]]): 単一レコードまたはレコードのリスト
                例:
                {'見積番号': 181440, '仕入先CD': '4603', ... }
                または
                [
                    {'見積番号': 181440, '仕入先CD': '4603', ... },
                    {'見積番号': 181440, '仕入先CD': '4603', ... },
                ]

        Returns:
            SQLQueryResponse: 挿入結果に関するレスポンス
        """
        # データが空の場合
        if not data:
            return SQLQueryResponse(results=[], count=0, message="挿入するデータがありません。")

        # 単一の辞書の場合はリストに変換
        if isinstance(data, dict):
            data = [data]

        total_count = 0
        skipped_count = 0

        for record in data:
            # 各レコードから値が None の項目を除外
            filtered_record = {k: v for k, v in record.items() if v is not None}

            # 挿入する値が1件もない場合はスキップ
            if not filtered_record:
                logger.info("このレコードの作成はスキップします。")
                skipped_count += 1
                continue

            # レコードごとにINSERT用のクエリを生成
            columns = list(filtered_record.keys())
            columns_str = ", ".join([f"[{col}]" for col in columns])
            placeholders = ", ".join([f":{col}" for col in columns])
            insert_query = f"INSERT INTO {table_name} ({columns_str}) VALUES ({placeholders})"
            logger.info(
                f"実行クエリ: {insert_query}\n"
                f"| パラメータ: {filtered_record} \n"
            )

            result = self.db.execute(text(insert_query), filtered_record)
            total_count += result.rowcount if result.rowcount is not None else 1

        message = f"{table_name} へ {total_count} 件のレコードを挿入しました。"
        if skipped_count:
            message += f" {skipped_count} 件のレコードは挿入する値がなかったためスキップしました。"
        return SQLQueryResponse(results=[], count=total_count, message=message)


    def execute_update(self, table_name: str, data: tuple, key_columns: List[str]) -> SQLQueryResponse:
        """
        UPDATE 用クエリを実行する関数（None の値や、old/new の値が同一の場合は更新対象から除外し、更新する値がなければ実行しない）

        Args:
            table_name (str): 更新先のテーブル名
            data (tuple): (old_data, new_data) のタプル
                - old_data: WHERE 句の条件に使用するデータ（dict または dict のリスト）
                - new_data: SET 句に使用する更新後のデータ（dict または dict のリスト）
            key_columns (List[str]): WHERE 句に使用するカラム名のリスト

        Returns:
            SQLQueryResponse: 更新結果に関するレスポンス
        """
        # dataがタプルで、2要素あることをチェック
        if not data or not isinstance(data, tuple) or len(data) != 2:
            return SQLQueryResponse(results=[], count=0, message="更新するデータが正しくありません。")

        old_data, new_data = data

        # 単一レコードの場合はリストに変換
        if isinstance(old_data, dict):
            old_data = [old_data]
        if isinstance(new_data, dict):
            new_data = [new_data]

        # 件数が一致しているか確認
        if len(old_data) != len(new_data):
            raise HTTPException(status_code=400, detail="古いデータと新しいデータの件数が一致しません。")

        total_count = 0
        skipped_count = 0

        # レコードごとに処理
        for old_rec, new_rec in zip(old_data, new_data):
            update_dict = {}
            for col, new_value in new_rec.items():
                # new の値が None の場合は更新対象から除外
                # if new_value is None:
                #     continue
                # 古い値と同じ場合も更新する必要がないため除外
                if col in old_rec and old_rec[col] == new_value:
                    continue
                update_dict[col] = new_value

            # 更新するカラムがない場合はスキップ
            if not update_dict:
                logger.info("このレコードの更新はスキップします。")
                skipped_count += 1
                continue

            # SET 句の作成（更新対象の値は update_dict の値。パラメータ名には new_ プレフィックスを付与）
            set_clause = ", ".join([f"[{col}] = :new_{col}" for col in update_dict.keys()])
            # WHERE 句の作成（条件は key_columns に基づき、old_rec の値を使用。パラメータ名には old_ プレフィックスを付与）
            where_clause = " AND ".join([f"[{col}] = :old_{col}" for col in key_columns])
            update_query = f"UPDATE {table_name} SET {set_clause} WHERE {where_clause}"
            logger.info(
                f"実行クエリ: {update_query}\n"
                f"| パラメータ: {update_dict} \n"
            )

            # パラメータ辞書の作成
            params = {}
            for col, value in update_dict.items():
                params[f"new_{col}"] = value
            for col in key_columns:
                if col in old_rec:
                    params[f"old_{col}"] = old_rec[col]
                else:
                    message = f"古いデータにWHERE条件のカラム [{col}] が含まれていません。"
                    logger.error(message)
                    raise HTTPException(status_code=400, detail=message)

            result = self.db.execute(text(update_query), params)
            total_count += result.rowcount if result.rowcount is not None else 1

        message = f"{table_name} で {total_count} 件のレコードを更新しました。"
        if skipped_count:
            message += f" {skipped_count} 件のレコードは更新する必要がなかったためスキップしました。"
        return SQLQueryResponse(results=[], count=total_count, message=message)


    def execute_delete(self, table_name: str, data: Union[dict, List[dict]], key_columns: List[str]) -> SQLQueryResponse:
        """
        DELETE 用クエリを実行する関数

        Args:
            table_name (str): 削除対象のテーブル名
            data (Union[dict, List[dict]]): 単一レコードまたはレコードのリスト
                                            例:
                                            {'見積番号': 181440, '仕入先CD': '4603', ... }
                                            または
                                            [
                                                {'見積番号': 181440, '仕入先CD': '4603', ... },
                                                {'見積番号': 181441, '仕入先CD': '4604', ... },
                                            ]
            key_columns (List[str]): WHERE 句に使用するカラム名のリスト（例：主キーなど）

        Returns:
            SQLQueryResponse: 削除結果に関するレスポンス
        """
        # 削除するデータが空の場合
        if not data:
            return SQLQueryResponse(results=[], count=0, message="削除するデータがありません。")

        # 単一の辞書の場合はリストに変換
        if isinstance(data, dict):
            data = [data]

        # WHERE句の作成
        where_clause = " AND ".join([f"[{col}] = :{col}" for col in key_columns])

        delete_query = f"DELETE FROM {table_name} WHERE {where_clause}"
        logger.info(f"実行クエリ: {delete_query}")

        result = self.db.execute(text(delete_query), data)
        count = result.rowcount if result.rowcount is not None else len(data)
        response = SQLQueryResponse(
            results=[],
            count=count,
            message=f"{table_name} から {count} 件のレコードを削除しました。"
        )

        logger.info(f"実行メッセージ: {response.message}")
        return response

    def execute_stored_procedure(self, storedname: str, params: dict, outputparams: dict = {"@RetST": "INT", "@RetMsg": "VARCHAR(255)"}):
        """
        ストアドプロシージャを実行する関数

        Args:
            storedname (str): ストアドプロシージャの名前
            params (dict): ストアドプロシージャのパラメータ
            outputparams (dict): 出力パラメータの初期値 (デフォルト: {"@RetST": "", "@RetMsg": ""})

        Returns:
            StoredQueryResponse: 実行結果のリスト、出力パラメータ、メッセージを含むレスポンス
        """
        logger.info(f"ストアドプロシージャ実行開始: {storedname} - {params} - {outputparams}")

        # パラメータのクエリ部分を作成
        query_params = []
        for key, value in params.items():
            if isinstance(value, (int, float)):
                query_params.append(f"{key}={value}")
            elif(value == None):
                query_params.append(f"{key}=null")
            else:
                query_params.append(f"{key}='{value}'")

        # DECLARE, OUTPUT, SELECT を作成
        declare_statements = []
        output_statements = []
        select_columns = []
        prefix_alias = "tmp_"
        for key, value in outputparams.items():
            declare_statements.append(f"DECLARE {key} {value}")
            output_statements.append(f"{key}={key} OUTPUT")
            select_columns.append(f"{key} AS {prefix_alias}{key}")

        # クエリを構築
        query = f"""
            {'; '.join(declare_statements)};
            EXEC {storedname} {', '.join(query_params)}, {', '.join(output_statements)};
            SELECT {', '.join(select_columns)};
        """

        logger.info(f"実行クエリ: {query}")
        result = self.db.execute(text(query))

        # 複数のテーブルセットを格納するリスト
        all_results = []
        output_values = {}
        total_count = 0

        # カーソルを取得
        cursor = result.cursor
        # すべての結果セットを処理
        while True:
            current_result = []
            if cursor.description:
                columns = [desc[0] for desc in cursor.description]

                # OUTPUT パラメータのセットかチェック
                if any(col.startswith(prefix_alias) for col in columns):
                    row = cursor.fetchone()
                    if row:
                        for i, col in enumerate(columns):
                            # prefixを除去してパラメータ名を取得
                            param_name = col.replace(prefix_alias, '')
                            if param_name in outputparams:
                                output_values[param_name] = self._convert_value(row[i])
                else:
                    # 通常の結果セット処理
                    for row in cursor:
                        tmp = {}
                        for i, val in enumerate(row):
                            tmp[columns[i]] = self._convert_value(val)
                        current_result.append(tmp)

                    all_results.append(current_result)
                    total_count += len(current_result)

            if not cursor.nextset():
                break

        if total_count > 0:
            response = StoredQueryResponse(
                results=all_results,
                count=total_count,
                output_values=output_values,
                message=f"ストアドプロシージャが正常に完了し、{len(all_results)}個の結果セットから合計{total_count}件のデータを取得しました"
            )
        else:
            response = StoredQueryResponse(
                results=[[]],
                count=0,
                output_values=output_values,
                message="ストアドプロシージャが正常に完了しました"
            )

        logger.info(
            f"実行結果: {response.results}\n"
            f"実行アウトプット: {response.output_values}\n"
            f"実行メッセージ: {response.message}\n"
        )
        return response

    def get_columns(self, table_name: str):
        """
        テーブルのカラム情報を取得する関数

        Args:
            table_name (str): カラム情報を取得するテーブル名

        Returns:
            SQLQueryResponse: カラム情報を含むレスポンス
        """
        query = f"""
            SELECT
                COLUMN_NAME
            FROM
                INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{table_name}'
        """
        result = self.execute_query(query)

        # resultsから単純な文字列配列に変換
        columns = [row['COLUMN_NAME'] for row in result.results]
        return columns