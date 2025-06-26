from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.models.admin import AdminModel
from app.resources.const import UserRoleType
from app.schemas.master import (
    AlertSettingAttribute,
    CreateMasterRequest,
    CreateMasterRespose,
    GetAlertSettingResponse,
    GetBatchControlResponse,
    GetMasterByIdResponse,
    GetMastersMasterQuery,
    GetMastersResponse,
    GetServiceTypesQuery,
    GetServiceTypesResponse,
    GetSupporterOrganizationsQuery,
    GetSupporterOrganizationsResponse,
    UpdateMasterByIdQuery,
    UpdateMasterByIdRequest,
    UpdateMasterByIdResponse,
)
from app.service.master_service import MasterService

router = APIRouter()


@router.post(
    "/masters",
    tags=["Master"],
    description="マスターを一件作成します。",
    response_model=CreateMasterRespose,
    status_code=status.HTTP_200_OK,
)
def create_master(
    body: CreateMasterRequest,
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> CreateMasterRespose:
    """Post /master マスター登録API
    Args:
        body (CreateMasterRequest): 登録内容
        current_user (AdminModel): 認証済みユーザー

    Raises:
        HTTPException: 400

    Returns:
        CreateMasterRespose: 登録結果
    """
    return MasterService.create_master(item=body, current_user=current_user)


@router.get(
    "/masters",
    tags=["Master"],
    description="マスターの検索・一覧を取得します。 マスターメンテナンスの対象でないデータは取得しない。",
    response_model=GetMastersResponse,
    status_code=status.HTTP_200_OK,
)
def get_masters(
    query_params: GetMastersMasterQuery = Depends(),
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetMastersResponse:
    """Get /masters マスター一覧取得API
    Args:
        dataType (Enum): データ区分（すべて、支援者組織情報、サービス種別）
        limit (int): 最大取得件数
        offsetPage (int): リストの中で何ページ目を取得するか
        current_user (AdminModel): 認証済みユーザー

    Returns:
        GetMastersResponse: 取得結果
    """
    return MasterService.get_masters(
        dataType=query_params.data_type,
        limit=query_params.limit,
        offsetPage=query_params.offset_page,
        current_user=current_user,
    )


@router.get(
    "/masters/service-types",
    tags=["Master"],
    description="サービス種別の一覧を取得します。",
    response_model=GetServiceTypesResponse,
    status_code=status.HTTP_200_OK,
)
def get_service_types(
    query_params: GetServiceTypesQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetServiceTypesResponse:
    """Get /masters/service-types サービス種別一覧取得API

    Returns:
        GetServiceTypesResponse: 取得結果
    """
    return MasterService.get_service_types(disabled=query_params.disabled)


@router.get(
    "/masters/batch-control/{id}",
    tags=["Master"],
    description="各種集計バッチ処理の最終完了日時を取得します。",
    response_model=GetBatchControlResponse,
    status_code=status.HTTP_200_OK,
)
def get_batch_control_by_id(
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetBatchControlResponse:
    """Get /masters/batch-control/{id} バッチ処理最終完了日時取得API
    Args:
        id (str): バッチ処理関数ID パスパラメータで指定

    Returns:
        GetBatchControlResponse: 取得結果
    """
    return MasterService.get_batch_control_by_id(batch_id=id, current_user=current_user)


@router.get(
    "/masters/alert-setting",
    tags=["Master"],
    description="工数アラート設定を取得します。",
    response_model=GetAlertSettingResponse,
    status_code=status.HTTP_200_OK,
)
def get_alert_setting(
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetAlertSettingResponse:
    """Get /masters/alert-setting 工数アラート設定一覧取得API

    Returns:
        GetAlertSettingResponse: 取得結果
    """
    return MasterService.get_alert_setting()


@router.put(
    "/masters/alert-setting",
    tags=["Master"],
    description="工数アラート設定を更新します。",
    response_model=GetAlertSettingResponse,
    status_code=status.HTTP_200_OK,
)
def update_alert_setting(
    version: int,
    body: AlertSettingAttribute,
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.MAN_HOUR_OPS.key,
            }
        )
    ),
) -> GetAlertSettingResponse:
    """Update /masters/alert-setting 工数アラート設定更新API

    Returns:
        GetAlertSettingResponse: 取得結果
    """
    return MasterService.update_alert_setting(
        item=body, version=version, current_user=current_user
    )


@router.get(
    "/masters/supporter-organizations",
    tags=["Master"],
    description="支援者組織の一覧を取得します。",
    response_model=GetSupporterOrganizationsResponse,
    status_code=status.HTTP_200_OK,
)
def get_supporter_organization(
    query_params: GetSupporterOrganizationsQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSupporterOrganizationsResponse:
    """Get /masters/supporter-organizations 支援者組織一覧取得API

    Returns:
        GetSupporterOrganizationsResponse: 取得結果
    """
    return MasterService.get_supporter_organizations(disabled=query_params.disabled)


@router.get(
    "/masters/{master_id}",
    tags=["Master"],
    description="マスターをIDで一意に取得します。",
    response_model=GetMasterByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_master_by_id(
    master_id: str = Path(...),
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetMasterByIdResponse:
    """Get /masters/{data_type}/{master_id} マスター詳細取得API
    Args:
        master_id (str): マスターID パスパラメータで指定
        current_user (AdminModel): 認証済みユーザー

    Returns:
        GetMasterByIdResponse: 取得結果
    """
    return MasterService.get_master_by_id(
        master_id=master_id, current_user=current_user
    )


@router.put(
    "/masters/{master_id}",
    tags=["Master"],
    description="マスターをIDで一意に更新します。",
    response_model=UpdateMasterByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_master_by_id(
    body: UpdateMasterByIdRequest,
    query_params: UpdateMasterByIdQuery = Depends(),
    master_id: str = Path(...),
    current_user: AdminModel = Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> UpdateMasterByIdResponse:
    """Put /masters/{master_id} マスター更新API
    Args:
        body (UpdateMasterByIdRequest): 更新内容
        current_user (AdminModel): 認証済みユーザー

    Raises:
        HTTPException: 400

    Returns:
        UpdateMasterByIdResponse: 更新結果
    """
    return MasterService.update_master_by_id(
        master_id=master_id,
        version=query_params.version,
        item=body,
        current_user=current_user,
    )
