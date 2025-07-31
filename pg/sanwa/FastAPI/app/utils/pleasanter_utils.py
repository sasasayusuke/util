import logging
import requests
from app.core.config import settings

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class PleasanterApiClient:
    def __init__(self, base_url=None, api_key=None):
        """
        コンストラクタ
        :param base_url: Pleasanter のベースURL
        :param api_key: APIキー
        """
        self.base_url = base_url or f"http://{settings.PLEASANTER_HOST}"
        self.api_key = api_key or settings.PLEASANTER_API_KEY
        self.api_version = "1.1"

    def get_records(self, site_id, view_params=None, include_deleted=True, use_column_name=True):
        """
        指定したサイトIDから、複数レコードを全件取得(200件を超える場合は繰り返し取得)

        :param site_id: 取得対象テーブル (サイト) の ID
        :param view_params: フィルタ条件 (dict)
        :return: 取得したアイテムのリスト (KeyValues形式)
        """
        if view_params is None:
            view_params = {}

        # 追加パラメータの設定
        if use_column_name:

            view_params["ApiDataType"] = "KeyValues"
            # ColumnName でキー/値を取得したい場合
            view_params["ApiColumnKeyDisplayType"] = "LabelText"
            # 値は "Value" とする (DisplayValue にしたい場合は "ApiColumnValueDisplayType" を "DisplayValue" に変更)
            view_params["ApiColumnValueDisplayType"] = "Value"

        records_all = []
        offset = 0

        while True:
            # 1回分(最大200件)を取得する
            data_part, total_count, page_size = self._get_records_paged(
                site_id=site_id,
                offset=offset,
                view_params=view_params
            )
            # 取得結果を追加
            records_all.extend(data_part)

            # ページング処理
            offset += page_size

            # すべて取得したらループ終了
            if offset >= total_count:
                break


        logger.info(
            f"実行結果: {records_all}\n"
            f"実行メッセージ: API呼び出しが正常に完了し {len(records_all)} 件のデータを取得しました\n"
        )

        return records_all

    def _get_records_paged(self, site_id, offset=0, view_params=None):
        """
        1ページ(最大200件)を取得して返す内部メソッド
        """
        if view_params is None:
            view_params = {}

        url = f"{self.base_url}/api/items/{site_id}/get"
        headers = {
            "Content-Type": "application/json"
        }

        payload = {
            "ApiVersion": self.api_version,
            # ApiKey を使う場合 (ログインセッションで呼ぶなら不要)
            "ApiKey": self.api_key,
            # Offset指定
            "Offset": offset,
            # フィルタ・ソートなどを指定できる View
            "View": view_params
        }

        logger.info(f"PleasanterApiClient POST -> URL: {url}, Offset: {payload['Offset']}, View: {payload['View']}")

        try:
            response = requests.post(url, json=payload, headers=headers)
            response.raise_for_status()  # ステータスコード4xx/5xx で例外
        except requests.exceptions.RequestException as e:
            logger.error(f"PleasanterAPI呼び出しエラー: {str(e)}")
            raise

        resp_json = response.json()
        if "Response" not in resp_json:
            raise ValueError(f"レスポンスの形式が想定外: {resp_json}")

        response_data = resp_json["Response"]
        data_part = response_data.get("Data", [])
        total_count = response_data.get("TotalCount", 0)
        page_size = response_data.get("PageSize", 0)

        return data_part, total_count, page_size

    # def update_site_setting(self, site_id, view_params=None, include_deleted=True, use_column_name=True):