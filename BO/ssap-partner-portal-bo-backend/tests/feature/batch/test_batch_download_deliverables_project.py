import asyncio
import pytest
import shutil
import boto3
import os
import freezegun
from moto import mock_dynamodb
from concurrent.futures import ThreadPoolExecutor
from unittest.mock import patch
from moto import mock_s3
from datetime import datetime, timedelta
from dateutil.relativedelta import relativedelta
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
        with patch('functions.batch_download_deliverables.BaseBundleDeliverables.s3_client', s3):
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

    async def _create_test_file(project_id, karte_id, file_size, num=1, target_month=None):

        target_month = get_target_month(target_month)

        def _create_file(i):
            # S3アップロード先フォルダ
            s3_folder_path = f"project/{project_id}/karte/{karte_id}/deliverables/"
            # テスト対象ファイル
            date_num = i + 1
            date = str(date_num).zfill(2) if date_num <= 30 else str(date_num % 30).zfill(2)
            target_file_name = f"{target_month}{date}-00000000{i}.txt"
            target_file_path = f'{tmp_folder_path}{target_file_name}'

            # テスト用のファイルを作成(100MB未満)
            with open(target_file_path, 'wb') as f:
                f.write(bytearray(os.urandom(file_size * 1024 * 1024)))

            # S3のアップロード先
            deliverable_prefix = f"{s3_folder_path}{target_file_name}"
            # ZIPファイルをS3にアップロード
            with open(target_file_path, 'rb') as f:
                s3.put_object(Bucket=bucket_name, Key=deliverable_prefix, Body=f)

        tasks = []
        loop = asyncio.get_running_loop()
        with ThreadPoolExecutor(max_workers=5) as executor:
            for i in range(num):
                task = loop.run_in_executor(executor, _create_file, i)
                tasks.append(task)
            await asyncio.gather(*tasks)
        return s3

    yield _create_test_file
    delete_s3_objects(s3, "project/")
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

    def _create_test_data(file_keys, target_month=None):
        """ テストデータ作成 """
        with mock_dynamodb():
            MasterBatchControlModel.create_table(
                read_capacity_units=5, write_capacity_units=5, wait=True
            )
            ProjectModel.create_table(
                read_capacity_units=5, write_capacity_units=5, wait=True
            )
            ProjectKarteModel.create_table(
                read_capacity_units=5, write_capacity_units=5, wait=True
            )

            support_date_from = (
                (datetime.now().replace(day=1) - timedelta(days=1)).strftime("%Y/%m/%d")
                if not target_month else
                datetime.strptime(target_month, '%Y%m').strftime("%Y/%m/%d")
            )

            data = {}
            data["project"] = ProjectModel(
                id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                data_type="project",
                customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                customer_name="SGC",
                name="案件1",
                main_sales_user_id="main_sales_user_id",
                support_date_from=support_date_from,
                support_date_to="2024/09/30",
                total_contract_time=200,
                main_customer_user_id="b9b67094-cdab-494c-818e-d4845088269b",
                is_karte_remind=True,
                is_secret=False,
            ).save()

            data["project_karte"] = ProjectKarteModel(
                karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
                project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                customer_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
                date=support_date_from,
                start_datetime=f"{support_date_from} 00:00",
                man_hour=10,
                is_draft=True,
                deliverables=[
                    {
                        "file_name": "test.png",
                        "path": key
                    }
                    for key in file_keys
                ]
            ).save()
            return data

    yield _create_test_data
    MasterBatchControlModel.delete_table()
    ProjectModel.delete_table()
    ProjectKarteModel.delete_table()


@pytest.fixture
async def before_download_and_upload_ok(fixed_time, create_test_file, create_test_data):
    """
      テストデータ作成
      対象テスト: test_udownload_and_upload_ok
    """
    s3 = await create_test_file(
        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
        file_size=50,
        num=2,
        target_month='202401'
    )
    result = s3.list_objects_v2(Bucket=bucket_name, Prefix="project/")
    create_test_data([obj['Key'] for obj in result['Contents']], target_month='202401')
    return s3


@pytest.fixture
async def before_multipart_upload_ok(fixed_time, create_test_file, create_test_data):
    """
      テストデータ作成
      対象テスト: test_udownload_and_upload_ok
    """
    s3 = await create_test_file(
        project_id="89cbe2ed-f44c-4a1c-9408-c67b0ca2270d",
        karte_id="7ac8bddf-88da-46c9-a504-a03d1661ad58",
        file_size=100,
    )
    result = s3.list_objects_v2(Bucket=bucket_name, Prefix="project/")
    create_test_data([obj['Key'] for obj in result['Contents']])
    return s3


def test_download_and_upload_ok(before_download_and_upload_ok):
    """生成したZIPファイルが100MB未満の場合（通常アップロード）のテスト:正常"""
    result = handler({'stageParams': {'stage': 'dev'}, 'deliverableType': 'project', 'targetMonth': '202401'}, {})
    assert result == "Normal end."


def test_multipart_upload_ok(before_multipart_upload_ok):
    """生成したZIPファイルが100MB以上の場合（マルチパートアップロード）のテスト:正常"""
    result = handler({'stageParams': {'stage': 'dev'}, 'deliverableType': 'project'}, {})
    assert result == "Normal end."
