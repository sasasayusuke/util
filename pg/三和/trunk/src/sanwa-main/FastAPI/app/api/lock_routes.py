import logging
from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.utils.db_utils import get_db, managed_session, SQLExecutor
from app.models.sql_models import lockQueryRequest,StoredQueryResponse,unlockQueryRequest
from app.core.config import settings

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

router = APIRouter()

@router.post("/execute-lock",response_model=StoredQueryResponse)
def lock_data(request: lockQueryRequest, db: Session = Depends(get_db)):
    """画面ロックのエンドポイント"""
    logger.info(f"ロック処理開始")

    stored_name = "usp_LockData_py"
    params = {
        "@iDataName":request.iDataName,
		"@iNumber":request.iNumber,
		"@iDataCode":request.iDataCode,
		"@iCheck":request.iCheck,
		"@iPCName":request.iPCName
    }
    outputparams = {"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        sql_executor = SQLExecutor(session)
        results = sql_executor.execute_stored_procedure(stored_name, params, outputparams)
        return results


@router.post("/execute-unlock",response_model=StoredQueryResponse)
def lock_undata(request: unlockQueryRequest, db: Session = Depends(get_db)):
    """画面アンロックのエンドポイント"""
    logger.info(f"アンロック処理開始")

    stored_name = "usp_UnLockData_py"
    params = {
        "@iDataName":request.iDataName,
		"@iNumber":request.iNumber,
		"@iDataCode":request.iDataCode,
		"@iPCName":request.iPCName
    }
    outputparams = {"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        sql_executor = SQLExecutor(session)
        results = sql_executor.execute_stored_procedure(stored_name, params, outputparams)
        return results

