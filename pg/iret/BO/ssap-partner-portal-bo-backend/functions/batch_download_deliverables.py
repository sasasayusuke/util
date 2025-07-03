import sys
import traceback
import io
import boto3
import os
import zipfile
import asyncio
from abc import abstractmethod
from dataclasses import dataclass
from enum import Enum
from typing import Optional
from logging import getLogger
from datetime import datetime, timedelta
from dateutil.relativedelta import relativedelta
from botocore.config import Config
from botocore.exceptions import ClientError

from app.core.config import get_app_settings
from app.resources.const import UserRoleType
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from app.models.master import BatchControlErrors, MasterBatchControlModel
from app.models.admin import AdminModel
from functions.batch_exceptions import ExitHandler
from functions.batch_common import (
    batch_end_common_procedure,
    batch_init_common_procedure,
    get_operation_datetime,
    send_mail
)
from functions.batch_const import (
    BatchErrorType,
    BatchFunctionId,
    BatchFunctionName,
    BatchStatus,
    DataType,
    MailType,
    MasterDataType
)


# ログ出力設定をロードする
get_app_settings().configure_logging()
logger = getLogger(__name__)


class DeliverableType(Enum):
    PROJECT = 'project'
    DELIVERABLE = 'deliverable'


# アップロード対象ファイルクラス
@dataclass
class UploadFile:
    key: str
    name: Optional[str] = None


class BaseBundleDeliverables:
    # バケット名
    BUCKET_NAME = get_app_settings().upload_s3_bucket_name
    # アップロード可能ファイルサイズ 3GB
    MAX_FILE_SIZE = (
        int(os.getenv("MAX_FILE_SIZE"))
        if os.getenv("MAX_FILE_SIZE")
        else (1024 * 1024 * 1024) * 3
    )
    # タイムアウト設定
    s3_client = boto3.client('s3', config=Config(
        connect_timeout=600,
        read_timeout=600
    ))
    # バッチID
    batch_control_id: str
    # 実行タイプ
    type: DeliverableType
    # 今日
    today: datetime
    # 対象年
    target_year: str
    # 対象月の1日 00:00:00
    first_day_target_month: datetime
    # 対象月の最終日 23:59:59
    last_day_target_month: datetime
    # 成果物格納先のパス
    base_deliverables_prefix: str
    # 合計アップロードファイルサイズ
    total_upload_file_size: int

    def __init__(
        self,
        event: dict[str, any],
        batch_control_id: str,
        type: DeliverableType,
        target_month: Optional[str] = None
    ):
        self.event = event
        self.batch_control_id = batch_control_id
        self.type = type
        self._set_date(target_month)
        self.base_deliverables_prefix = f'deliverables/{self.target_year}'
        self.total_upload_file_size = 0

    async def execute(self):
        # 起動日時
        operation_start_datetime: datetime = get_operation_datetime()
        logger.info(f"operation_start_datetime: {operation_start_datetime}")

        # 初期処理
        batch_init_common_procedure(
            event=self.event,
            operation_start_datetime=operation_start_datetime,
            batch_control_id=self.batch_control_id,
            batch_control_name=(
                BatchFunctionName.DOWNLOAD_DELIVERABLES_BATCH
                if self.type == DeliverableType.PROJECT
                else BatchFunctionName.DOWNLOAD_DELIVERABLES_PROJECT_BATCH
            ),
        )

        # 実行
        errors: list[dict] = await self._execute()

        # 終了処理
        self.batch_end_procedure(
            batch_status=BatchStatus.EXECUTED,
            batch_control_id=self.batch_control_id,
            errors=errors
        )

    def batch_end_procedure(self, batch_status: str, batch_control_id: str, errors: list[dict]):
        """
            終了処理(共通)

        Args:
            event:
            operation_start_datetime (datetime): 起動日時
            batch_control_id (str): バッチ関数ID
        """
        # 汎用マスター バッチ処理制御の項目取得
        range_key_condition = MasterBatchControlModel.value == batch_control_id
        batch_control_list: list[MasterBatchControlModel] = list(
            MasterBatchControlModel.data_type_value_index.query(
                hash_key=MasterDataType.BATCH_CONTROL,
                range_key_condition=range_key_condition,
            )
        )
        datetime_now = datetime.now()
        batch_control_list[0].update(
            actions=[
                MasterBatchControlModel.attributes.batch_end_at.set(datetime_now),
                MasterBatchControlModel.attributes.batch_status.set(batch_status),
                MasterBatchControlModel.update_at.set(datetime_now),
                MasterBatchControlModel.errors.set(errors if len(errors) > 0 else None)
            ]
        )

    @abstractmethod
    async def _execute(self) -> list[dict]:
        raise NotImplementedError

    def _set_date(self, target_month: Optional[str] = None) -> datetime:
        """
            先月の1日を取得
          Args:
            なし
          Returns:
            なし
        """
        self.today = datetime.today()
        target_date = self.today if not target_month else datetime.strptime(target_month, '%Y%m')
        self.target_year = str(self.today.year)

        # 今月の1日
        first_day_of_target_month = (
            target_date.replace(day=1)
            if not target_month else
            target_date + relativedelta(months=1)
        )

        # 対象月の1日
        self.first_day_target_month = (
            (first_day_of_target_month - timedelta(days=1))
            .replace(day=1, hour=0, minute=0, second=0, microsecond=0)
        )

        # 対象月の最終日
        self.last_day_target_month = (
            (first_day_of_target_month - timedelta(days=1))
            .replace(hour=23, minute=59, second=59, microsecond=999999)
        )

    def _format_datetime(self, datetime: datetime, format: str = '%Y/%m/%d %H:%M') -> str:
        """
            datetimeオブジェクトをフォーマット
          Args:
            datetime: 対象の日時
            format: フォーマット
          Returns:
            str: フォーマットされた日付
        """
        return datetime.strftime(format)

    def _create_bundle_zip_file(self, upload_files: list[UploadFile]) -> Optional[io.BytesIO]:
        """
          S3に存在する複数のファイルをZIPファイルに圧縮

          Args:
            list[UploadFile]: 対象のファイル
          Returns:
            io.BytesIO: zipファイル
        """
        zip_buffer = io.BytesIO()

        # ZIP ファイルを作成
        try:
            with zipfile.ZipFile(zip_buffer, 'w', zipfile.ZIP_DEFLATED) as zip_file:
                target_key = ''
                for file in upload_files:
                    # S3 からファイルを取得し、ZIP に書き込む
                    target_key = file.key
                    file_object = self.s3_client.get_object(Bucket=self.BUCKET_NAME, Key=file.key)
                    # ファイルの内容を直接 ZIP に追加
                    file_name = file.name if file.name else file.key.split('/')[-1]
                    zip_file.writestr(file_name, file_object['Body'].read())

            # バッファの位置を先頭に戻す
            zip_buffer.seek(0)
            return zip_buffer
        except ClientError as e:
            if e.response["Error"]["Code"] == "NoSuchKey":
                logger.warning(f'Object Not Found: {target_key}')
                return None
            else:
                logger.error(f'Create Bundle ZipFile Error: {e}')
                raise e
        except Exception as e:
            logger.error(f'Create Bundle ZipFile Error: {e}')
            raise e

    async def _multipart_upload(self, zip_buffer: io.BytesIO, file_key: str) -> bool:
        """
          S3にzipファイルをアップロード

          Args:
            zip_buffer: 対象のzipファイル
          Returns:
            bool: 結果
        """
        try:
            # マルチパートアップロードの開始
            multipart_upload = self.s3_client.create_multipart_upload(
                Bucket=self.BUCKET_NAME,
                Key=file_key
            )
            upload_id = multipart_upload['UploadId']
            # 5MB のパートサイズ
            part_size = 5 * 1024 * 1024
            part_number = 1
            parts = []
            tasks = []
            while True:
                part_data = zip_buffer.read(part_size)
                if not part_data:
                    break
                tasks.append(self._upload_part(
                    file_key, upload_id, part_number, part_data
                ))
                part_number += 1
            parts = await asyncio.gather(*tasks)
            # アップロードが完了したらマルチパートアップロードを完了させる
            self.s3_client.complete_multipart_upload(
                Bucket=self.BUCKET_NAME,
                Key=file_key,
                UploadId=upload_id,
                MultipartUpload={'Parts': parts}
            )
            return True
        except Exception as e:
            # エラーが発生した場合、アップロードを中止し、マルチパートアップロードを中止する
            logger.error(f'Error Multipart Uploading: {e}')
            self.s3_client.abort_multipart_upload(
                Bucket=self.BUCKET_NAME,
                Key=file_key,
                UploadId=upload_id
            )
            return False

    async def _upload_part(self, file_key, upload_id, part_number, part_data) -> dict:
        """
          マルチパートアップロード

          Args:
            file_key: 取得対象のプレフィックス
            upload_id: アップロードID
            part_number: マルチパートアップロードのパート番号
            part_data: マルチパートアップロードのパートデータ
          Returns:
            dict: {"ETag": レスポンスのETag, "PartNumber": パート番号}
        """
        response = self.s3_client.upload_part(
            Bucket=self.BUCKET_NAME,
            Key=file_key,
            UploadId=upload_id,
            PartNumber=part_number,
            Body=part_data
        )
        return {"ETag": response['ETag'], "PartNumber": part_number}

    def _get_s3_object_keys(self, prefix: str) -> list[str]:
        """
          S3に存在するオブジェクトのキー一覧を取得

          Args:
            prefix: 取得対象のプレフィックス
          Returns:
            list[str]: キーリスト
        """
        keys = []
        continuation_token = None

        while True:
            # リストアップ
            params = {
                'Bucket': self.BUCKET_NAME,
                'Prefix': prefix,
                'Delimiter': '/',
            }
            if continuation_token:
                params['ContinuationToken'] = continuation_token

            response = self.s3_client.list_objects_v2(**params)

            # オブジェクトキーのリストを追加
            if 'Contents' in response:
                keys.extend([obj['Key'] for obj in response['Contents']])

            # 次のページがあるか確認
            continuation_token = response.get('NextContinuationToken')
            if not continuation_token:
                break

        return keys

    def _delete_objects(self, file_keys: list[str]) -> bool:
        """
          S3から指定されたオブジェクトを削除

          Args:
            file_keys: 削除対象のキーリスト
          Returns:
            bool: 結果
        """
        try:
            response = self.s3_client.delete_objects(
                Bucket=self.BUCKET_NAME,
                Delete={
                    'Objects': [{'Key': key} for key in file_keys],
                    'Quiet': False
                }
            )
            if 'Errors' in response:
                logger.error('Delete Objects Error: None')
                return False
            else:
                return True
        except Exception as e:
            logger.error(f'Delete Objects Error: {e}')
            return False

    def _create_destination_key(self, type: DeliverableType) -> str:
        """
          アップロード先のS3パス

          Args:
            なし
          Returns:
            str: S3パス
        """
        if type == DeliverableType.PROJECT:
            return f"{self.base_deliverables_prefix}/{self._format_datetime(self.first_day_target_month, '%Y%m')}"
        elif type == DeliverableType.DELIVERABLE:
            return f"{self.base_deliverables_prefix}"

    def _get_s3_file_size(self, object_key: str) -> int:
        """
          S3に存在するファイルのサイズを取得する

          Args:
            str: ファイルキー
          Returns:
            int: ファイルサイズ
        """
        try:
            response = self.s3_client.head_object(Bucket=self.BUCKET_NAME, Key=object_key)
            file_size = response['ContentLength']
            return file_size
        except Exception:
            return 0


class BundleProjectDeliverables(BaseBundleDeliverables):
    """
        案件ごとの個別カルテに添付されている成果物をzip化し、まとめる
    """

    def __init__(
        self,
        event: dict[str, any],
        batch_control_id: str,
        type: DeliverableType,
        target_month: Optional[str] = None
    ):
        super().__init__(event, batch_control_id, type, target_month)

    async def _execute(self) -> list[dict]:
        """
          実行

          Args:
            なし
          Returns:
            なし
        """
        projects: list[ProjectModel] = self._get_projects()
        # 全案件を並行で処理する
        errors: list[dict] = await asyncio.gather(*[self._upload(project) for project in projects])

        overflow_files: list[dict] = [result for result in errors if result]
        if overflow_files:
            # 実行エラーデータを返却
            return [
                {
                    'type': BatchErrorType.OVERFLOW_FILES,
                    'details': overflow_files
                }
            ]
        else:
            return []

    def _get_projects(self) -> list[ProjectModel]:
        """
          支援期間中の案件取得

          Args:
            なし
          Returns:
            list[ProjectModel]: 案件リスト
        """
        return list(
            ProjectModel.data_type_support_date_to_index.query(
                hash_key=DataType.PROJECT,
                range_key_condition=ProjectModel.support_date_to >= self._format_datetime(self.last_day_target_month, '%Y/%m/%d')
            )
        )

    async def _upload(self, project: ProjectModel) -> Optional[dict]:
        """
          案件ごとに成果物をアップロード

          Args:
            project: 対象の案件
          Returns:
            bool: 結果
        """
        # 対象のカルテを取得
        karte: Optional[ProjectKarteModel] = self._get_target_karte(project.id)
        if not karte:
            return None

        # アップロード対象ファイルキーリスト
        upload_files: list[UploadFile] = []
        # 容量オーバーのため、アップロードできないファイルキーリスト
        overflow_files: list[UploadFile] = []

        for deliverable in karte.deliverables:
            # 合計ファイルサイズを追加
            self.total_upload_file_size += self._get_s3_file_size(deliverable.path)

            if self.total_upload_file_size <= self.MAX_FILE_SIZE:
                upload_files.append(UploadFile(
                    key=deliverable.path,
                    name=deliverable.file_name
                ))
            else:
                overflow_files.append(UploadFile(
                    key=deliverable.path,
                    name=deliverable.file_name
                ))

        if upload_files:
            # 成果物をすべてzip化
            target_files = [
                f'ファイル: {file.key}({file.name})' for file in upload_files
            ]
            logger.info(f'案件名: {project.name}, アップロード対象成果物: {target_files}')
            zip_buffer: Optional[io.BytesIO] = self._create_bundle_zip_file(upload_files)

            if not zip_buffer:
                return None

            # アップロード
            file_key: str = f'{self._create_destination_key(self.type)}/{project.customer_name}_{project.name}.zip'
            await self._multipart_upload(
                zip_buffer,
                file_key
            )
            logger.info(f'アップロードファイル: {file_key}')

        if overflow_files:
            overflow_files_log = [
                f'ファイル: {file.key}({file.name})' for file in overflow_files
            ]
            logger.info(f'案件名: {project.name}, 容量オーバーの成果物: {overflow_files_log}')
            return {
                'project_name': project.name,
                'customer_name': project.customer_name,
                'file': [
                    {
                        "name": file.name,
                        "key": file.key
                    }
                    for file in overflow_files
                ]
            }

        return None

    def _get_target_karte(self, project_id: str) -> Optional[ProjectKarteModel]:
        """
          対象のカルテを取得
          （先月の個別カルテで成果物が添付されている最新のカルテ）

          Args:
            project_id: 案件ID
          Returns:
            Optional[ProjectKarteModel]: 対象のカルテ
        """
        # 先月のカルテを降順で取得
        kartes: list[ProjectKarteModel] = list(
            ProjectKarteModel.project_id_start_datetime_index.query(
                project_id,
                range_key_condition=(
                    ProjectKarteModel.start_datetime.between(
                        self._format_datetime(self.first_day_target_month),
                        self._format_datetime(self.last_day_target_month),
                    )
                ),
                scan_index_forward=False
            )
        )
        # 成果物が添付されているカルテが存在すれば返却
        result: Optional[ProjectKarteModel] = None
        for karte in kartes:
            if karte.deliverables and len(karte.deliverables):
                result = karte
                break

        return result


class BundleDeliverables(BaseBundleDeliverables):
    """
        S3に存在する案件ごとの成果物一覧まとめる
    """

    def __init__(
        self,
        event: dict[str, any],
        batch_control_id: str,
        type: DeliverableType,
        target_month: Optional[str] = None
    ):
        super().__init__(event, batch_control_id, type, target_month)

    async def _execute(self) -> list[dict]:
        """
          実行

          Args:
            なし
          Returns:
            なし
        """
        # 案件ごとの成果物一覧のキーを取得
        deliverable_keys: list[str] = self._get_deliverable_keys()
        if deliverable_keys:
            # 案件ごとの成果物一覧をzip化し、アップロード
            result: bool = await self._upload(deliverable_keys)
            if not result:
                return []

            # 案件ごとの成果物一覧を削除
            self._delete_objects(deliverable_keys)

        # エラーになったデータを取得
        overflow_files: Optional[BatchControlErrors] = self._get_overflow_files()
        if overflow_files:
            # エラーメールを送信
            self._send_error_mail(overflow_files)

        return []

    async def _upload(self, deliverable_keys: list[str]) -> bool:
        """
          対象の成果物をまとめてアップロード

          Args:
            deliverable_keys: 対象の案件
          Returns:
            bool: 結果
        """
        # 成果物をすべてzip化

        zip_buffer: Optional[io.BytesIO] = self._create_bundle_zip_file([
            UploadFile(key=key) for key in deliverable_keys
        ])
        if not zip_buffer:
            return None
        logger.info(f'成果物: {deliverable_keys}')

        # 成果物ファイル名
        last_month = self._format_datetime(self.first_day_target_month, '%Y%m')
        current_time = datetime.now().strftime("%Y%m%d%H%M")
        zip_file_name: str = (
            f'{self._create_destination_key(self.type)}/成果物_{last_month}_{current_time}.zip'
        )
        # アップロード
        result: bool = await self._multipart_upload(
            zip_buffer,
            zip_file_name
        )
        logger.info(f'アップロードファイル: {zip_file_name}')
        return result

    def _get_overflow_files(self) -> Optional[BatchControlErrors]:
        """
          エラーになったファイル情報を取得

          Args:
            なし
          Returns:
            Optional[BatchControlErrors]: ファイル情報
        """
        # 案件ごとの成果物アップロードバッチの実行結果を取得
        batch_download_deliverables_project: Optional[MasterBatchControlModel] = (
            self._get_batch_download_deliverables_project_result()
        )
        if not batch_download_deliverables_project:
            return []

        # エラーがあるか確認
        if not batch_download_deliverables_project.errors:
            return []

        errors: list[BatchControlErrors] = [
            error for error in
            batch_download_deliverables_project.errors
            if error.type == BatchErrorType.OVERFLOW_FILES
        ]
        return errors[0] if errors else None

    def _send_error_mail(self, overflow_files: BatchControlErrors):
        """
          アップロードできなかったファイルリストを管理者に通知
          Args:
            なし
          Returns:
            なし
        """
        # 有効なシステム管理者を取得
        admin_list: list[AdminModel] = list(
            AdminModel.scan(
                (AdminModel.roles.contains(UserRoleType.SYSTEM_ADMIN.key))
                & (AdminModel.disabled == False)
            )
        )
        # 送信先を設定
        email_to_list: list[str] = [
            admin.email for admin in admin_list
        ]

        # メール送信
        send_mail(
            template=MailType.BATCH_DOWNLOAD_DELIVERABLES_ERROR_MAIL,
            to_addr_list=email_to_list,
            cc_addr_list=[],
            payload={
                "error_datetime": datetime.now().strftime("%Y/%m/%d %H:%M"),
                "error_files": self._create_mail_body(overflow_files),
            },
        )
        batch_download_deliverables_project: MasterBatchControlModel = self._get_batch_download_deliverables_project_result()
        batch_download_deliverables_project.update(
            actions=[
                MasterBatchControlModel.errors.set(None)
            ]
        )
        logger.info(f'Error email sent: {email_to_list}')
        return

    def _create_mail_body(self, overflow_files: BatchControlErrors) -> str:
        """
          エラーメールの内容を作成

          Args:
            BatchControlErrors: エラーファイルリスト
          Returns:
            str: エラーメールの内容
        """
        body: str = ''
        for details in overflow_files.details:
            body += f'・お客様名: {details.get("customer_name", "")}\n'
            body += f'・案件名: {details.get("project_name", "")}\n'
            body += '・ファイル:\n'
            for file in details.get("file", []):
                body += f'  ・ファイル名：{file.get("name", "")}, パス：{file.get("key", "")}\n'
            body += '\n'
        return body

    def _get_deliverable_keys(self) -> list[str]:
        """
          S3に格納されている成果物のキー一覧を取得

          Args:
            なし
          Returns:
            list[str]: 成果物のキーリスト
        """

        return self._get_s3_object_keys(
            self._create_destination_key(DeliverableType.PROJECT) + "/"
        )

    def _get_batch_download_deliverables_project_result(self) -> Optional[MasterBatchControlModel]:
        """
          案件ごとの成果物アップロードバッチの実行結果を取得
          Args:
            なし
          Returns:
            MasterBatchControlModel: 実行結果
        """
        batch_download_deliverables_project_id: str = BatchFunctionId.DOWNLOAD_DELIVERABLES_PROJECT_BATCH.format(
            landscape=self.event["stageParams"]["stage"]
        )
        update_at_from = datetime.now().replace(hour=0, minute=0, second=0, microsecond=0)
        update_at_to = datetime.now()
        batch_control_list: list[MasterBatchControlModel] = list(
            MasterBatchControlModel.scan(
                (MasterBatchControlModel.update_at.between(update_at_from, update_at_to))
                & (MasterBatchControlModel.value == batch_download_deliverables_project_id)
                & (MasterBatchControlModel.data_type == MasterDataType.BATCH_CONTROL)
            )
        )
        return batch_control_list[0] if batch_control_list else None


def handler(event, context):
    """
    バッチ処理 : BO成果物ダウンロード処理

    Args:
        event:
        context:

    Returns:
        str: 実行結果("Normal end.","Skipped processing.","Error end.")
    """
    try:
        # バッチID, バッチ名, バッチタイプを設定
        if event["deliverableType"] == DeliverableType.PROJECT.value:
            batch_control_id = BatchFunctionId.DOWNLOAD_DELIVERABLES_PROJECT_BATCH.format(
                landscape=event["stageParams"]["stage"]
            )
            batch_control_name = BatchFunctionName.DOWNLOAD_DELIVERABLES_PROJECT_BATCH
            batch_type = DeliverableType.PROJECT
            target_class = BundleProjectDeliverables
        elif event["deliverableType"] == DeliverableType.DELIVERABLE.value:
            batch_control_id = BatchFunctionId.DOWNLOAD_DELIVERABLES_BATCH.format(
                landscape=event["stageParams"]["stage"]
            )
            batch_control_name = BatchFunctionName.DOWNLOAD_DELIVERABLES_BATCH
            batch_type = DeliverableType.DELIVERABLE
            target_class = BundleDeliverables
        else:
            raise Exception("The type is incorrect")

        logger.info(f"Process start: {batch_control_id}")
        asyncio.run(
            target_class(event, batch_control_id, batch_type, event.get("targetMonth")).execute()
        )

        return "Normal end."
    except ExitHandler:
        # 処理の途中で処理終了する場合
        return "Skipped processing."
    except Exception as e:
        logger.error(e)
        error_mail_to = os.getenv("ERROR_MAIL_TO")
        if error_mail_to:
            # 環境変数ERROR_MAIL_TOに宛先が設定されている場合、エラーメール送信
            send_mail(
                template=MailType.BATCH_ERROR_MAIL,
                to_addr_list=[error_mail_to],
                cc_addr_list=[],
                payload={
                    "error_datetime": datetime.now().strftime("%Y/%m/%d %H:%M"),
                    "error_function": batch_control_name,
                    "error_message": traceback.format_exc(),
                },
            )
        return "Error end."
    finally:
        batch_end_common_procedure(
            event=event, batch_status=BatchStatus.ERROR, batch_control_id=batch_control_id
        )
        logger.info("Process End")


# 実行
if __name__ == "__main__":
    param = {
        "stageParams": {"stage": sys.argv[1]},
        "deliverableType": sys.argv[2],
        "targetMonth": sys.argv[3] if sys.argv[3] else None
    }
    handler(param, {})
