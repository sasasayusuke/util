from fastapi import HTTPException, status
from typing import List, Union

from app.auth.auth import AuthUser
from app.core.common_logging import CustomLogger
from app.models.master import (
    MasterBatchControlModel,
    MasterSupporterOrganizationModel,
    MasterSelectItems,
)
from app.resources.const import MasterDataType
from app.schemas.master import (
    GetBatchControlResponse,
    GetServiceTypesResponse,
    GetSupporterOrganizationsResponse,
    GetSupporterOrganizationsResponseItems,
    ServiceTypes,
    GetSelectItemsResponse,
    SelectItems,
)

logger = CustomLogger.get_logger()


class MasterService:
    @staticmethod
    def get_service_types(
        current_user: AuthUser, disabled: bool
    ) -> GetServiceTypesResponse:
        """サービス種別を取得する

        Args:
            current_user (AuthUser): 認証済みのユーザー
            disabled (bool): 無効データ含む
        Returns:
            GetServiceTypesResponse: 取得結果
        """
        service_types: GetServiceTypesResponse = []

        # Modelのqueryでflake8(E712)エラーが出るため定義
        bool_true: bool = True
        filter_condition = None
        if not disabled:
            filter_condition &= MasterSupporterOrganizationModel.use == bool_true

        master_iter = MasterSupporterOrganizationModel.data_type_order_index.query(
            hash_key=MasterDataType.MASTER_SERVICE_TYPE.value,
            filter_condition=filter_condition,
        )

        for master in master_iter:
            master = ServiceTypes(**master.attribute_values)
            service_types.append(master)

        return GetServiceTypesResponse(service_types=service_types)

    @staticmethod
    def get_batch_control_by_id(
        batch_id: str, current_user: AuthUser
    ) -> GetBatchControlResponse:
        """Get /masters/batch-control/{id} バッチ処理最終完了日時取得API
        Args:
            batch_id (str): バッチ処理関数ID パスパラメータで指定

        Raises:
            HTTPException: 404 Not Found

        Returns:
            GetBatchControlResponse: 取得結果
        """

        try:
            range_key_condition = MasterBatchControlModel.value == batch_id
            master = next(
                MasterBatchControlModel.data_type_value_index.query(
                    hash_key=MasterDataType.BATCH_CONTROL.value,
                    range_key_condition=range_key_condition,
                )
            )

        except StopIteration:
            logger.warning(f"GetBatchControl not found. batch_id: {batch_id}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        # batch_end_at の値がNoneのときはModelクラスで許容していないため、考慮しない
        return GetBatchControlResponse(batch_end_at=master.attributes.batch_end_at)

    @staticmethod
    def get_supporter_organizations(
        current_user: AuthUser, disabled: bool
    ) -> GetSupporterOrganizationsResponse:
        """支援者組織の一覧を取得する

        Args:
            current_user (AuthUser): 認証済みのユーザー
            disabled (bool): 無効データ含む
        Returns:
            GetSupporterOrganizationResponse: 取得結果
        """
        supporter_organizations = []

        # Modelのqueryでflake8(E712)エラーが出るため定義
        bool_true: bool = True
        filter_condition = None
        if not disabled:
            filter_condition &= MasterSupporterOrganizationModel.use == bool_true

        master_iter = MasterSupporterOrganizationModel.data_type_order_index.query(
            hash_key=MasterDataType.MASTER_SUPPORTER_ORGANIZATION.value,
            filter_condition=filter_condition,
        )

        for master in master_iter:
            supporter_organizations.append(
                GetSupporterOrganizationsResponseItems(
                    id=master.id, name=master.name, short_name=master.value
                )
            )

        return GetSupporterOrganizationsResponse(
            supporter_organizations=supporter_organizations
        )

    @staticmethod
    def get_select_items(
        current_user: AuthUser, data_type: str
    ) -> GetSelectItemsResponse:
        """セレクトの選択肢を取得する

        Args:
            current_user (AuthUser): 認証済みのユーザー
            data_type (str): データ区分
        Returns:
            GetSelectItemsResponse: 取得結果
        """
        masters: GetSelectItemsResponse = []

        # Modelのqueryでflake8(E712)エラーが出るため定義
        bool_true: bool = True
        filter_condition = None
        # 常にuse（利用フラグ）が有効のデータをDBから取得
        filter_condition &= MasterSelectItems.use == bool_true

        if data_type == "issue_map50":
            master_iter = MasterSelectItems.data_type_order_index.query(
                hash_key=data_type,
                filter_condition=filter_condition,
            )
        else:
            master_iter = MasterSelectItems.data_type_index.query(
                hash_key=data_type,
                filter_condition=filter_condition,
            )

        for master in master_iter:
            master = SelectItems(**master.attribute_values)
            masters.append(master)

        return GetSelectItemsResponse(masters=masters)

    @staticmethod
    def get_issue_map50_name(
        issue_map50_id_list: List[str]
    ) -> Union[List[str], None, HTTPException]:
        """汎用マスタから課題を取得.
            取得できない場合はNoneを発行.
            issue_map50_idがNoneまたは空の場合、Noneを返却.

        Args:
            issue_map50_id (List[str]): 課題マップ50のIDのリスト

        Returns:
            List[str]: 課題のリスト
        """

        if not issue_map50_id_list:
            return None

        issue_map50_name_list: List[str] = []
        item_keys = [(id, MasterDataType.ISSUE_MAP50.value) for id in issue_map50_id_list]
        for item in MasterSelectItems.batch_get(item_keys):
            issue_map50_name_list.append(item.name)

        return issue_map50_name_list

    @staticmethod
    def get_tsi_areas_name(
        tsi_areas_id_list: List[str]
    ) -> Union[List[str], None, HTTPException]:
        """汎用マスタから東証33業種経験/対応可能領域名を取得.
            取得できない場合はNoneを発行.
            tsi_areas_id_listがNoneまたは空の場合、Noneを返却.

        Args:
            tsi_areas_id_list (List[str]): 東証33業種経験/対応可能領域IDのリスト

        Returns:
            List[str]: 東証33業種経験/対応可能領域名のリスト
        """

        if not tsi_areas_id_list:
            return None

        tsi_areas_name_list: List[str] = []
        item_keys = [(id, MasterDataType.INDUSTRY_SEGMENT.value) for id in tsi_areas_id_list]
        for item in MasterSelectItems.batch_get(item_keys):
            tsi_areas_name_list.append(item.name)

        return tsi_areas_name_list
