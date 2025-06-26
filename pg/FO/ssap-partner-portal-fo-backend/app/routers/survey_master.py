from fastapi import APIRouter, Depends, Path, status

from app.auth.auth import get_current_user_factory
from app.resources.const import UserRoleType
from app.schemas.survey_master import (
    CheckAndGetSurveyMastersAnonymousByIdAndRevisionRequest,
    GetSurveyMastersByIdAndRevisionResponse,
    GetSurveyMastersQuery,
    GetSurveyMastersResponse,
    GetSurveyMasterOfSatisfactionByIdAndRevisionRequest,
    GetSurveyMasterOfSatisfactionByIdAndRevisionResponse,
)
from app.service.survey_master_service import SurveyMasterService

router = APIRouter()


@router.get(
    "/survey-masters",
    tags=["SurveyMasters"],
    description="最新バージョンのアンケートひな形の一覧を取得します。",
    response_model=GetSurveyMastersResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_masters(
    query_params: GetSurveyMastersQuery = Depends(),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
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
        survey_type=query_params.survey_type,
        in_operation=query_params.in_operation,
        current_user=current_user,
    )


@router.get(
    "/survey-masters/{id}/{revision}",
    tags=["SurveyMasters"],
    description="アンケートマスターをIDとバージョンで一意に取得します。",
    response_model=GetSurveyMastersByIdAndRevisionResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_masters_by_id_and_revision(
    id: str = Path(...),
    revision: int = Path(...),
    current_user=Depends(
        get_current_user_factory(
            accessible_roles={
                UserRoleType.CUSTOMER.key,
                UserRoleType.SUPPORTER.key,
                UserRoleType.SALES.key,
                UserRoleType.SUPPORTER_MGR.key,
                UserRoleType.SALES_MGR.key,
                UserRoleType.BUSINESS_MGR.key,
            }
        )
    ),
) -> GetSurveyMastersByIdAndRevisionResponse:
    """Get /survey-masters/{id}/{revision} アンケートマスター単一取得API

    Returns:
        GetSurveyMastersByIdAndRevisionResponse: 取得結果
    """
    return SurveyMasterService.get_survey_masters_by_id_and_revision(
        survey_master_id=id, revision=revision, current_user=current_user
    )


@router.post(
    "/survey-masters/anonymous/{id}/{revision}",
    tags=["SurveyMasters"],
    description="匿名アンケート用。アンケートマスターをIDとバージョンで一意に取得します。 パスワードとJWT(アンケートIDと有効期限から生成された)で認証します。",
    response_model=GetSurveyMastersByIdAndRevisionResponse,
    status_code=status.HTTP_200_OK,
)
def check_and_get_survey_masters_anonymous_by_id_and_revision(
    body: CheckAndGetSurveyMastersAnonymousByIdAndRevisionRequest,
    id: str = Path(...),
    revision: int = Path(...),
) -> GetSurveyMastersByIdAndRevisionResponse:
    """Post /survey-masters/{id}/{revision} 匿名アンケート用。アンケートマスター単一取得API

    Returns:
        GetSurveyMastersByIdAndRevisionResponse: 取得結果
    """
    return (
        SurveyMasterService.check_and_get_survey_masters_anonymous_by_id_and_revision(
            item=body, survey_master_id=id, revision=revision
        )
    )


@router.post(
    "/survey-masters/satisfaction/{id}/{revision}",
    tags=["SurveyMasters"],
    description="満足度評価のみ回答アンケート用。アンケートマスターをIDとバージョンでひな形情報を一意に取得し、満足度評価の設問のみフロントに返します。JWT(アンケートIDと有効期限（60日）から生成された)で認証します。",
    response_model=GetSurveyMasterOfSatisfactionByIdAndRevisionResponse,
    status_code=status.HTTP_200_OK,
)
def get_survey_master_satisfaction_by_id_and_revision(
    body: GetSurveyMasterOfSatisfactionByIdAndRevisionRequest,
    id: str = Path(...),
    revision: int = Path(...),
) -> GetSurveyMasterOfSatisfactionByIdAndRevisionResponse:
    """Post /survey-masters/satisfaction/{id}/{revision} 満足度評価のみ回答アンケート用。アンケートマスター単一取得API

    Returns:
        GetSurveyMasterOfSatisfactionByIdAndRevisionResponse: 取得結果
    """
    return SurveyMasterService.get_survey_master_satisfaction_by_id_and_revision(
        item=body, survey_master_id=id, revision=revision
    )
