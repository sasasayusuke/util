import asyncio
import pytest
import os
import shutil
import boto3
import zipfile
import freezegun
from concurrent.futures import ThreadPoolExecutor
from datetime import datetime, timedelta
from dateutil.relativedelta import relativedelta
from unittest.mock import patch
from moto import mock_s3, mock_dynamodb
from app.models.master import MasterBatchControlModel
from app.models.project import ProjectModel
from app.models.project_karte import ProjectKarteModel
from functions.batch_download_deliverables import (
    handler,
)

bucket_name = 'partnerportal-dev-common-upload'


def get_target_month(target_month=None) -> str:
    today = datetime.now()
    first_day_of_this_month = (
        today.replace(day=1)
        if not target_month
        else datetime.strptime(target_month, '%Y%m') + relativedelta(months=1)
    )
    target_month = first_day_of_this_month - timedelta(days=1)
    return target_month.strftime('%Y%m')


@pytest.fixture
def fixed_time():
    with freezegun.freeze_time("2024-05-01")as frozen_time:
        yield frozen_time


@pytest.fixture
def s3_setup():
    """ S3のモックをセットアップ """
    with mock_s3():
        s3 = boto3.client('s3', region_name='us-east-1')
        s3.create_bucket(Bucket=bucket_name)
        with (
            patch('functions.batch_download_deliverables.BaseBundleDeliverables.s3_client', s3),
        ):
            yield s3


@pytest.fixture
def create_test_file(s3_setup):
    """ テスト対象のファイルをS3にアップロード """
    s3 = s3_setup
    # 一時保存用フォルダ
    tmp_folder_path = '/tmp/test/'

    # フォルダが存在しない場合は作成
    if not os.path.exists(tmp_folder_path):
        os.makedirs(tmp_folder_path, exist_ok=True)

    async def _create_test_file(file_size, num=1, target_month=None):

        # S3アップロード先フォルダ
        current_year = datetime.now().strftime("%Y")
        target_month = get_target_month(target_month)
        s3_folder_path = f"deliverables/{current_year}/{target_month}/"

        def _create_file(i):
            # テスト対象ファイル
            target_file_name = f'test_{i}.txt'
            target_file_file_path = f'{tmp_folder_path}{target_file_name}'
            # zip後のテスト対象ファイル
            zip_file_name = f'output_zip_{i}.zip'
            zip_file_path = f'{tmp_folder_path}{zip_file_name}'

            # テスト用のファイルを作成(100MB未満)
            with open(target_file_file_path, 'wb') as f:
                f.write(bytearray(os.urandom(file_size * 1024 * 1024)))
            # ファイルをZIPに圧縮
            with zipfile.ZipFile(zip_file_path, 'w') as zipf:
                zipf.write(target_file_file_path, arcname=target_file_name)

            # S3のアップロード先
            deliverable_prefix = f"{s3_folder_path}{zip_file_name}"
            # ZIPファイルをS3にアップロード
            with open(zip_file_path, 'rb') as f:
                s3.put_object(Bucket=bucket_name, Key=deliverable_prefix, Body=f)

        tasks = []
        loop = asyncio.get_running_loop()
        with ThreadPoolExecutor(max_workers=5) as executor:
            for i in range(num):
                task = loop.run_in_executor(executor, _create_file, i)
                tasks.append(task)
            await asyncio.gather(*tasks)

    yield _create_test_file
    delete_s3_objects(s3, "deliverables/")
    shutil.rmtree(tmp_folder_path)


def delete_s3_objects(s3, prefix):

    def _delete_s3_objects(next_continuation_token=None):
        # プレフィックスに一致するオブジェクトをリスト
        response = (
            s3.list_objects_v2(Bucket=bucket_name, Prefix=prefix, ContinuationToken=next_continuation_token)
            if next_continuation_token else
            s3.list_objects_v2(Bucket=bucket_name, Prefix=prefix)
        )

        if 'Contents' in response:
            objects_to_delete = [{'Key': obj['Key']} for obj in response['Contents']]
            # オブジェクトを削除
            s3.delete_objects(Bucket=bucket_name, Delete={'Objects': objects_to_delete})

        return response

    response = _delete_s3_objects()
    # 続きがある場合は再帰的に削除
    while response.get('IsTruncated'):
        response = _delete_s3_objects()


@pytest.fixture
def create_test_data():
    """ テストデータ作成 """
    with mock_dynamodb():
        MasterBatchControlModel.create_table(
            read_capacity_units=5, write_capacity_units=5, wait=True
        )
        yield
        MasterBatchControlModel.delete_table()


@pytest.fixture
async def before_upload_under_100MB_ok(fixed_time, create_test_file, create_test_data):
    """
      テストデータ作成
      対象テスト: test_upload_under_100MB_ok
    """
    await create_test_file(
        file_size=50,
        target_month='202401'
    )


@pytest.fixture
async def before_upload_over_100MB_ok(fixed_time, create_test_file, create_test_data):
    """
      テストデータ作成
      対象テスト: test_upload_over_100MB_ok
    """
    await create_test_file(
        file_size=150,
        num=3
    )


@pytest.fixture
async def before_multipart_upload_over_10GB_ok(fixed_time, create_test_file, create_test_data):
    """
      テストデータ作成
      対象テスト: test_multipart_upload_over_10GB_ok
    """
    await create_test_file(
        file_size=100,
        num=110
    )


def test_upload_under_100MB_ok(before_upload_under_100MB_ok):
    """S3に存在するファイルが10GB未満かつ生成したZIPファイルが100MB未満の場合（通常アップロード）のテスト:正常"""
    result = handler({'stageParams': {'stage': 'dev'}, 'deliverableType': 'deliverable', 'targetMonth': '202401'}, {})
    assert result == "Normal end."


def test_upload_over_100MB_ok(before_upload_over_100MB_ok):
    """S3に存在するファイルが10GB未満かつ生成したZIPファイルが100MB以上の場合（マルチパートアップロード）のテスト:正常"""
    result = handler({'stageParams': {'stage': 'dev'}, 'deliverableType': 'deliverable'}, {})
    assert result == "Normal end."


@pytest.mark.skip(reason="メモリが不足するためスキップ")
def test_multipart_upload_over_10GB_ok(before_multipart_upload_over_10GB_ok):
    """S3に存在するファイルが10GB以上の場合（マルチパートアップロード）のテスト:正常"""
    result = handler({'stageParams': {'stage': 'dev'}, 'deliverableType': 'deliverable'}, {})
    assert result == "Normal end."
