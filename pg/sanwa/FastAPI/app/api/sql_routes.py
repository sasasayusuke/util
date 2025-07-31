
import logging
from fastapi import APIRouter, Depends
from fastapi.responses import JSONResponse
from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from app.utils.db_utils import SQLExecutor, get_db, managed_session
from app.models.sql_models import SQLQueryRequest, SQLQueryResponse, StoredQueryRequest, StoredQueryResponse, getTaxQueryRequest
from app.core.config import settings
from app.core.exceptions import ServiceError
import datetime

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

router = APIRouter()

@router.post("/execute-get-tax", response_model=SQLQueryResponse)
def execute_get_tax_sql(request: getTaxQueryRequest, db: Session = Depends(get_db)):
    """
    税率を取得するSQLクエリを実行するエンドポイント

    Args:
        request (SQLQueryRequest): 実行するSQLクエリを含むリクエストオブジェクト

    Returns:
        SQLQueryResponse: クエリ実行結果を含むレスポンスオブジェクト
    """
    sql_executor = SQLExecutor(db)
    
    #iDateの値に不正なデータがないかチェック
    if type(request.iDate) is not datetime.datetime:
        raise ServiceError('税率取得クエリに正しい引数(日付型)を渡してください')
    
    date_str = (request.iDate).strftime('%Y/%m/%d')

    query = f"select dbo.fn_GetTax('{date_str}') AS 税率"

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        results = sql_executor.execute_query(query)
        return results

@router.post("/execute-sql", response_model=SQLQueryResponse)
def execute_sql(request: SQLQueryRequest, db: Session = Depends(get_db)):
    """
    SQLクエリを実行するエンドポイント

    Args:
        request (SQLQueryRequest): 実行するSQLクエリを含むリクエストオブジェクト

    Returns:
        SQLQueryResponse: クエリ実行結果を含むレスポンスオブジェクト
    """
    sql_executor = SQLExecutor(db)

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        results = sql_executor.execute_query(request.query)
        return results

@router.post("/execute-stored-procedure", response_model=StoredQueryResponse)
def execute_stored_procedure(request: StoredQueryRequest, db: Session = Depends(get_db)):
    """
    ストアドプロシージャを実行するエンドポイント

    Args:
        request (StoredQueryRequest): 実行するストアドプロシージャ名とパラメータを含むリクエストオブジェクト

    Returns:
        StoredQueryResponse: ストアドプロシージャ実行結果を含むレスポンスオブジェクト
    """
    sql_executor = SQLExecutor(db)

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        results = sql_executor.execute_stored_procedure(request.storedname, request.params, request.output_params)
        return results

@router.get("/health")
def health_check(db: Session = Depends(get_db)):
    """
    データベース接続状態を確認するヘルスチェックエンドポイント

    Returns:
        dict: ヘルスチェック結果を含むレスポンス
            - 成功時: {"status": "OK", "database": "connected"}
            - 失敗時: {"status": "ERROR", "database": "disconnected", "message": エラー詳細}

    Note:
        - データベースに対して簡単なSELECTクエリを実行して接続を確認
        - エラー時は503または500ステータスコードを返却
    """
    sql_executor = SQLExecutor(db)
    try:
        # データベース接続をチェック
        result = sql_executor.execute_query("SELECT 1")
        return {
            "status": "OK",
            "database": "connected"
        }
    except SQLAlchemyError as e:
        logger.exception(f"データベース接続エラー: {str(e)}")
        return JSONResponse(
            status_code=503,
            content={
                "status": "ERROR",
                "database": "disconnected",
                "message": "データベースに接続できません"
            }
        )
    except Exception as e:
        logger.exception(f"ヘルスチェック中に予期せぬエラーが発生しました: {str(e)}")
        return JSONResponse(
            status_code=500,
            content={
                "status": "ERROR",
                "message": "ヘルスチェック中に内部サーバーエラーが発生しました"
            }
        )

