from app.schemas.miscellaneous import HealthCheckResponse
from app.service.miscellaneous_service import MiscellaneousService
from fastapi import APIRouter, status

router = APIRouter()


@router.get(
    "/misc/health-check",
    tags=["Miscellaneous"],
    description="死活監視API S3のバケット取得、DynamoDBのテーブル一覧を取得し稼働を確認する",
    response_model=HealthCheckResponse,
    status_code=status.HTTP_200_OK,
)
def health_check() -> HealthCheckResponse:
    """Get /misc/health-check 死活監視API

    Returns:
        HealthCheckResponse: 稼働状況
    """
    return MiscellaneousService.health_check()
