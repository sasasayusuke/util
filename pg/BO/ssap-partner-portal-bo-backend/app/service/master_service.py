import uuid
from datetime import datetime
from typing import List

from fastapi import HTTPException, status

from app.core.common_logging import CustomLogger
from app.models.admin import AdminModel
from app.models.master import (
    MasterBatchControlModel,
    MasterSupporterOrganizationModel,
    SupporterOrganizationAttribute,
)
from app.models.master_alert_setting import (
    AlertSettingAttribute as AlertSettingAttributeOfModel,
)
from app.models.master_alert_setting import (
    FactorSettingSubAttribute,
    MasterAlertSettingModel,
)
from app.resources.const import MasterDataType, QueryParameterForMasterDataType
from app.schemas.master import (
    AlertSettingAttribute,
    CreateMasterRequest,
    CreateMasterRespose,
    FactorSetting,
    GetAlertSettingResponse,
    GetBatchControlResponse,
    GetMasterByIdResponse,
    GetMastersMasterResponse,
    GetMastersResponse,
    GetServiceTypesResponse,
    GetSupporterOrganizationsResponse,
    GetSupporterOrganizationsResponseItems,
    ServiceTypes,
    UpdateMasterByIdRequest,
    UpdateMasterByIdResponse,
)
from app.service.common_service.pagination import EmptyPage, Paginator

logger = CustomLogger.get_logger()


class MasterService:
    @staticmethod
    def decompress_factor_setting(
        item: List[FactorSettingSubAttribute], type: str = "schema"
    ):
        if type == "schema":
            factor_settings: List[FactorSetting] = []
            for factor_setting in item:
                factor_settings.append(
                    FactorSetting(
                        service_type_id=factor_setting.service_type_id,
                        direct_support_man_hour=round(
                            factor_setting.direct_support_man_hour, 2
                        ),
                        direct_and_pre_support_man_hour=round(
                            factor_setting.direct_and_pre_support_man_hour, 2
                        ),
                        pre_support_man_hour=factor_setting.pre_support_man_hour,
                        hourly_man_hour_price=factor_setting.hourly_man_hour_price,
                        monthly_profit=factor_setting.monthly_profit,
                    )
                )
        elif type == "model":
            factor_settings: List[FactorSettingSubAttribute] = []
            for factor_setting in item:
                factor_settings.append(
                    FactorSettingSubAttribute(
                        service_type_id=factor_setting.service_type_id,
                        direct_support_man_hour=round(
                            factor_setting.direct_support_man_hour, 2
                        ),
                        direct_and_pre_support_man_hour=round(
                            factor_setting.direct_and_pre_support_man_hour, 2
                        ),
                        pre_support_man_hour=factor_setting.pre_support_man_hour,
                        hourly_man_hour_price=factor_setting.hourly_man_hour_price,
                        monthly_profit=factor_setting.monthly_profit,
                    )
                )

        return factor_settings

    @staticmethod
    def create_master(
        item: CreateMasterRequest, current_user: AdminModel
    ) -> CreateMasterRespose:
        """マスターの登録を行う

        Args:
            item (CreateMasterRequest): 登録内容
            current_user (AdminModel): 認証済みユーザー

        Raises:
            HTTPException: 400

        Returns:
            CreateMasterRespose: 登録結果
        """
        if MasterSupporterOrganizationModel.is_duplicate_data_type_and_value(
            data_type=item.data_type, value=item.value
        ):
            logger.warning("CreateMaster. data type and value is already exist.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="data type and value is already exist.",
            )

        # 種別の中で順番（order）を登録するために登録件数を取得
        # 最新のデータを取得するは複数取得してきてその中で制御する必要があるため種別の登録件数で対応
        latest_registration_number = (
            MasterSupporterOrganizationModel.get_same_data_type_registrations_count(
                data_type=item.data_type
            )
        )

        create_datetime = datetime.now()
        master_supporter_organization_model = MasterSupporterOrganizationModel(
            id=str(uuid.uuid4()),
            data_type=item.data_type,
            name=item.name,
            value=item.value,
            attributes=SupporterOrganizationAttribute(
                info1=item.attributes.info1,
                info2=item.attributes.info2,
                info3=item.attributes.info3,
                info4=item.attributes.info4,
                info5=item.attributes.info5,
            ),
            order=latest_registration_number + 1,
            use=item.use,
            create_id=current_user.id,
            create_at=create_datetime,
            update_id=current_user.id,
            update_at=create_datetime,
        )
        master_supporter_organization_model.save()

        return CreateMasterRespose(
            **master_supporter_organization_model.attribute_values
        )

    @staticmethod
    def get_masters(
        dataType: QueryParameterForMasterDataType,
        limit: int,
        offsetPage: int,
        current_user: AdminModel,
    ) -> GetMastersResponse:
        """マスターを一覧取得する

        Args:
            name (str): 名称
            dataType (Enum): データ区分（すべて、支援者組織情報、サービス種別）
            limit (int): 最大取得件数
            offsetPage (int): リストの中で何ページ目を取得するか
            current_user (AdminModel): 認証済みユーザー
        Raises:
            HTTPException: 404 Not found

        Returns:
            GetMastersResponse: 取得結果
        """

        # クエリ条件を指定
        condition = None

        # 抽出条件を指定
        if dataType == QueryParameterForMasterDataType.ALL:
            # master_ から始まるデータ区分のアイテムをすべて取得
            condition &= MasterSupporterOrganizationModel.data_type.startswith(
                "master_"
            )

            # scanするとプライマリキーの降順で返却されるため昇順で格納
            master_iter = reversed(
                list(
                    MasterSupporterOrganizationModel.data_type_index.scan(
                        filter_condition=condition,
                    )
                )
            )
        else:
            master_iter = MasterSupporterOrganizationModel.data_type_index.query(
                hash_key=dataType.value,
                filter_condition=condition,
                scan_index_forward=True,
            )

        # クエリ結果をリスト変数へ格納
        masters_list: List[GetMastersMasterResponse] = []
        for master in master_iter:
            masters_list.append(GetMastersMasterResponse(**master.attribute_values))

        # 指定したアイテム数とページ位置のユーザーを取得
        try:
            p = Paginator(masters_list, limit)
            current_page = p.page(offsetPage).object_list
        except EmptyPage:
            logger.warning(f"GetMasters not found. offset_page:{offsetPage}")
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND)

        return GetMastersResponse(
            offsetPage=offsetPage, total=len(masters_list), masters=current_page
        )

    @staticmethod
    def get_master_by_id(
        master_id: str, current_user: AdminModel
    ) -> GetMasterByIdResponse:
        """マスターIDを指定してマスターデータを取得する

        Args:
            master_id (str): マスターID
            current_user (AdminModel): 認証済みユーザー
        Raises:
            HTTPException: 404 Not found

        Returns:
            GetMasterByIdResponse: 取得結果
        """
        masters = MasterSupporterOrganizationModel.query(hash_key=master_id)
        for master in masters:
            return GetMasterByIdResponse(**master.attribute_values)
        else:
            logger.warning(f"GetMasterById not found. master_id:{master_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

    @staticmethod
    def update_master_by_id(
        master_id: str,
        version: int,
        item: UpdateMasterByIdRequest,
        current_user: AdminModel,
    ):
        """マスターIDを指定してマスターデータを更新する

        Args:
            master_id (str): マスターID
            version (int): 楽観ロックチェックバージョン
            item (UpdateMasterByIdRequest): 更新内容
            current_user (AdminModel): 認証済みユーザー

        Raises:
            HTTPException: 404 Not found
            HTTPException: 409 Conflict

        Returns:
            _type_: _description_
        """

        masters = MasterSupporterOrganizationModel.query(hash_key=master_id)
        for master in masters:
            break
        else:
            logger.warning(f"UpdateMasterById not found. master_id:{master_id}")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        default_count = 0
        if master.value == item.value:
            default_count = -1

        if MasterSupporterOrganizationModel.is_duplicate_data_type_and_value(
            data_type=master.data_type, value=item.value, count=default_count
        ):
            logger.warning("UpdateMasterById. Data type and value is already exist.")
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Data type and value is already exist.",
            )

        # 楽観ロックチェック
        if version != master.version:
            logger.warning(
                f"UpdateMasterById conflict. request_ver:{version} master_ver: {master.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        master.update(
            actions=[
                MasterSupporterOrganizationModel.name.set(item.name),
                MasterSupporterOrganizationModel.value.set(item.value),
                MasterSupporterOrganizationModel.attributes.info1.set(
                    item.attributes.info1
                ),
                MasterSupporterOrganizationModel.attributes.info2.set(
                    item.attributes.info2
                ),
                MasterSupporterOrganizationModel.attributes.info3.set(
                    item.attributes.info3
                ),
                MasterSupporterOrganizationModel.attributes.info4.set(
                    item.attributes.info4
                ),
                MasterSupporterOrganizationModel.attributes.info5.set(
                    item.attributes.info5
                ),
                MasterSupporterOrganizationModel.use.set(item.use),
                MasterSupporterOrganizationModel.update_id.set(current_user.id),
                MasterSupporterOrganizationModel.update_at.set(datetime.now()),
            ]
        )

        return UpdateMasterByIdResponse(**master.attribute_values)

    @staticmethod
    def get_service_types(disabled: bool) -> GetServiceTypesResponse:
        """サービス種別の一覧を取得する

        Args:
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
        batch_id: str, current_user: AdminModel
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
        disabled: bool,
    ) -> GetSupporterOrganizationsResponse:
        """支援者組織の一覧を取得する

        Args:
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
    def get_alert_setting() -> GetAlertSettingResponse:
        try:
            alert_setting_iter = MasterAlertSettingModel.data_type_name_index.query(
                hash_key=MasterDataType.ALERT_SETTING.value,
            )
            alert_setting = next(alert_setting_iter)
        except StopIteration:
            logger.warning("GetAlertSetting not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        return GetAlertSettingResponse(
            id=alert_setting.id,
            name=alert_setting.name,
            # display_settingは必要になったタイミングで追加予定
            attributes=AlertSettingAttribute(
                factor_setting=MasterService.decompress_factor_setting(
                    alert_setting.attributes.factor_setting
                )
            ),
            create_id=alert_setting.create_id,
            create_at=alert_setting.create_at,
            update_id=alert_setting.update_id,
            update_at=alert_setting.update_at,
            version=alert_setting.version,
        )

    @staticmethod
    def update_alert_setting(
        item: AlertSettingAttribute, version: int, current_user: AdminModel
    ) -> GetAlertSettingResponse:
        try:
            alert_setting_iter = MasterAlertSettingModel.data_type_name_index.query(
                hash_key=MasterDataType.ALERT_SETTING.value,
            )
            alert_setting = next(alert_setting_iter)
        except StopIteration:
            logger.warning("UpdateAlertSetting not found.")
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Not found"
            )

        if version != alert_setting.version:
            logger.warning(
                f"UpdateAlertSetting conflict. request_ver:{version} alert_setting_ver: {alert_setting.version}"
            )
            raise HTTPException(status_code=status.HTTP_409_CONFLICT, detail="Conflict")

        alert_setting.update(
            actions=[
                MasterAlertSettingModel.attributes.set(
                    AlertSettingAttributeOfModel(
                        factor_setting=MasterService.decompress_factor_setting(
                            item.factor_setting, type="model"
                        )
                    )
                ),
                MasterAlertSettingModel.update_id.set(current_user.id),
                MasterAlertSettingModel.update_at.set(datetime.now()),
            ]
        )

        return GetAlertSettingResponse(
            id=alert_setting.id,
            name=alert_setting.name,
            # display_settingは必要になったタイミングで追加予定
            attributes=AlertSettingAttribute(
                factor_setting=MasterService.decompress_factor_setting(
                    alert_setting.attributes.factor_setting
                )
            ),
            create_id=alert_setting.create_id,
            create_at=alert_setting.create_at,
            update_id=alert_setting.update_id,
            update_at=alert_setting.update_at,
            version=alert_setting.version,
        )
