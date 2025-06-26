import json
import re
import urllib.request

from botocore.auth import SigV4Auth
from botocore.awsrequest import AWSRequest
from botocore.credentials import Credentials

from app.core.config import get_app_settings
from functions.batch_access_platform import get_secret


class PlatformApiOperator:
    # TODO: JWTTokenのヘッダーの設定 IAMのクレデンシャルから設定
    def __init__(self, jwt_token: str = None, is_batch: bool = False):
        self.pf_api_base_url = get_app_settings().pf_api_base_url

        if not is_batch:
            self.headers = {
                "Authorization": f"Bearer {jwt_token}",
                "serviceName": "partner-portal-bo",
                "Content-Type": "application/json",
            }
        else:
            values = get_secret()
            credentials = Credentials(
                values["access_key_id"], values["secret_access_key"]
            )
            aws_request = AWSRequest(
                method="GET", url=get_app_settings().pf_api_base_url
            )
            SigV4Auth(credentials, "execute-api", "ap-northeast-1").add_auth(
                aws_request
            )
            self.headers = {
                "Authorization": aws_request.headers["Authorization"],
                "serviceName": "partner-portal-batch",
                "Content-Type": "application/json",
            }

    def convert_to_camel_case(self, params):
        """スネークケースをキャメルケースに変換する"""
        keys = params.copy().keys()
        for key in keys:
            value = params[key]
            del params[key]
            camel_key = re.sub("_(.)", lambda x: x.group(1).upper(), key)
            params[camel_key] = value
        return params

    def get_projects(self, params):
        """プロジェクト一覧取得 API"""
        query_string = urllib.parse.urlencode(params)
        url = f"{self.pf_api_base_url}/projects"
        req = urllib.request.Request(
            url="{}?{}".format(url, query_string),
            headers=self.headers,
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def get_project_by_pf_id(self, pf_project_id):
        """プロジェクト詳細取得 API"""
        url = f"{self.pf_api_base_url}/projects/{pf_project_id}"
        req = urllib.request.Request(url=url, headers=self.headers)
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def get_code_master(self):
        """コードマスタ取得 API"""

        url = f"{self.pf_api_base_url}/params"
        req = urllib.request.Request(url=url, headers=self.headers)
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def create_project(self, params):
        """プロジェクト登録 API"""
        url = f"{self.pf_api_base_url}/project"
        req = urllib.request.Request(
            url=url,
            headers=self.headers,
            data=json.dumps(params, default=str).encode(),
            method="POST",
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def update_project_by_pf_id(self, project_id, params):
        """プロジェクト更新 API"""
        url = f"{self.pf_api_base_url}/projects/{project_id}"
        req = urllib.request.Request(
            url=url,
            headers=self.headers,
            data=json.dumps(params, default=str).encode(),
            method="PUT",
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def delete_project_by_pf_id(self, pf_project_id, version):
        """プロジェクト削除 API"""
        url = f"{self.pf_api_base_url}/projects/{pf_project_id}/{version}"
        req = urllib.request.Request(url=url, headers=self.headers, method="DELETE")
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def put_project_publication(self, params):
        """プロジェクト公開およびCSV取り込み状態変更 API"""
        url = f"{self.pf_api_base_url}/project/publication"
        req = urllib.request.Request(
            url=url,
            headers=self.headers,
            data=json.dumps(params, default=str).encode(),
            method="PUT",
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def create_program(self, params):
        """プロジェクト公開およびCSV取り込み状態変更 API"""
        url = f"{self.pf_api_base_url}/program"
        req = urllib.request.Request(
            url=url,
            headers=self.headers,
            data=json.dumps(params, default=str).encode(),
            method="POST",
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body

    def create_customer(self, params):
        """取引先登録 API"""
        url = f"{self.pf_api_base_url}/customer"
        req = urllib.request.Request(
            url=url,
            headers=self.headers,
            data=json.dumps(params, default=str).encode(),
            method="POST",
        )
        try:
            with urllib.request.urlopen(req) as res:
                body = json.loads(res.read())
                status_code = res.getcode()
        except urllib.error.HTTPError as e:
            body = json.loads(e.read())
            status_code = e.getcode()
        return status_code, body
