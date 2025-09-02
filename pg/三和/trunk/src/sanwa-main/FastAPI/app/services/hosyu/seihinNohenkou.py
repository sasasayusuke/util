from typing import Dict, Any
import logging
from app.services.base_service import BaseService
from app.core.config import settings
from fastapi import HTTPException
from fastapi.responses import StreamingResponse
from fastapi.responses import JSONResponse
from app.utils.db_utils import SQLExecutor
from app.core.exceptions import ServiceError
import json
import requests

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class SeihinNohenkouService(BaseService):

    def display(self, request, session) -> Dict[str, Any]:
        """印刷処理を行うメソッド"""
        pass

    def execute(self, request, session) -> Dict[str, Any]:
        """実行処理を行うメソッド"""
        pass

    def update(self, request, session) -> StreamingResponse:
        """製品No変更処理(index)"""
        logger.info("【START】製品No変更")

        # SDBの製品No変更処理を実行
        self.process_product_change(request, session)

        # PleasanterのNo変更処理を実行
        self.process_product_change_pleasanter(request, session)

        return JSONResponse(
            content={},
            headers={
                'Download-Skip': "yes"
            }
        )

    def process_product_change(self, request, session):
        """製品No変更処理をDBで実行"""
        # 書式区分に応じてストアドプロシージャ名を設定
        stored_name = "usp_HJ0300コード変更" if request.params["書式区分"] == "0" else "usp_HJ0301製品コード統合"

        # ストアドプロシージャのパラメータ
        params = {
            "@i製品NO_OLD": request.params["変更前製品No"],
            "@i仕様NO_OLD": request.params["変更前仕様No"],
            "@i製品NO_NEW": request.params["変更後製品No"],
            "@i仕様NO_NEW": request.params["変更後仕様No"],
        }

        # ストアプロシージャを実行
        storedresults = dict(SQLExecutor(session).execute_stored_procedure(
            storedname=stored_name, params=params, outputparams={"@RetST": "INT", "@RetMsg": "VARCHAR(255)"}
        ))

        # 結果が-1の場合、エラー
        if storedresults["output_values"]["@RetST"] == -1:
            raise ServiceError(storedresults["output_values"]["@RetMsg"])

    def process_product_change_pleasanter(self, request, session):
        """製品No変更処理をPleasanterで実行"""

        headers = {'Content-Type':'application/json','charset':'utf-8'}

        ### 製品マスタ ###
        # 書式区分が0の時は変更前製品No、仕様Noをキーに、1の時は変更後製品No、仕様Noをキーに製品名を取得する
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{settings.PLEASANTER_INFO['製品マスタ']['ID']}/get"
        if request.params["書式区分"] == "0":
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "View":{
                    "ColumnFilterHash":{
                        "ClassA":request.params["変更前製品No"],
                        "ClassB":request.params["変更前仕様No"]
                    }
                }
            }
        else:
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "View":{
                    "ColumnFilterHash":{
                        "ClassA":request.params["変更後製品No"],
                        "ClassB":request.params["変更後仕様No"]
                    }
                }
            }
        response = requests.post(url, headers=headers, json=body)
        seihin_name = response.json()['Response']['Data'][0]['ClassHash']['ClassC']

        # 製品マスタから該当のレコードIDを取得
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{settings.PLEASANTER_INFO['製品マスタ']['ID']}/get"
        body = {
            "ApiVersion":settings.PLEASANTER_API_VERSION,
            "ApiKey":settings.PLEASANTER_API_KEY,
            "View":{
                "ColumnFilterHash":{
                    "ClassA":request.params["変更前製品No"],
                    "ClassB":request.params["変更前仕様No"]
                }
            }
        }
        response = requests.post(url, headers=headers, json=body)
        seihin_record_id = response.json()['Response']['Data'][0]['ResultId']

        # 製品マスタの該当レコードを更新
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{seihin_record_id}/update"
        # 書式区分が0の時は製品No、仕様Noを更新、1の時は廃番flgをTrueにする
        if request.params["書式区分"] == "0":
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "ItemTitle":request.params["変更後製品No"],
                "ClassHash":{
                    "ClassA":request.params["変更後製品No"],
                    "ClassB":request.params["変更後仕様No"],
                    "Class100":request.params["変更後製品No"] + '__' + request.params["変更後仕様No"]
                }
            }
        else:
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "CheckHash":{
                    "CheckA":"True"
                }
            }
        response = requests.post(url, headers=headers, json=body)

        ### 棚番マスタ ###
        # 棚番マスタから該当のレコードIDを取得
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{settings.PLEASANTER_INFO['棚番マスタ']['ID']}/get"
        body = {
            "ApiVersion":settings.PLEASANTER_API_VERSION,
            "ApiKey":settings.PLEASANTER_API_KEY,
            "View":{
                "ColumnFilterHash":{
                    "ClassG":request.params["変更前製品No"],
                    "ClassB":request.params["変更前仕様No"]
                }
            }
        }
        response = requests.post(url, headers=headers, json=body)

        # 棚番マスタの該当レコードを更新
        for rec_data in response.json()['Response']['Data']:
            temp_record_id = rec_data['ResultId']
            url = f"http://{settings.PLEASANTER_HOST}/api/items/{temp_record_id}/update"
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "ClassHash":{
                    "ClassG":request.params["変更後製品No"],
                    "ClassB":request.params["変更後仕様No"],
                    "ClassC":seihin_name
                }
            }
            response = requests.post(url, headers=headers, json=body)

        ### 売上仕入単価マスタ ###
        # 売上仕入単価マスタから該当のレコードIDを取得
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{settings.PLEASANTER_INFO['売上仕入単価マスタ']['ID']}/get"
        body = {
            "ApiVersion":settings.PLEASANTER_API_VERSION,
            "ApiKey":settings.PLEASANTER_API_KEY,
            "View":{
                "ColumnFilterHash":{
                    "ClassJ":request.params["変更前製品No"],
                    "ClassG":request.params["変更前仕様No"]
                }
            }
        }
        response = requests.post(url, headers=headers, json=body)

        # 売上仕入単価マスタの該当レコードを更新
        for rec_data in response.json()['Response']['Data']:
            temp_record_id = rec_data['ResultId']
            # 得意先CD、仕入先CDを取得(検索ダイアログ項目のためApiDataTypeをKeyValuesに設定して取得が必要)
            data_type_url = f"http://{settings.PLEASANTER_HOST}/api/items/{temp_record_id}/get"
            data_type_body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "View":{
                    "ApiDataType": "KeyValues"
                }
            }
            response = requests.post(data_type_url, headers=headers, json=data_type_body)
            tmp_tokui_cd = response.json()['Response']['Data'][0]['得意先CD']
            tmp_siire_cd = response.json()['Response']['Data'][0]['仕入先CD']

            # 売上仕入単価マスタの該当レコードを更新
            url = f"http://{settings.PLEASANTER_HOST}/api/items/{temp_record_id}/update"
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "ClassHash":{
                    "ClassJ":request.params["変更後製品No"],
                    "ClassG":request.params["変更後仕様No"],
                    "ClassH":seihin_name,
                    "Class100":tmp_tokui_cd + '__' + tmp_siire_cd + '__' + rec_data['ClassHash']['ClassE'] + '__' + request.params["変更後製品No"] + '__' + request.params["変更後仕様No"]
                }
            }
            response = requests.post(url, headers=headers, json=body)

        ### ヤオコー製品変換マスタ ###
        # ヤオコー製品変換マスタから該当のレコードIDを取得
        url = f"http://{settings.PLEASANTER_HOST}/api/items/{settings.PLEASANTER_INFO['ヤオコー製品変換マスタ']['ID']}/get"
        body = {
            "ApiVersion":settings.PLEASANTER_API_VERSION,
            "ApiKey":settings.PLEASANTER_API_KEY,
            "View":{
                "ColumnFilterHash":{
                    "ClassI":request.params["変更前製品No"],
                    "ClassD":request.params["変更前仕様No"]
                }
            }
        }
        response = requests.post(url, headers=headers, json=body)

        # ヤオコー製品変換マスタの該当レコードを更新
        for rec_data in response.json()['Response']['Data']:
            temp_record_id = rec_data['ResultId']
            # 得意先CDを取得(検索ダイアログ項目のためApiDataTypeをKeyValuesに設定して取得が必要)
            data_type_url = f"http://{settings.PLEASANTER_HOST}/api/items/{temp_record_id}/get"
            data_type_body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "View":{
                    "ApiDataType": "KeyValues"
                }
            }
            response = requests.post(data_type_url, headers=headers, json=data_type_body)
            tmp_tokui_cd = response.json()['Response']['Data'][0]['得意先CD']

            # 売上仕入単価マスタの該当レコードを更新
            url = f"http://{settings.PLEASANTER_HOST}/api/items/{temp_record_id}/update"
            body = {
                "ApiVersion":settings.PLEASANTER_API_VERSION,
                "ApiKey":settings.PLEASANTER_API_KEY,
                "ClassHash":{
                    "ClassI":request.params["変更後製品No"],
                    "ClassD":request.params["変更後仕様No"],
                    "ClassE":seihin_name,
                    "Class100":tmp_tokui_cd + '__' + request.params["変更後製品No"] + '__' + request.params["変更後仕様No"] + '__' + rec_data['ClassHash']['ClassG']
                }
            }
            response = requests.post(url, headers=headers, json=body)

    def critical_alarm(self, message: str):
        """エラーメッセージログ"""
        logger.error(message)
        raise ServiceError(message, status_code=400)
