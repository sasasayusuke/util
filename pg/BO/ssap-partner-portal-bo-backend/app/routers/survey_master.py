from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.survey_master import (
    CopySurveyMastersByIdResponse,
    CreateSurveyMastersRequest,
    CreateSurveyMastersResponse,
    GetSurveyMasterByIdAndRevisionResponse,
    GetSurveyMastersByIdResponse,
    GetSurveyMastersQuery,
    GetSurveyMastersResponse,
    OriginRevisionQuery,
    PatchSurveyMasterRevisionByIdQuery,
    PatchSurveyMasterRevisionByIdResponse,
    UpdateSurveyMasterDraftByIdRequest,
    UpdateSurveyMasterDraftByIdResponse,
    UpdateSurveyMasterLatestByIdRequest,
    UpdateSurveyMasterLatestByIdResponse,
)
from app.service.survey_master_service import SurveyMasterService

router = APIRouter()


@router.post(
    "/survey-masters",
    tags=["SurveyMasters"],
    description="アンケートマスターを新規作成します。",
    response_model=CreateSurveyMastersResponse,
    status_code=status.HTTP_200_OK,
)
def create_survey_masters(
    body: CreateSurveyMastersRequest,
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> CreateSurveyMastersResponse:
    """Post /survey-masters アンケートマスター登録API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        CreateSurveyMastersResponse: 登録後の結果
    """

    return SurveyMasterService.create_survey_masters(
        item=body, current_user=current_user
    )


@router.get(
    "/survey-masters",
    tags=["SurveyMasters"],
    description="アンケートマスターの検索・一覧を取得します。",
    response_model=GetSurveyMastersResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_masters(
    query_params: GetSurveyMastersQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.MAN_HOUR_OPS.key,
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveyMastersResponse:
    """Get /survey-masters アンケートひな形一覧取得API
    Returns:
        GetSurveyMastersResponse: 取得結果
    """
    return SurveyMasterService.get_survey_masters(
        name=query_params.name,
        survey_type=query_params.survey_type,
        latest=query_params.latest,
        in_operation=query_params.in_operation,
        current_user=current_user,
    )


@router.get(
    "/survey-masters/{id}",
    tags=["SurveyMasters"],
    description="アンケートマスターをIDでバージョン順に一覧取得します。",
    response_model=GetSurveyMastersByIdResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_masters_by_id(
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveyMastersByIdResponse:
    """Get /survey-masters/{id} アンケートマスター単一取得API

    Args:
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveyMastersByIdResponse: 結果
    """

    return SurveyMasterService.get_survey_masters_by_id(
        survey_master_id=id, current_user=current_user
    )


@router.get(
    "/survey-masters/{id}/{revision}",
    tags=["SurveyMasters"],
    description="アンケートマスターをIDとバージョンで一意に取得します。",
    response_model=GetSurveyMasterByIdAndRevisionResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_master_by_id_and_revision(
    id: str = Path(...),
    revision: int = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SALES.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SURVEY_OPS.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveyMasterByIdAndRevisionResponse:
    """Get /survey-masters/{id}/{revision} アンケートマスターバージョン単一取得API

    Args:
        id (str): アンケートマスターID
        revision: アンケートマスターバージョン
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        GetSurveyMasterByIdAndRevisionResponse: 結果
    """

    return SurveyMasterService.get_survey_master_by_id_and_revision(
        survey_master_id=id, revision=revision, current_user=current_user
    )


@router.put(
    "/survey-masters/{id}",
    tags=["SurveyMasters"],
    description="アンケートマスターのIDで下書きバージョンを作成します。 既に下書きバージョンが存在する場合作成不可",
    response_model=CopySurveyMastersByIdResponse,
    status_code=status.HTTP_200_OK,
)
def copy_survey_masters_by_id(
    query_params: OriginRevisionQuery = Depends(),
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> CopySurveyMastersByIdResponse:
    """Put /survey-masters/{id} アンケートマスター下書き作成API

    Args:
        origin_revision (int): コピー元バージョン
        id (str): アンケートマスタID
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        CopySurveyMastersByIdResponse: 結果
    """

    return SurveyMasterService.copy_survey_masters_by_id(
        origin_revision=query_params.origin_revision,
        survey_master_id=id,
        current_user=current_user,
    )


@router.patch(
    "/survey-masters/{id}",
    tags=["SurveyMasters"],
    description="アンケートマスターのIDで下書きバージョンを最新バージョン（運用中）に更新します。",
    response_model=PatchSurveyMasterRevisionByIdResponse,
    status_code=status.HTTP_200_OK,
)
def patch_survey_masters_by_id(
    query_params: PatchSurveyMasterRevisionByIdQuery = Depends(),
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> PatchSurveyMasterRevisionByIdResponse:
    """Patch /survey-masters/{id} 下書きバージョンを最新バージョンへ切り替えるAPI

    Args:
        id (str): アンケートマスタID
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        PatchSurveyMasterRevisionByIdResponse: 結果
    """

    return SurveyMasterService.patch_survey_masters_by_id(
        version=query_params.version,
        survey_master_id=id,
        current_user=current_user,
    )


@router.put(
    "/survey-masters/{id}/latest",
    tags=["SurveyMasters"],
    description="アンケートマスターのIDで詳細情報を変更します。 最新バージョン（運用中）のものに対してだけ変更が行えます。",
    response_model=UpdateSurveyMasterLatestByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_survey_master_latest_by_id(
    body: UpdateSurveyMasterLatestByIdRequest,
    version: int,
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> UpdateSurveyMasterLatestByIdResponse:
    """Put /survey-masters/{id}/latest アンケートマスター詳細情報更新API

    Args:
        id (str): アンケートマスタID
        version (int): ロックキー（楽観ロック制御)
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateSurveyMasterLatestByIdResponse: 更新結果
    """

    return SurveyMasterService.update_survey_master_latest_by_id(
        item=body,
        version=version,
        survey_master_id=id,
        current_user=current_user,
    )


@router.put(
    "/survey-masters/{id}/draft",
    tags=["SurveyMasters"],
    description="アンケートマスターの下書きをIDで一意に更新します。 下書きバージョンのものに対してだけ変更が行えます。",
    response_model=UpdateSurveyMasterDraftByIdResponse,
    status_code=status.HTTP_200_OK,
)
def update_survey_master_draft_by_id(
    body: UpdateSurveyMasterDraftByIdRequest,
    version: int,
    id: str = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SYSTEM_ADMIN.key,
                UserRoleType.SURVEY_OPS.key,
            }
        )
    ),
) -> UpdateSurveyMasterDraftByIdResponse:
    """Put /survey-masters/{id}/draft アンケートマスター下書き更新API

    Args:
        id (str): アンケートマスタID
        version (int): ロックキー（楽観ロック制御)
        current_user (Behavior, optional): 認証済みのユーザー

    Returns:
        UpdateSurveyMasterDraftByIdResponse: 更新結果
    """

    return SurveyMasterService.update_survey_master_draft_by_id(
        item=body,
        version=version,
        survey_master_id=id,
        current_user=current_user,
    )
