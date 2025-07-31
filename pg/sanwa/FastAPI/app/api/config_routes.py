import os
import requests
import logging
from fastapi import APIRouter, Depends
from app.utils.string_utils import read_asset
from app.models.sync_models import RecordCreateRequest, RecordUpdateRequest, RecordDeleteRequest
from app.core.config import settings


# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

router = APIRouter()


@router.get("/get-env")
async def get_env():
    """環境変数の確認エンドポイント"""
    logger.info("環境変数の取得を行います")
    return settings.get_env()


@router.get("/update-site-setting")
async def update_site_setting():
    """サイト設定更新用エンドポイント"""

    # 最初に設定削除をいれるか
    delete_first = True
    # IDいくつまで削除するか
    delete_limit = 30

    # 更新対象の要素
    target_elements = [
        "Styles",
        "Scripts",
        "Htmls",
        "ServerScripts",
        "Processes",
        "StatusControls"
    ]
    headers={'Content-Type':'application/json','charset':'utf-8'}
    pleasanter_url = f'http://{settings.PLEASANTER_HOST}'
    static_dir = os.path.join(settings.APP_ROOT, 'public', 'pleasanter', '00_SiteSettingTool')

    results = {}

    logger.info(f"Pleasanter({pleasanter_url}) のサイト更新を行います")
    for site_name, site_info in settings.PLEASANTER_INFO.items():
        # IDが存在し、かつ値が空でない場合
        if "ID" in site_info and site_info["ID"]:
            site_id = site_info['ID']
        else:
            continue
        api_url = f"{pleasanter_url}/api/items/{site_id}/updatesitesettings"
        update_payload = {}
        delete_payload = {}

        # 更新対象の要素をループして処理
        for element_name, element_settings in site_info.items():

            # 更新対象の場合のみ処理
            if element_name not in target_elements:
                continue

            # 要素の設定にBodyを追加
            for setting in element_settings:
                if element_name == 'Processes':
                    content = read_asset(os.path.join(static_dir, site_name, element_name, setting["Name"]))
                else:
                    content = read_asset(os.path.join(static_dir, site_name, element_name, setting["Title"]))
                
                if content:
                    setting["Body"] = content

            update_payload[element_name] = element_settings
            delete_payload[element_name] = [{"Id": i, "Delete": 1} for i in range(1, delete_limit)]

        # データが空の場合はスキップ
        if update_payload == {}:
            continue

        # フラグがたてたときだけ、サイト更新前削除処理
        if delete_first:
            logger.info(
                f"更新前削除処理開始...  \n"
                f"| タイトル: {site_name} \n"
                f"| URL: {pleasanter_url}/items/{site_id}/index \n"
                f"| 更新設定: {delete_payload} \n"
            )

            delete_payload["ApiVersion"] = settings.PLEASANTER_API_VERSION
            delete_payload["ApiKey"] = settings.PLEASANTER_API_KEY

            response = requests.post(
                api_url,
                headers=headers,
                json=delete_payload
            )

            del delete_payload["ApiVersion"]
            del delete_payload["ApiKey"]

        # サイト更新処理
        logger.info(
            f"更新処理開始...  \n"
            f"| タイトル: {site_name} \n"
            f"| URL: {pleasanter_url}/items/{site_id}/index \n"
            f"| 更新設定: {update_payload} \n"
        )

        update_payload["ApiVersion"] = settings.PLEASANTER_API_VERSION
        update_payload["ApiKey"] = settings.PLEASANTER_API_KEY

        response = requests.post(
            api_url,
            headers=headers,
            json=update_payload
        )

        del update_payload["ApiVersion"]
        del update_payload["ApiKey"]


        status = "success" if response.status_code == 200 else "error"
        results[site_name] = {
            "status": status,
            "status_code": response.status_code,
            "message": response.text,
            "delete_first": delete_first,
            "update_payload": update_payload,
        }

    logger.info(f"更新処理結果... {results} ")

    return results
