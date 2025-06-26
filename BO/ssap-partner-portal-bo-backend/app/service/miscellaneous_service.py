import boto3

from app.core.common_logging import CustomLogger
from app.schemas.miscellaneous import HealthCheckResponse

logger = CustomLogger.get_logger()


class MiscellaneousService:
    @staticmethod
    def get_s3_bucket():
        """S3のバケット取得
        Returns:
            buckets : 取得したS3バケットのオブジェクト
        """

        try:
            s3 = boto3.resource("s3")
            buckets = s3.buckets.all()
            return buckets
        except Exception as ex:
            logger.error(ex)

    @staticmethod
    def get_dynamo_db_table():
        """DynamoDBテーブル取得
        Returns:
            tables: DynamoDBテーブル一覧のオブジェクト
        """

        try:
            dynamo = boto3.resource("dynamodb")
            tables = dynamo.tables.all()
            return tables
        except Exception as ex:
            logger.error(ex)

    @staticmethod
    def health_check() -> HealthCheckResponse:
        """死活監視API S3のバケット取得、DynamoDBのテーブル一覧を取得し稼働を確認する

        Returns:
            HealthCheckResponse: 稼働状況
        """
        results = []
        if not MiscellaneousService.get_s3_bucket():
            results.append("S3 NG")

        if not MiscellaneousService.get_dynamo_db_table():
            results.append("DynamoDB NG")

        if not results:
            results.append("Status OK")

        return HealthCheckResponse(results=results)
