
import logging
from fastapi import Request, APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.services.service_factory import ServiceFactory
from app.models.service_models import ServiceRequest
from app.utils.db_utils import get_db, managed_session
from app.core.config import settings


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

router = APIRouter()

@router.post("/execute-service")
def execute_service(request: ServiceRequest, db: Session = Depends(get_db)):
    """サービス実行エンドポイント"""
    logger.info(f"サービス実行開始: {request.category}/{request.title} - {request.button}")

    # managed_session を使ってセッションを管理
    with managed_session(db) as session:
        # サービスファクトリの初期化とリクエストの処理
        factory = ServiceFactory()
        result = factory.handle_request(request=request, session=session)
        return result


